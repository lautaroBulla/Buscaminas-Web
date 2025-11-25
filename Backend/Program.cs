using Backend.Data;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("BuscaminasDBConnection")));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BuscaminasDBConnection")));


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "AuthScheme";
        options.DefaultChallengeScheme = "AuthScheme";
    })
    // AUTH TOKEN
    .AddJwtBearer("AuthScheme", options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Jwt_Access_Token"]!)),
            ValidateLifetime = true
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("Authentication"))
                {
                    context.Token = context.Request.Cookies["Authentication"];
                }
                return Task.CompletedTask;
            },
            OnTokenValidated = async context =>
            {
                var principal = context.Principal;
                if (principal == null)
                {
                    context.Fail("Principal is null");
                    return;
                }
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    context.Fail("Missing user id claim");
                    return;
                }
                var userId = Guid.Parse(userIdClaim.Value); 
                var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                var user = await authService.GetMeAsync(userId);
                if (user == null)
                {
                    context.Fail("Invalid token");
                    return;
                }
                return;
            }
        };
    })
    //REFRESH TOKEN
    .AddJwtBearer("RefreshScheme", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Jwt_Refresh_Token"]!)),
            ValidateLifetime = true
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("Refresh"))
                {
                    context.Token = context.Request.Cookies["Refresh"];
                }
                return Task.CompletedTask;
            },
            OnTokenValidated = async context =>
            {
                var principal = context.Principal;
                if (principal == null)
                {
                    context.Fail("Principal is null");
                    return;
                }

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    context.Fail("Missing user id claim");
                    return;
                }

                var userId = Guid.Parse(userIdClaim.Value); 
                var refreshTokenCookie = context.Request.Cookies["Refresh"]!;

                var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

                var user = await authService.VerifyUser(userId, refreshTokenCookie);
                if (user == null)
                {
                    context.Fail("Invalid refresh token");
                    return;
                }

                return;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Authenticated", policy =>
    {
        policy.AuthenticationSchemes.Add("AuthScheme");
        policy.RequireAuthenticatedUser();
    });

    options.AddPolicy("Refresh", policy =>
    {
        policy.AuthenticationSchemes.Add("RefreshScheme");
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5173",
                "https://buscaminassa.com",
                "https://www.buscaminassa.com"
                ) 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
