using Microsoft.Extensions.Hosting;

namespace Crawler.Queue;

internal sealed class Worker(ServiceQueue queue): BackgroundService
{
    private readonly SemaphoreSlim _semaphore = new(Environment.ProcessorCount);
    private readonly HashSet<Guid> _inProgress = new();
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested && _inProgress.Count > 0 && queue.Count() > 0)
        {
            try
            {
                var work = await queue.DequeueAsync(cancellationToken);
                
                await _semaphore.WaitAsync(cancellationToken);
                
                var taskId = new Guid();
                lock (_inProgress)
                {
                    _inProgress.Add(taskId);
                }
                _ = Execute(taskId, work, cancellationToken);

            }
            catch (OperationCanceledException) { }
            
        }
    }

    private async Task Execute(Guid taskId, CrawlSource work, CancellationToken cancellationToken)
    {
        
        
        
        _semaphore.Release();
        lock (_inProgress)
        {
            _inProgress.Remove(taskId);
        }
    }
}