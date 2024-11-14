using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
           builder => builder.WithOrigins("http://localhost:3000")
                             .AllowAnyMethod()
                             .AllowAnyHeader());
});
builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<IUserService, UserService>(); 

// Register the PasswordHasher
builder.Services.AddSingleton<PasswordHasher<User>>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string? clientId = builder.Configuration["ApiSettings:ClientId"];
string? clientSecret = builder.Configuration["ApiSettings:ClientSecret"];
builder.Services.AddSingleton((services) => new ApiService(clientId, clientSecret));
builder.Services.AddDbContext<IApplicationDBContext, ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));
var app = builder.Build();

app.UseCors("AllowLocalhost3000");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
ApiService apiService = app.Services.GetService<ApiService>();
apiService.Init();
app.Run();
