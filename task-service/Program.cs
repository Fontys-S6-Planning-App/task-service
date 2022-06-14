using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using task_service.DBContexts;
using task_service.Messaging;
using task_service.Repositories;
using task_service.Repositories.Interfaces;
using task_service.Services;
using task_service.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// set cors
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://planning-app.marktempelman.duckdns.org", "*")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Set connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TaskContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// Add services to the container.
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddHostedService<MessageReceiver>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = bool.Parse(builder.Configuration["Authentication:RequireHttpsMetadata"]);
    o.Authority = builder.Configuration["Authentication:Authority"];
    o.IncludeErrorDetails = bool.Parse(builder.Configuration["Authentication:IncludeErrorDetails"]);
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = bool.Parse(builder.Configuration["Authentication:ValidateAudience"]),
        ValidAudience = builder.Configuration["Authentication:ValidAudience"],
        ValidateIssuerSigningKey = bool.Parse(builder.Configuration["Authentication:ValidateIssuerSigningKey"]),
        ValidateIssuer = bool.Parse(builder.Configuration["Authentication:ValidateIssuer"]),
        ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],
        ValidateLifetime = bool.Parse(builder.Configuration["Authentication:ValidateLifetime"]),
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(name: "task", pattern: "task/{controller=Task}/{action=Index}/{id?}");

app.Run();