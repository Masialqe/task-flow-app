using Serilog;
using TFA.App.Database;
using TFA.App.Domain.Models.Users;
using TFA.App.Extensions.Middleware;
using TFA.App.Extensions.ServiceCollection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureMsIdentity();

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.ApplyMigrations();
}

app.MapIdentityApi<User>();

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.Run();
