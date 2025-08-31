using AutoMapper;
using Core.Mapper;
using Core.Service.Interface.Admin;
using Data;
using Domain;
using DrMoradi.MiddleWare;
using IOC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using PersonalSite.MiddleWare;
using Serilog;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Xml.Linq;
using Mapper = Core.Mapper.Mapper;

var builder = WebApplication.CreateBuilder(args);

#region SeriLog
var con = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(con).CreateLogger();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
#endregion

#region AutoMapper
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<Mapper>();
});
IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
#endregion

#region Context
builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connect"));
});
#endregion
builder.Services.AddHttpClient();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(
        new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys"))
    )
    .SetApplicationName(Assembly.GetExecutingAssembly().GetName().Name);


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.IsEssential = true;
    options.LoginPath = "/Smslogin";
    options.LogoutPath = "/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
});

builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All, }));
builder.Services.AddHttpContextAccessor();

RegisterServices(builder.Services);

// بارگذاری تنظیمات سایت از دیتابیس
using (var tempProvider = builder.Services.BuildServiceProvider())
{
    var settingService = tempProvider.GetRequiredService<ISetting>();
    var settingData = await settingService.GetSettingAsync();

    builder.Services.AddSingleton(new SiteSettingCache
    {
        Setting = settingData
    });
}

var app = builder.Build();


var keysFolderPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys");

try
{
    if (!Directory.Exists(keysFolderPath))
    {
        Directory.CreateDirectory(keysFolderPath);
        app.Logger.LogWarning("Keys folder did not exist, created: {Path}", keysFolderPath);
    }
    else
    {
        app.Logger.LogInformation("Keys folder exists: {Path}", keysFolderPath);
    }

    // لیست فایل‌های کلید
    var keyFiles = Directory.GetFiles(keysFolderPath, "key-*.xml");
    if (keyFiles.Length == 0)
    {
        app.Logger.LogWarning("No key files found in keys folder.");
    }
    else
    {
        foreach (var file in keyFiles)
        {
            try
            {
                var xml = XDocument.Load(file);
                var idAttr = xml.Root?.Attribute("id")?.Value ?? "(no id)";
                var creationDate = xml.Root?.Attribute("creationDate")?.Value ?? "(no date)";
                app.Logger.LogInformation("Key file: {File}, ID: {ID}, Created: {Date}", Path.GetFileName(file), idAttr, creationDate);
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "Error reading key file {File}", file);
            }
        }
    }

    // تست نوشتن
    var testFile = Path.Combine(keysFolderPath, "write_test.txt");
    File.WriteAllText(testFile, "test");
    if (File.Exists(testFile))
    {
        app.Logger.LogInformation("Write permission OK - test file created successfully.");
        File.Delete(testFile);
    }
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Error checking keys folder {Path}", keysFolderPath);
}

//app.UseTraceMiddleware();
app.UseMiddleware<ExeptionMiddleware>();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    var path = response.StatusCode switch
    {
        404 => "/NotFound",
        _ => null
    };
    if (path != null)
    {
        context.HttpContext.Response.Redirect(path);
    }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



// مسیرهای دیگر با احراز هویت و Permission
app.UseAuthentication();
app.UseAuthorization();
app.UsePermissionCheckMiddleware();
app.UseSessionLogMiddleware();
app.UseMiddleware<CheckFullNameMiddleware>();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();

void RegisterServices(IServiceCollection Services)
{
    UserDependencyContainer.RegisterServices(Services);
}
