﻿using Crawler;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using Microsoft.Extensions.DependencyInjection;

namespace Crawl;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var seed = new Option<string>(name: "--seed", description: "Website to start crawling from.")
        {
            IsRequired = true
        };

        var services = new ServiceCollection();
        services.UserCrawler();

        var provider = services.BuildServiceProvider();

        var rootCommand = new RootCommand("Web crawler.");
        rootCommand.AddOption(seed);

        rootCommand.SetHandler(async s =>
        {
            var crawler = provider.GetRequiredService<WebCrawler>();
            await crawler.Crawl(s);
        }, seed);

        var builder = new CommandLineBuilder(rootCommand)
            .UseHelp()
            .UseDefaults()
            .Build();

        return await builder.InvokeAsync(args);
    }
}
