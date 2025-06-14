using Crawler.Harvesting;
using Crawler.Robots;

namespace Crawler;

public class WebCrawler
{
    private const int MaxDepth = 1;

    public async Task Crawl(string seed, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(seed);

        var robot = await RobotsHandler.GetDisallowedSites(uri, cancellationToken);

        var visitQueue = new Queue<Uri>();
        visitQueue.Enqueue(uri);

        var seen = new HashSet<Uri>();
        var depth = 0;
        while (visitQueue.Count > 0)
        {
            var width = visitQueue.Count;

            for (var i = 0; i < width; i++)
            {
                var current = visitQueue.Dequeue();
                seen.Add(current);
                Console.WriteLine($"Visiting: {current}");

                var html = await LinkVisitor.VisitAsync(current, cancellationToken);
                if (!string.IsNullOrEmpty(html))
                {
                    foreach (var links in LinkHarvester.Harvest(uri, html))
                    {
                        if (!seen.Contains(links) && !robot.Disallowed.Contains(links))
                        {
                            visitQueue.Enqueue(links);
                        }
                    }

                }

                Thread.Sleep(robot.Delay);
            }

            if (depth == MaxDepth)
            {
                break;
            }

            depth++;
        }
    }
}
