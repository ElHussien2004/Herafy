using Shared.ChatDTOS;
using Shared.CommonResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IChatService
    {
        // الحصول على كل المحادثات الخاصة باليوزر الحالي (علشان لستة الشاتات اللي بره)
        Task<Result<IEnumerable<ChatSummaryDto>>> GetUserChatsAsync(string userId);

        // الحصول على رسائل شات معين مع Pagination (علشان لما يفتح الشات من جوه)
        Task<Result<IEnumerable<MessageDto>>> GetChatMessagesAsync(int chatId, int skip, int take);

        // التأكد من وجود شات بين العميل والفني، وإذا لم يوجد يتم إنشاؤه
        // بنحتاج دي لما العميل يضغط على زرار "تواصل مع الفني"
        Task<Result<int>> GetOrCreateChatAsync(string clientId, string technicianId);

        // تعليم الرسائل كـ "مقروءة"
        Task<Result<bool>> MarkAsReadAsync(int chatId, string currentUserId);
    }
}
