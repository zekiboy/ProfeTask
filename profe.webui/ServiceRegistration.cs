using System;
using System.Globalization;
using System.Reflection;
using profe.webui.Bases;
using profe.webui.Data.Context;
using profe.webui.Data.Repositories;
using profe.webui.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using profe.webui.Data.EmailService;
using Microsoft.Extensions.Configuration;
using profe.webui.Data.Services;
using profe.webui.Data.Configurations;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using profe.webui.Exceptions;
using FluentValidation;

namespace profe.webui
{
	public static class ServiceRegistration
	{
        public static void AddServiceRegistration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<ExceptionMiddleware>();

            var assembly = Assembly.GetExecutingAssembly();

            serviceCollection.AddRulesFromAssemblyContaining(assembly, typeof(BaseRules));

            serviceCollection.AddValidatorsFromAssembly(assembly);
            //FluentValidation.DependencyInjectionExtensions
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("tr");


            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            serviceCollection.AddScoped<IEmailSender, SmtpEmailSender>(i =>
                  new SmtpEmailSender(
                      configuration["EmailSender:Host"],
                      configuration.GetValue<int>("EmailSender:Port"),
                      configuration.GetValue<bool>("EmailSender:EnableSSL"),
                      configuration["EmailSender:UserName"],
                      configuration["EmailSender:Password"])
            );



            serviceCollection.AddScoped<ICartRepository, CartRepository>();
            serviceCollection.AddScoped<ICartService, CartService>();
            serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
            serviceCollection.AddScoped<IProductRepository, ProductRepository>();
            serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            serviceCollection.AddIdentity<User, IdentityRole>(
                options => {
                    options.Stores.MaxLengthForKeys=128;

                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();
            //kullanıcıyı her sayfa geçişinde login sayfasına göndermemesi için, tarayıcısına cookie gönderiyoruz
            //TokenProviders parola resetleme işlemleri için benzersizbir id oluşturmak için kullanıyoruz

            serviceCollection.Configure<IdentityOptions>(options => {
                //PASSWORD
                options.Password.RequireDigit = true;
                //mutlaka sayıfal bir değer girmeli
                options.Password.RequireLowercase = true;
                //paralo içerisinde mutlaka küçük harf olmak zorunda
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;

                //LOCKOUT
                options.Lockout.MaxFailedAccessAttempts = 5;
                //parolayı max 5 defe yanlış girebilir
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //5dk sonra paraloya girmeye devam edebilir

                // options.User.AllowedUserNameCharacters="";
                options.User.RequireUniqueEmail = true;
                //her kullanıcının ayrı bi mail adresi olmalı mı
                options.SignIn.RequireConfirmedEmail = true;
                //onay maili
                options.SignIn.RequireConfirmedPhoneNumber = false;

            });

            //cookie base Authentication -- jwt ile vs farklarını araştır

            serviceCollection.ConfigureApplicationCookie(option => {
                option.LoginPath = "/account/login";
                option.LogoutPath = "/account/logout";

                //kullanıcının yetkisi olmayan sayfalara erişimi engeller
                option.AccessDeniedPath = "/account/accessdenied";

                //her işlem yaptıktan sonra logout olmak için 20dk var,
                //false ise işlemin önemi yok, loginden sonra 20 dk
                option.SlidingExpiration = true;

                //login olduktan sonra 365 günün var
                option.ExpireTimeSpan = TimeSpan.FromDays(365);

                option.Cookie = new CookieBuilder
                {
                    //sadece http talebi alabilsin
                    HttpOnly = true,
                    //tarayıcısı cookiesine varsayılan haricinde bir isim vermek için
                    Name = ".ProfeTask.Security.Cookie",
                    SameSite = SameSiteMode.Strict
                };


            });

        }



        //AddRules
        private static IServiceCollection AddRulesFromAssemblyContaining(
            this IServiceCollection services,
            Assembly assembly,
            Type type)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();

            foreach (var item in types)
                services.AddTransient(item);

            return services;
        }


    }
}

