using System.Threading.Channels;

namespace Crawler.Queue;

internal sealed class ServiceQueue
{
    private readonly Channel<CrawlSource> _channel;

    public ServiceQueue()
    {
        _channel = Channel.CreateUnbounded<CrawlSource>();
    }

    public async ValueTask EnqueueAsync(CrawlSource message)
    {
        ArgumentNullException.ThrowIfNull(message);   
        
        await _channel.Writer.WriteAsync(message);
    }

    public async ValueTask<CrawlSource> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }

    public int Count() => _channel.Reader.Count;
}