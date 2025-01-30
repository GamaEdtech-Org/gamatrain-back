using FluentValidation;
using FluentValidation.AspNetCore;
using GamaEdtech.DataSource.Cities;
using GamaEdtech.DataSource.Contries;
using GamaEdtech.DataSource.Schools;
using GamaEdtech.DataSource.States;
using GamaEdtech.DataSource.Utils;
using GamaEdtech.Domain.Cities;
using GamaEdtech.Domain.Countries;
using GamaEdtech.Domain.Schools;
using GamaEdtech.Domain.States;
using GamaEdtech.Gateway.RestApi.Utils;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConnectionString connectionString;

if (builder.Environment.IsDevelopment())
{
    connectionString = new ConnectionString(builder.Configuration.GetConnectionString("Default")!);
}
else
{
    connectionString = new ConnectionString(
        builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!
    );
    //connectionString = new ConnectionString(Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING")!);

    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString(
            "AZURE_REDIS_CONNECTIONSTRING"
        )!;
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
