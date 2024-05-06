using ApiCoreCrudAcciones.Data;
using ApiCoreCrudAcciones.Helpers;
using ApiCoreCrudAcciones.Repositories;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAzureClients(factory =>
{
    factory.AddSecretClient
    (builder.Configuration.GetSection("KeyVault"));
});


SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();

KeyVaultSecret secretK = await secretClient.GetSecretAsync("SecretKey"); //Aqui ponemos el nombre del secret
KeyVaultSecret audienceKey = await secretClient.GetSecretAsync("Audience");
KeyVaultSecret issuerKey = await secretClient.GetSecretAsync("Issuer");

string secretKey = secretK.Value;
string audience = audienceKey.Value;
string issuer = issuerKey.Value;

HelperActionServicesOAuth helper = new HelperActionServicesOAuth(secretKey, audience, issuer);


KeyVaultSecret secret =
    await secretClient.GetSecretAsync("SqlAzure");
string connectionString = secret.Value;


//HelperActionServicesOAuth helper = new HelperActionServicesOAuth(builder.Configuration);
builder.Services.AddSingleton<HelperActionServicesOAuth>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticateSchema()).AddJwtBearer(helper.GetJwtBearerOptions());

builder.Services.AddTransient<RepositoryAcciones>();
builder.Services.AddTransient<HelperAccion>();
//string connectionString = builder.Configuration.GetConnectionString("SqlAzure");
builder.Services.AddDbContext<AccionesContext>(options=>options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Crud Acciones",
        Description = "Crud Acciones"
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint(url: "/swagger/v1/swagger.json",
        name: "Api Crud Acciones v1");
    options.RoutePrefix = "";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
