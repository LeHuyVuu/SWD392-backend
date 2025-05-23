using System.Text;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using DotNetEnv;
using SWD392_backend.Models;
using SWD392_backend.Services.UserService;
using SWD392_backend.Repositories.UserRepository;

var builder = WebApplication.CreateBuilder(args);

// Load env
Env.Load();

// Create connection string
var connectionString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                       $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Password={Environment.GetEnvironmentVariable("DB_PASS")};";

// Load cấu hình
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token theo định dạng: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:44376")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// DbContext
builder.Services.AddDbContext<DefaultdbContext>(options => options.UseNpgsql(connectionString));

// DI các Repository và Service
// Service
builder.Services.AddScoped<IUserService, UserService>();

// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Authentication + xử lý lỗi không có token
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };

        // ✅ Xử lý lỗi xác thực token (không có hoặc sai)
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(
                    "{\"status\":401,\"message\":\"Unauthorized: Invalid token\"}"
                );
            },
            OnChallenge = context =>
            {
                context.HandleResponse(); // Ngăn lỗi mặc định
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(
                    "{\"status\":401,\"message\":\"Unauthorized: Token is missing or expired\"}"
                );
            }
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication(); // ✅ Trước Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
