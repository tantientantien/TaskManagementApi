using AzureBlobStorage.WebApi.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagementApi.Data;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Middlewares;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using TaskManagementApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders().AddSerilog(Log.Logger, dispose: true);

// Add core services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.EnableAnnotations());

// Configure database connection
builder.Services.AddDbContext<TaskManagementContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnectionString")
    )
);

// Use Identity API Endpoints
builder.Services.AddIdentityApiEndpoints<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<TaskManagementContext>()
    .AddDefaultTokenProviders();


// Add CORS with a named policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              //.WithOrigins("https://localhost:44351))
              .SetIsOriginAllowed(_ => true);
    });
});

// Add other services
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IAzureService, AzureService>();


// Register repositories
builder.Services.AddScoped<IGenericRepository<TaskManagementApi.Models.Task>, TaskRepository>();
builder.Services.AddScoped<IGenericRepository<Label>, LabelRepository>();
builder.Services.AddScoped<IGenericRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IGenericRepository<TaskComment>, TaskCommentRepository>();
builder.Services.AddScoped<ITaskLabelRepository, TaskLabelRepository>();
builder.Services.AddScoped<ITaskAttachmentRepository, TaskAttachmentRepository>();

var app = builder.Build();

// Configure middleware pipeline
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Use Identity API Endpoints
app.MapIdentityApi<User>();

app.MapControllers();

// Ensure logging is properly disposed
app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();