using Microsoft.Extensions.Hosting;

namespace Frontier;

internal sealed class Worker(ServiceQueue queue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && queue.GetCount() != 0)
        {
            try
            {
                var work = await queue.DequeueAsync(cancellationToken);

                _ = work();
            }
            catch (OperationCanceledException)
            {
                
            }
        }
    }
}