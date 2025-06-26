using System.Threading.Channels;

namespace Frontier;

internal sealed class ServiceQueue
{
    private readonly Channel<Func<Task>> _channel;

    public ServiceQueue()
    {
        _channel = Channel.CreateUnbounded<Func<Task>>();
    }

    public async ValueTask QueueAsync(Func<Task> work, CancellationToken cancellationToken) => await _channel.Writer.WriteAsync(work, cancellationToken);

    public async ValueTask<Func<Task>> DequeueAsync(CancellationToken cancellationToken) =>
        await _channel.Reader.ReadAsync(cancellationToken);

    public int GetCount() => _channel.Reader.Count;
}