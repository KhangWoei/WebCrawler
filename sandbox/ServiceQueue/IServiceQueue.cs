namespace ServiceQueue;

public interface IServiceQueue
{
    ValueTask QueueWorkAsync(Func<CancellationToken, ValueTask> work);
    
    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
}
