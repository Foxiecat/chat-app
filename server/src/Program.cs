using Scalar.AspNetCore;
using Serilog;
using src.extensions;

Log.Logger = new LoggerConfiguration()
    .CreateBootstrapLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.RegisterRepositories();
builder.Services.RegisterEndpoints();

builder.Services.AddJwtServices(builder.Configuration);
builder.Services.AddDatabase(builder.Configuration);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Chat App API");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();
