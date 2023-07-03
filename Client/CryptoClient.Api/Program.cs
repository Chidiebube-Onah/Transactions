using CryptoClient.Infrastructure.Model;
using CryptoClient.Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MessageQueueConfig>(builder.Configuration.GetSection($"{nameof(MessageQueueConfig)}"));
builder.Services.AddSingleton<MessageQueueConfig>();
builder.Services.AddSingleton<MessageQueueConfig>();
builder.Services.AddSingleton<CryptoClientService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddTransient<IMessageQueue, MessageQueue>();





MessageQueueConfig messageConfig = builder.Configuration.GetSection($"{nameof(MessageQueueConfig)}").Get<MessageQueueConfig>();



builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(new Uri(messageConfig.Server), h =>
        {
            h.Username(messageConfig.Username);
            h.Password(messageConfig.Password);
        });
    }));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{app.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables();

app.Run();
