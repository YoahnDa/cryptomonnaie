using Backend_Crypto.Data;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Repository;
using Backend_Crypto.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string firebaseCredentialsPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

Console.WriteLine($"🔍 Chemin des credentials Firebase : {firebaseCredentialsPath}");

try
{
    // Utilisation de Create() au lieu de CreateAsync()
    var firebase = FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(firebaseCredentialsPath)
    });
    Console.WriteLine("Connexion à Firebase réussie !");
}
catch (Exception ex)
{
    Console.WriteLine($"Erreur lors de l'initialisation de Firebase: {ex.Message}");
    throw;
}

builder.Services.AddScoped<MigrationServices>();
builder.Services.AddScoped<AnalytiqueCryptoService>();
builder.Services.AddScoped<UserAnalytique>();
builder.Services.AddScoped<EmailProvider>();
builder.Services.AddSingleton<IServiceScopeFactory>(sp => sp.GetRequiredService<IServiceScopeFactory>());
builder.Services.AddHostedService<CryptoUpdateServices>(); // Ajout du service en arrière-plan
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ExternalApiService>();
builder.Services.AddHttpClient<ExternalApiService>(client =>
{
    client.BaseAddress = new Uri("http://authentication-api:8000/api/");// Adresse de base de l'API externe
    client.DefaultRequestHeaders.Add("Accept", "application/json"); // Headers par défaut
});
builder.Services.AddLogging(); // Ajout du logger si nécessaire

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection"));
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITokenValidator, TokenValidator>();
builder.Services.AddScoped<ICryptoRepository, CryptoRepository>();
builder.Services.AddScoped<IPorteFeuilleRepository, PorteFeuilleRepository>();
builder.Services.AddScoped<IHistoriqueRepository, HistoriqueRepository>();
builder.Services.AddScoped<ITransactionRepository,TransactionRepository>();
builder.Services.AddScoped<IFavorisRepository,FavorisRepository>();
builder.Services.AddScoped<IAuthTokenRepository,AuthTokenRepository>();
builder.Services.AddScoped<IStockPortefeuilleRepository,StockPortefeuilleRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations and initialize the application
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    Console.WriteLine("🔄 Vérification et application des migrations...");
    dbContext.Database.Migrate();
    Console.WriteLine("✅ Migrations appliquées avec succès ! \n Hello !!! Et démarrage de la simulation.");

    // Exécuter la migration vers Firestore
    var migrationService = scope.ServiceProvider.GetRequiredService<MigrationServices>();
    await migrationService.MigrateDataIfNeeded();
    Console.WriteLine("✅ Migration vers Firestore terminée !");

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Démarrer le service en arrière-plan dès le lancement
app.Run();
