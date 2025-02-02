using AutoMapper;
using CleanSharpArchitecture.API.Extensions;
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
builder.Services.AddBlobConfigurations(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
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

    if (context.Response.StatusCode == 401)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"message\": \"You must be logged in to make this request.\"}");
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
app.Run();
