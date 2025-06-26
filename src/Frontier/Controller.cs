using Crawling;
using Microsoft.Extensions.Hosting;

namespace Frontier;

public sealed class Controller (ServiceQueue serviceQueue, IHostApplicationLifetime applicationLifetime)
{
    private readonly CancellationToken _cancellationToken = applicationLifetime.ApplicationStopping;
    
    public void Start(Configuration configuration)
    {
        while (!_cancellationToken.IsCancellationRequested)
        {
            await serviceQueue.QueueAsync();
        }
    }
    
    private async 
}