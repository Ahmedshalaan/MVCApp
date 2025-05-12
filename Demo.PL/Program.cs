using Demo.BLL.Common.Services.Attachments;
using Demo.BLL.Common.Services.EmailSettings;
using Demo.BLL.Services.Departments;
using Demo.BLL.Services.Employees;
using Demo.DAL.Entities.Identity;
using Demo.DAL.Persistence.Data;
using Demo.DAL.Persistence.Repositories.Departments;
using Demo.DAL.Persistence.Repositories.Employees;
using Demo.DAL.Persistence.UnitOfWork;
using Demo.PL.Mapping;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL
{
	public class Program
    {
        // Entry Point
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            // Services LifeTime =>
            //  1. Singleton   => Per Application
            //  2. Scoped      => Per Request
            //  3. Transinet   => Per Operation


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            

            builder.Services.AddDbContext<ApplicationDbContext>((optionBuilder) =>
            {
                optionBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // ALLow DI For IDepartmentRepository

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IDepartmentService, DepartmentService>();

            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));

            builder.Services.AddTransient<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IEmailSetting, EmailSetting>();


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>((options) =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true; // @ # $
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 1;

                options.User.RequireUniqueEmail = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(5);

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); // PasswordSignInAsync Depend On AddDefaultTokenProviders Service 

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                options =>
                {
                    options.LogoutPath = "/Account/LogIn";
                    options.AccessDeniedPath = "/Home/Error";
                    options.LogoutPath = "/Account/LogIn";
				});

			#endregion

			var app = builder.Build();

            #region Configure Kestrel Middelwares
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}/{id?}"); 


            #endregion

            app.Run();
        }
    }
}
