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
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()   
                           .AllowAnyMethod()   
                           .AllowAnyHeader();  
                });
            });

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
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            #region Profile AutoMapper
            builder.Services.AddAutoMapper(a => a.AddProfile(new ServicProfile()));
            builder.Services.AddAutoMapper(c=>c.AddProfile(new ClientProfile()));
            builder.Services.AddAutoMapper(c => c.AddProfile(new ServicProfile()));
            builder.Services.AddAutoMapper(c => c.AddProfile(new ServicProfile()));
            builder.Services.AddAutoMapper(c => c.AddProfile(new OrderProfile()));
            #endregion

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

            #region Add Admin
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Create Role
                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                // Create Admin User
                var user = await userManager.FindByEmailAsync("admin@test.com");

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = "admin@test.com",
                        Email = "admin@test.com",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(user, "P@ssw0rd");
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            #endregion

            // Configure the HTTP request pipeline.
            // Middleware
            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapGet("/", () => "Herafy API is running 🚀");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}