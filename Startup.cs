using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using DentistCalendar.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DentistCalendar.Data.Entity;

namespace DentistCalendar
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, AppRole>(options => {
                options.User.RequireUniqueEmail = true; // e mail isteme
                //  options.User.AllowedUserNameCharacters = "abcddddd";
                options.SignIn.RequireConfirmedAccount = false; //e mail doðrulama
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Password.RequireDigit = false; //rakamlardan oluþsun mu zorunlu mu
                options.Password.RequiredLength = 6; //en az 6 uzunlukta olsun
                options.Password.RequireLowercase = false; //küçük karakter zorunlu mu
                options.Password.RequireUppercase = false; //büyük karakter zorunlu mu
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; //Kullanýcý oturuma sahip deðilse
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/Denied";//eriþim engeli varsa
                options.Cookie.Name = "Dentist.Cookie";
                options.SlidingExpiration = true; //sisteme giriþ yaptýðýnda süreyi tazeleyip tazelememe
            });

            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
