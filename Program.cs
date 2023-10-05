using employeeDailyTaskRecorder.Data;
using Microsoft.EntityFrameworkCore;
using employeeDailyTaskRecorder.Seed;
using Hangfire;
using Hangfire.SqlServer;
using employeeDailyTaskRecorder.Hangfire;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
});

builder.Services.AddHangfire(configuration => configuration
.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();
builder.Services.AddTransient<ISendEmail, SendEmail>();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    seedData.Initialize(context);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();app.UseAuthorization();
app.MapControllerRoute(
    name: "default",

 pattern: "{controller=Auth}/{action=Index}/{id?}");
app.UseHangfireDashboard();
app.MapHangfireDashboard();
#pragma warning disable CS0618 // Type or member is obsolete
RecurringJob.AddOrUpdate<ISendEmail>(x => x.SendEmailToAdmin(), cronExpression: "0 0 14 * * ?");
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
RecurringJob.AddOrUpdate<ISendEmail>(x => x.SendEmailToEmployee(), cronExpression: "0 0 11 * * ?");
#pragma warning restore CS0618 // Type or member is obsolete
app.Run();


