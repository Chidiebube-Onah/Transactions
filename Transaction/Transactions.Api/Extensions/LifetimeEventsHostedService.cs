namespace Transactions.Api.Extensions;

internal class LifetimeEventsHostedService : IHostedService
{
    private readonly ILogger<LifetimeEventsHostedService> _logger;
    private readonly IHostApplicationLifetime _appLifetime;

    public LifetimeEventsHostedService(
        ILogger<LifetimeEventsHostedService> logger,
        IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _appLifetime = appLifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(OnStarted);
        _appLifetime.ApplicationStopping.Register(OnStopping);
        _appLifetime.ApplicationStopped.Register(OnStopped);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStarted()
    {
        _logger.LogInformation("Application Started Successfully !");


        // Perform post-startup activities here
    }

    private void OnStopping()
    {
        _logger.LogInformation("Application is Stopping... ");

        // Perform on-stopping activities here

    }

    private void OnStopped()
    {
        _logger.LogInformation("Application Has Shutdown !");

        // Perform post-stopped activities here
    }
}