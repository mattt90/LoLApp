using Kunc.RiotGames.Api;
using Kunc.RiotGames.Lol.DataDragon;
using Kunc.RiotGames.Lol.GameClient;
using LolApp.BackgroundServices;
using LolApp.Data;
using LolApp.Lcu;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddSingleton<IWamp, Wamp>();
//builder.Services.AddLolLeagueClientUpdate();
builder.Services.AddLolDataDragon();
builder.Services.AddSingleton<ILolGameClient, LolGameClient>();
builder.Services.AddRiotGamesApi(c => c.ApiKey = builder.Configuration["RGAPIKEY"]!);

builder.Services.AddSingleton<ILcuClient, LcuClient>();
//builder.Services.AddSingleton<ILcuEventListener, LcuEventListener>();


builder.Services.AddHostedService<AutoAcceptQueueBackgroundServiceOld>();

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddDbContext<LeagueContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Sql")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LeagueContext>();
    context.Database.EnsureCreated();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();