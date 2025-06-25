using Crawler.Harvesting;
using Crawler.LinkVisiting;

namespace Crawler;

public class WebCrawler(ILinkVisitor linkVisitor)
{
    public async Task Crawl(Configuration configuration, CancellationToken cancellationToken = default)
    {
        var seenSeeds = new HashSet<string>();
        var seeds = new Queue<string>();
        seeds.Enqueue(configuration.Seed);
        var currentWidth = 0;
        
        while (seeds.Count > 0 && currentWidth < configuration.Width)
        {
            var currentSeed = seeds.Dequeue();
            var source = await CrawlSource.Create(currentSeed, configuration.Depth, cancellationToken);

            while (source.Queue.TryDequeue(out var current, out var currentDepth) && currentDepth < source.Depth)
            {
                var html = await linkVisitor.VisitAsync(current, cancellationToken);
                if (!string.IsNullOrEmpty(html))
                {
                    foreach (var link in LinkHarvester.Harvest(html))
                    {
                        if (!source.CanVisit(link))
                        {
                            continue;
                        }

                        if (source.Seen.TryInsert(link))
                        {
                            source.Queue.Enqueue(link, currentDepth + 1);
                        }
                        else
                        {
                            // TODO: Normalize 
                            var newSeed = link.GetLeftPart(UriPartial.Authority);
                            
                            if (!seenSeeds.Add(newSeed))
                            {
                                seeds.Enqueue(link.GetLeftPart(UriPartial.Authority));
                            }
                        }
                        
                    }
                }

                Thread.Sleep(source.Robot.DelayMs);
            }

            currentWidth++;
        }
    }
}