using Castel.DataBaseContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Data;
using Castel.Models.Authentication;
using Castel.Extensions;
using Castel.Middlewares;
using Castel.Core.Unit;
using Castel.Mapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Castel.Specification;
using Castel.Specification.InvoiceItemSpecification;
using Castel.Specification.InvoiceSpecification;
internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigurationManager _appConfiguration = builder.Configuration; // allows both to access and to set up the config
        IWebHostEnvironment environment = builder.Environment;
        const string CorsPolicyName = "AllowOrigin";


        //Initialize DB Connection
        builder.Services.AddDbContext<StoreDbContext>(options =>
            options.UseSqlServer(_appConfiguration.GetConnectionString("AppDb"),
            sqlServerOptionsAction: sqlServerOptions =>
            {
                sqlServerOptions.CommandTimeout(60);
                sqlServerOptions.EnableRetryOnFailure
                (
                    maxRetryCount: 5
                );
            }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
            ServiceLifetime.Transient);

        //Initialize Identity System
        builder.Services.AddIdentity<StoreUser, StoreRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
        }).AddEntityFrameworkStores<StoreDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        // Add services to the container.
        builder.Services.AddAuthorization();

        builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        builder.Services.AddControllersWithViews(options =>
        {
            var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        }).AddViewOptions(options => {
            options.HtmlHelperOptions.ClientValidationEnabled = true;
        });

        builder.Services.AddScoped<IUserClaimsPrincipalFactory<StoreUser>, StoreUserClaimsPrincipalFactory>();
        builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        builder.Services.AddScoped(typeof(InvoiceItemFilterBuilder), typeof(InvoiceItemFilterBuilder));
        builder.Services.AddScoped(typeof(InvoiceMasterFilterBuilder), typeof(InvoiceMasterFilterBuilder));
        builder.Services.AddAutoMapper(typeof(MapperInit));

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: CorsPolicyName,
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseHsts();
        app.UseCors(CorsPolicyName);
        app.UseMiddleware(typeof(ExceptionHandlerMiddleWare));
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapDefaultControllerRoute();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Account}/{action=Login}/{id?}");
        await InitializeDbAsync(app);
        app.Run();
    }
    static async Task InitializeDbAsync(WebApplication host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
        await context.Database.MigrateAsync();
    }
}