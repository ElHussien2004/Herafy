using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Communications;
using Service.Specifications;
using ServiceAbstraction;
using Shared.ChatDTOS;
using Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ChatService(IUnitOfWork _unitOfWork, IMapper _mapper) : IChatService
    {
        public async Task<Result<int>> GetOrCreateChatAsync(string clientId, string technicianId)
        {
            var spec = new ChatWithPartiesSpecification(clientId, technicianId);
            var existingChat = await _unitOfWork.ChatRepo.GetByIdAsync(spec);

            if (existingChat != null)
                return existingChat.Id; 

            var newChat = new Chat
            {
                ClientId = clientId,
                TechnicianId = technicianId,
                LastMessageAt = DateTime.UtcNow,
                LastMessageContent = "بدأ المحادثة"
            };

            await _unitOfWork.ChatRepo.AddAsync(newChat);
            var result = await _unitOfWork.SaveAsync();

            if (result <= 0)
                return Error.Failure("Chat.CreateError", "فشل إنشاء المحادثة، حاول مرة أخرى");

            return newChat.Id;
        }
        public async Task<Result<IEnumerable<MessageDto>>> GetChatMessagesAsync(int chatId, int skip, int take)
        {
            var spec = new MessagesByChatIdSpecification(chatId, skip, take);
            var messages = await _unitOfWork.MessageRepo.GetAllAsync(spec);

            if (messages == null)
                return Error.NotFound("Chat.MessagesNotFound", "لا توجد رسائل في هذه المحادثة");

            var mappedMessages = _mapper.Map<IEnumerable<MessageDto>>(messages);
            return Result<IEnumerable<MessageDto>>.Ok(mappedMessages);
        }


        public async Task<Result<IEnumerable<ChatSummaryDto>>> GetUserChatsAsync(string userId)
        {
            
            var spec = new UserChatsWithIncludeSpecification(userId);
            var chats = await _unitOfWork.ChatRepo.GetAllAsync(spec);

            // تحويل الداتا لـ DTO مع تحديد مين "الطرف التاني" عشان نعرض اسمه وصورته
            var chatSummaries = chats.Select(c => new ChatSummaryDto
            {
                ChatId = c.Id,
                LastMessageAt = c.LastMessageAt,
                LastMessage = c.LastMessageContent,
                // لو أنا العميل، يبقى الطرف التاني هو الفني.. والعكس صحيح
               OtherPartyName = c.ClientId == userId ? c.Technician.User.FullName : c.Client.User.FullName,
               OtherPartyPicture = c.ClientId == userId ? c.Technician.User.ProfileImageURL : c.Client.User.ProfileImageURL,
               UnreadCount = c.Messages.Count(m => !m.IsRead && m.SenderId != userId)
            });

            return Result<IEnumerable<ChatSummaryDto>>.Ok(chatSummaries);
        }

        public async Task<Result<bool>> MarkAsReadAsync(int chatId, string currentUserId)
        {
            var spec = new UnreadMessagesSpecification(chatId, currentUserId);
            var unreadMessages = await _unitOfWork.MessageRepo.GetAllAsync(spec);

            if (unreadMessages == null || !unreadMessages.Any())
                return true;

            foreach (var msg in unreadMessages)
            {
                msg.IsRead = true;
                _unitOfWork.MessageRepo.Update(msg);
            }

            var result = await _unitOfWork.SaveAsync();

            if (result <= 0)
                return Error.Failure("Chat.UpdateReadError", "فشل تحديث حالة القراءة");

            return true;
        }
    }
}
