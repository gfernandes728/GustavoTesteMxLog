using GustavoTesteMxLog;
using GustavoTesteMxLog.Application.Interfaces;
using GustavoTesteMxLog.Application.Services;
using GustavoTesteMxLog.Application.UseCases.Auth.Login;
using GustavoTesteMxLog.Application.UseCases.Users.CreateUser;
using GustavoTesteMxLog.Application.UseCases.Users.DeleteUser;
using GustavoTesteMxLog.Application.UseCases.Users.GetDashboard;
using GustavoTesteMxLog.Application.UseCases.Users.GetUser;
using GustavoTesteMxLog.Application.UseCases.Users.UpdateUser;
using GustavoTesteMxLog.Infra.Data;
using GustavoTesteMxLog.Infra.Interfaces;
using GustavoTesteMxLog.Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly("GustavoTesteMxLog")));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IValidatorService, ValidatorService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(LoginQueryHandler).Assembly));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(DeleteUserCommandHandler).Assembly));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetDashboardQueryHandler).Assembly));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetUserQueryHandler).Assembly));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(UpdateUserCommandHandler).Assembly));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GustavoTesteMxLog API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(builder.Configuration["WebProject:Url"] ?? "")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors(builder.Configuration["WebProject:Cors"] ?? "");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await AppDbContextInitializer.SeedAsync(app.Services);

app.Run();
