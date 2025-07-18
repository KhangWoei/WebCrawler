namespace ServiceQueue;

public sealed class Controller(IServiceQueue serviceQueue, ILogger<Controller> logger, IHostApplicationLifetime applicationLifetime)
{
    private readonly CancellationToken _cancellationToken = applicationLifetime.ApplicationStopping;

    public void Start()
    {
        logger.LogInformation("Service Queue starting");

        Task.Run(async () => await MonitorAsync(), _cancellationToken);
    }

    private async ValueTask MonitorAsync()
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            var keyStroke = Console.ReadKey();
            if (keyStroke.Key == ConsoleKey.W)
            {
                await serviceQueue.QueueWorkAsync(BuildWorkAsync);
            }
        }
    }

    private async ValueTask BuildWorkAsync(CancellationToken cancellationToken)
    {
        var delayLoop = 0;
        var guid = Guid.NewGuid();
        
        var random = new Random();
        var maxDelay = random.Next(3, 11);
        logger.LogInformation("Queued work item {Guid} is starting. With loops: {Loop}", guid, maxDelay);

        while (!cancellationToken.IsCancellationRequested && delayLoop < maxDelay)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(random.Next(0,4)), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if the Delay is cancelled
            }

            ++ delayLoop;

            logger.LogInformation("Queued work item {Guid} is running. {DelayLoop}/{MaxDelay}", guid, delayLoop, maxDelay);
        }

        if (delayLoop == maxDelay)
        {
            logger.LogInformation("Queued Background Task {Guid} is complete.", guid);
        }
        else
        {
            logger.LogInformation("Queued Background Task {Guid} was cancelled.", guid);
        }
    }
}