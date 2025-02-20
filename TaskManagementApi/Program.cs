using Serilog;
using Serilog.Extensions.Logging;
using TaskManagementApi.Middlewares;
using TaskManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
       .WriteTo.File("Logs/app.log")
       .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new SerilogLoggerProvider());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();