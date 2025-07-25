using System.Threading.Channels;

namespace ServiceQueue;

internal sealed class ServiceQueue : IServiceQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _channel;

    public ServiceQueue(int capacity)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            // Async blocks producers if the channel is at full capacity
            FullMode = BoundedChannelFullMode.Wait
        };
        
        _channel = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
    }
    
    public async ValueTask QueueWorkAsync(Func<CancellationToken, ValueTask> work)
    {
        ArgumentNullException.ThrowIfNull(work);

        await _channel.Writer.WriteAsync(work);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
    {
        var work = await _channel.Reader.ReadAsync(cancellationToken);

        return work;
    }
}
