using Domain.Contracts;
using Domain.Entities.UsersEntity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Repositories;
using Service;
using Service.MappingProfiles;
using ServiceAbstraction;
using StackExchange.Redis;
using System.Text;
using System.Xml;

namespace Herafy.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<HerafyDbContext>();

            builder.Services.AddDbContext<HerafyDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISMSService,SMSService>();
            builder.Services.AddScoped<ITechnicianService, TechnicianService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddAutoMapper(a => a.AddProfile(new TechnicianProfile()));
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddAutoMapper(c=>c.AddProfile(new ClientProfile()));
            builder.Services.AddAutoMapper(c => c.AddProfile(new TechnicianProfile()));
            builder.Services.AddScoped(typeof(URLResolver<,>));
            builder.Services.AddSingleton<IConnectionMultiplexer>((_) =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnectionString"));
            }
            );
             builder.Services.AddAuthentication(Conf =>
             {
                 Conf.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                 Conf.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
             }).AddJwtBearer(Option =>
             {
                 Option.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateIssuer = true,
                     ValidIssuer = builder.Configuration["JWTOptions:issuer"],
                     ValidateAudience = true,
                     ValidAudience = builder.Configuration["JWTOptions:Audience"],
                     ValidateLifetime = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]))
                 };
             });


            var app = builder.Build();
            // Configure the HTTP request pipeline.
            // Middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}