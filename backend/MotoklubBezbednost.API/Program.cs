using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using MotoklubBezbednost.API.Mappings;
using MotoklubBezbednost.API.Options;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.Business.DependencyInjection;
using MotoklubBezbednost.Business.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MotoklubAppOptions>(builder.Configuration.GetSection(MotoklubAppOptions.SectionName));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<LocalAuthOptions>(builder.Configuration.GetSection(LocalAuthOptions.SectionName));

var motoklub = builder.Configuration.GetSection(MotoklubAppOptions.SectionName).Get<MotoklubAppOptions>() ?? new MotoklubAppOptions();
var dataDir = Path.Combine(builder.Environment.ContentRootPath, motoklub.DataDirectory);
Directory.CreateDirectory(dataDir);
var dbPath = Path.Combine(dataDir, motoklub.SqliteFileName);
var sqliteConnection = $"Data Source={dbPath}";

var jwt = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();

builder.Services.AddControllers(options =>
{
    if (motoklub.RequireAuthenticatedApi)
    {
        options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));
    }
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMotoklubPersistence(sqliteConnection);

builder.Services.AddAutoMapper(cfg => { }, typeof(BusinessMappingProfile), typeof(ApiMappingProfile));

builder.Services.AddMediatR(typeof(GetAllMembersQuery).Assembly);

var allowedOrigins = builder.Configuration["AllowedOrigins"]?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    ?? new[] { "http://localhost:3000" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (motoklub.AutoMigrate)
{
    app.Services.ApplyMotoklubMigrations();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

var spaEntry = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "index.html");
if (File.Exists(spaEntry))
{
    app.MapFallbackToFile("index.html");
}

app.Run();
