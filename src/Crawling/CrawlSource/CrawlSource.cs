﻿using Crawling.Robots;
using Crawling.TrieTree;

namespace Crawling.CrawlSource;

internal sealed class CrawlSource(IHttpClientFactory httpClientFactory, Uri uri, Robot robot, int depth) : ICrawlSource
{
    public string Source { get; } = uri.Host;
    public ITrie Seen { get; } = Trie.Create(uri);

    public PriorityQueue<Uri, int> Queue { get; } = new([(uri, -1)]);
    
    public IRobot Robot { get; } = robot;

    public int Depth { get; } = depth;

    public HttpClient CreateClient() => httpClientFactory.CreateClient(uri.Host);
     
    public bool CanVisit(Uri uriToVisit) => !Seen.Contains(uriToVisit) && !Robot.Disallowed.Contains(uriToVisit);
}