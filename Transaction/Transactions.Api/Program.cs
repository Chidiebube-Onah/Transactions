using System.Reflection;
using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Transactions.Api.Extensions;
using Transactions.Data.Context;
using Transactions.Logger;
using Transactions.Services.Extensions;
using Transactions.Services.Handlers;
using Transactions.Services.Implementation;
using Transactions.Services.Interfaces;



var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGenNewtonsoftSupport();

string dbConnString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<TransactionsDbContext>(options =>
{
    options.UseSqlServer(dbConnString, s =>
    {
        s.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(20),
            errorNumbersToAdd: null);
    }).EnableSensitiveDataLogging(builder.Environment.IsDevelopment() || builder.Environment.IsStaging());

});

//Add automapper
builder.Services.AddAutoMapper(Assembly.Load("Transactions.Model"));

builder.Services.RegisterServices();
builder.Services.AddHostedService<LifetimeEventsHostedService>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers(setupAction => { setupAction.ReturnHttpNotAcceptable = true; }).AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());


    }).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

});
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transactions Micro-service", Version = "v1" });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        },
    });
});

 IAsyncPolicy<HttpResponseMessage>
    circuitBreakerPolicy =
        Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrTransientHttpStatusCode()
            .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));

builder.Services.AddHttpClient<ICryptoApiClient, CryptoApiClient>()
    .AddPolicyHandler(request =>
        
        circuitBreakerPolicy
            )
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.crypto.com/transactions"));


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TransactionConsumer>();

    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("UpdateTransactions", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<TransactionConsumer>(provider);
        });
    }));
});


var app = builder.Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(app.Services.GetService<IConfiguration>())
    .Destructure.AsScalar<JObject>()
    .Destructure.AsScalar<JArray>()
    .Enrich.FromLogContext()
    .CreateLogger();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transactions Micro-service v1");
        }
    );
}

app.UseSerilogRequestLogging(opts
    => opts.EnrichDiagnosticContext = LogEnricher.EnrichFromRequest);

app.ConfigureException(builder.Environment);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables();



try
{
    Log.Information("Starting  Application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly!");
}
finally
{
    Log.CloseAndFlush();
}
