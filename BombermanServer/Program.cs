var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();


app.MapHub<BombermanServer.GameHub>("/gamehub");

app.Run("http://0.0.0.0:5000");
