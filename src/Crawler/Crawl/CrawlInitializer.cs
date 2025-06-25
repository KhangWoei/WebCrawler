using Crawling;
using Frontier;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crawl.Crawl;

internal static class CrawlInitializer
{
    public static Task<int> Run(string seed, int depth, int width, CancellationToken cancellationToken)
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.UseFrontier();

        var host = builder.Build();

        var controller = host.Services.GetRequiredService<Controller>();
        controller.Start();
        
        return Task.FromResult(0);
    }
}