using AutoMapper;
using CleanSharpArchitecture.API.Extensions;
using CleanSharpArchitecture.Application.Hubs;
using CleanSharpArchitecture.Application.Services;
using CleanSharpArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    )
);

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Debug);

builder.Services.AddServiceConfigurations();
builder.Services.AddRepositoryConfigurations();
//builder.Services.AddAzureAdConfigurations(builder.Configuration);
builder.Services.AddJwtConfigurations(builder.Configuration);
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddBlobConfigurations(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AllowAnonymousFilter()); // DEV MODE ONLY
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.MaxDepth = 64;

});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => // with origins -> production
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.Use(async (context, next) =>
{
    await next();

    switch (context.Response.StatusCode)
    {
        case 401: // Unauthorized
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"message\": \"You must be logged in to make this request.\"}");
            break;
        case 404: // Not Found
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"message\": \"The request was not found.\"}");
            break;
        case 403: // Forbidden
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"message\": \"You do not have permission to make this request.\"}");
            break;
        case 500: // Internal Server Error
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"message\": \"An error occurred while processing your request.\"}");
            break;
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.Run();
