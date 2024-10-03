using BusinessLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string? clientId = builder.Configuration["ApiSettings:ClientId"];
string? clientSecret = builder.Configuration["ApiSettings:ClientSecret"];
builder.Services.AddSingleton((services) => new ApiService(clientId, clientSecret));

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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
