using System.Reflection;
using Serilog;
using TFA.App.Database;
using TFA.App.Extensions.Middleware;
using TFA.App.Extensions.ServiceCollection;
using TFA.App.Services;
using FluentValidation;
using TFA.App.API.Endpoints.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddMediatR(cfg
    => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureMsIdentity();
builder.Services.AddServices();

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    //app.ApplyMigrations();
}
app.MapEndpoints();
app.UseExceptionHandler();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.Run();
