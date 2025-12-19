using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRexamples.Data;
using SignalRexamples.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// add local signalR service
   builder.Services.AddSignalR();

// Connect to Azure signalR
//var connectionAzureSignalR = $"EndPoint={builder.Configuration.GetValue<string>("AzureEndPoint")};AccessKey={builder.Configuration.GetValue<string>("AzureAccessKey")};Version=1.0;";
//builder.Services.AddSignalR().AddAzureSignalR(connectionAzureSignalR);

// Connect to Azure signalR
var app = builder.Build();

//

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

// add hub route here

app.MapHub<UserHub>("/hubs/userCount");
app.MapHub<VotingHub>("/hubs/votingStatus");
app.MapHub<HouseGroupHub>("/hubs/HouseGroup");
app.MapHub<NotificationHub>("/hubs/notification");
app.MapHub<OrderHub>("/hubs/order");
app.MapHub<BasicChatHub>("/hubs/basicChat");
app.MapHub<GroupChatHub>("/hubs/groupChatHub");

app.Run();
