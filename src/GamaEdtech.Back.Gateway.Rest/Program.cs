using FluentValidation;
using FluentValidation.AspNetCore;
using GamaEdtech.Back.DataSource.Cities;
using GamaEdtech.Back.DataSource.Contries;
using GamaEdtech.Back.DataSource.Schools;
using GamaEdtech.Back.DataSource.States;
using GamaEdtech.Back.DataSource.Utils;
using GamaEdtech.Back.Domain.Cities;
using GamaEdtech.Back.Domain.Countries;
using GamaEdtech.Back.Domain.Schools;
using GamaEdtech.Back.Domain.States;
using GamaEdtech.Back.Gateway.Rest.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using static CSharpFunctionalExtensions.Result;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var connectionStringValue =
    environment == "Development"
        ? configuration.GetConnectionString("Default")
        : configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");

ConnectionString connectionString = new ConnectionString(connectionStringValue);

if (builder.Environment.IsDevelopment())
{
    //connectionString = new ConnectionString(builder.Configuration.GetConnectionString("Default")!);
}
else
{
    //connectionString = new ConnectionString(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!);
    //connectionString = new ConnectionString(Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING")!);

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["AZURE_REDIS_CONNECTIONSTRING"];
        options.InstanceName = "SampleInstance";
    });
}

// Add services to the container.
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(
        options => options.InvalidModelStateResponseFactory = ModelStateValidator.Validate
    );
;

builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<Program>();
;

builder.Services.AddSingleton(connectionString);

//builder.Services.AddDbContext<GamaEdtechDbContext>(options =>
//	options.UseSqlServer(connectionStringValue!, x => x.UseNetTopologySuite()));
builder.Services.AddScoped<GamaEdtechDbContext>();
builder.Services.AddTransient<ISchoolRepository, SqlServerSchoolRepository>();
builder.Services.AddTransient<ICountryRepository, SqlServerCountryRepository>();
builder.Services.AddTransient<IStateRepository, SqlServerStateRepository>();
builder.Services.AddTransient<ICityRepository, SqlServerCityRepository>();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
