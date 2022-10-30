using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Clinic.Data.Data;
using Clinic.Infrastructure.Services;
using Clinic.Data.Entities;
using Clinic.Infrastructure.Mapper;
using Clinic.Web.Helper;
using Microsoft.AspNetCore.Authentication;
using Clinic.Web.Configurations;

namespace Clinic.Web
{
    public class Startup
    {
        CultureInfo[] supportedCultures = new CultureInfo[]
            {
                new CultureInfo("ar"),
                new CultureInfo("en"),
            };
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(supportedCultures[0], supportedCultures[0]);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();

            services.AddAuthentication(option =>
            {
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
             .AddCookie(options =>
             {
                 options.LoginPath = "/Login/";
                 options.AccessDeniedPath = "/AccessDenied/";
                 options.LogoutPath = "/Logout/";
             });

            services.ConfigureApplicationCookie(options =>
            {
                options.EventsType = typeof(XCookieAuthEvents);
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
                options.SlidingExpiration = true;
            });
            services.AddDistributedMemoryCache();
            services.AddCors();
            services.AddHttpClient();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromDays(356);
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".AdventureWorks.Session";
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });
            services.AddMemoryCache();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();

            services.AddIdentity<AppUser, RoleUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            services.AddTransient<IAppointmentTypeService, AppointmentTypeService>();

            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            services.AddControllersWithViews()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddAutoMapper(typeof(MapperProfile));
            services.AddAutoMapper(typeof(ProfileMapper));

            //services.AddSingleton(typeof(IStorageService), typeof(StorageService));
            //services.AddTransient(typeof(ISMSSender), typeof(SMSSender));
            services.AddScoped<XCookieAuthEvents>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // localization
            var localizationOption = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOption.Value);

            app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.MigrateAndSeedDb(development: true);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.MigrateAndSeedDb();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

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

