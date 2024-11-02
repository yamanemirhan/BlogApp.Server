using BlogApp.API.Middlewares;
using BlogApp.Application;
using BlogApp.Application.Helpers;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Services;
using BlogApp.Domain.Repositories;
using BlogApp.Infrastructure.Data;
using BlogApp.Infrastructure.Repositories;
using BlogApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        //options.JsonSerializerOptions.MaxDepth = 32; 


        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.MaxDepth = 32;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = false; 
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddScoped<IPostTagRepository, PostTagRepository>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddApplication();

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("auth_token"))
            {
                context.Token = context.Request.Cookies["auth_token"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<JwtTokenGenerator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") 
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<TokenValidationMiddleware>();

app.UseWhen(
    context => (context.Request.Path.StartsWithSegments("/api/post") ||
                context.Request.Path.StartsWithSegments("/api/comment")) &&
               (context.Request.Method == HttpMethods.Put || context.Request.Method == HttpMethods.Delete),
    appBuilder => appBuilder.UseMiddleware<OwnershipOrAdminAuthorizationMiddleware>()
);


//app.MapWhen(
//    context => context.Request.Path.StartsWithSegments("/api/admin"),
//    builder => builder.UseMiddleware<AdminAuthorizationMiddleware>());



app.MapControllers();

app.Run();
