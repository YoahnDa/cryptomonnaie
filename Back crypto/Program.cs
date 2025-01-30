using Backend_Crypto.Data;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Repository;
using Backend_Crypto.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ExternalApiService>();
builder.Services.AddHttpClient<ExternalApiService>(client =>
{
    client.BaseAddress = new Uri("http:localhost:5000/api/"); // Adresse de base de l'API externe
    client.DefaultRequestHeaders.Add("Accept", "application/json"); // Headers par défaut
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITokenValidator, TokenValidator>();
builder.Services.AddScoped<ICryptoRepository, CryptoRepository>();
builder.Services.AddScoped<IPorteFeuilleRepository, PorteFeuilleRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
