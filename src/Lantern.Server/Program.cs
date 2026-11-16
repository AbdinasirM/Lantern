using Lantern.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Lantern.Server.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<OllamaService>();
builder.Services.AddScoped<PairingService>();
builder.Services.AddSingleton<DeviceTokenService>();
builder.Services.AddScoped<UserRegistrationService>();
builder.Services.AddScoped<DeviceAuthenticationService>();


builder.Services
    .AddAuthentication("DeviceToken")
    .AddScheme<AuthenticationSchemeOptions, DeviceTokenAuthenticationHandler>(
        "DeviceToken",
        options => { });

builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("LanternDatabase");
var sqliteBuilder=  new SqliteConnectionStringBuilder(connectionString);
var sqlitePath = sqliteBuilder.DataSource;
var absoluteSqlitePath = Path.GetFullPath(sqlitePath, builder.Environment.ContentRootPath);
sqliteBuilder.DataSource = absoluteSqlitePath;
builder.Services.AddDbContext<LanternDbContext>(options => options.UseSqlite(sqliteBuilder.ToString()));





var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LanternDbContext>();
    await dbContext.Database.MigrateAsync();


}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();
