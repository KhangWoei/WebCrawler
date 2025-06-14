using System.Net.Http.Headers;

namespace Crawler;

internal static class LinkVisitor
{
    public static async Task<string> VisitAsync(Uri uri, CancellationToken cancellationToken)
    {   
        var client = new HttpClient();
        var response = await client.GetAsync(uri, cancellationToken);
        
        if (response.IsSuccessStatusCode && response.Content.Headers.ContentType?.MediaType != "html")
        {
            return await client.GetStringAsync(uri, cancellationToken);
        }

        return string.Empty;
    }
}
