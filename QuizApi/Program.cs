using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using QuizApi.Facades;
using QuizApi.Helpers;
using QuizApi.Interfaces;
using QuizApi.Models;
using QuizApi.Persistence;
using QuizApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.Configure<QuizboardsDatabaseSettings>(
    builder.Configuration.GetSection("QuizboardsDatabase"));
builder.Services.Configure<AccountDatabaseSettings>(
    builder.Configuration.GetSection("AccountDatabase"));

builder.Services.AddCors();

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<QuizboardsDatabaseSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<QuizboardsDatabaseSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IQuizboardRepository, QuizboardRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IImageUploadFacade, ImageUploadFacade>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token key not found");

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseCors(options 
    => options
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithOrigins("http://localhost:4200", "https://localhost:4200", "https://www.wir-sind-gbs.de"));

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
});

app.UseAuthentication();
app.UseAuthorization();

// app.UseHttpsRedirection();

app.MapControllers();

/* FOR DEBUGGING
using (var scope = app.Services.CreateScope())
{
    var svc = scope.ServiceProvider.GetRequiredService<IPhotoService>();
    svc.PrintSettings();
}
*/

app.Run();