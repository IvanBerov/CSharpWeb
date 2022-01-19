﻿using SoftUniHttpServer;
using SoftUniHttpServer.HTTP;
using SoftUniHttpServer.Responses;

public class StartUp
{
    private const string HtmlForm = @"<form action='/HTML' method='POST'>
               Name: <input type='text' name='Name'/>
               Age: <input type='number' name ='Age'/>
               <input type='submit' value ='Save' />
            </form>";

    private const string DownloadForm = @"<form action='/Content' method='POST'>
                <input type='submit' value ='Download Sites Content' /> 
            </form>";

    private const string FileName = "content.txt";

    public static async Task Main(string[] args)
    {
        await DownloadSitesAsTextFile
            (StartUp.FileName, new string[] { "https://judge.softuni.org/", "https://softuni.bg/" });

        var server = new HttpServer(routes => routes
        .MapGet("/", new TextResponse("Hello from the server."))
        .MapGet("/Redirect", new RedirectResponse("https://softuni.bg"))
        .MapGet("/HTML", new HtmlResponse(StartUp.HtmlForm))
        .MapPost("/HTML", new TextResponse("", StartUp.AddFormDataAction))
        .MapGet("/Content", new HtmlResponse(StartUp.DownloadForm))
        .MapPost("/Content", new TextFileResponse(StartUp.FileName)));

        await server.Start();
    }

    private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
    {
        var downloads = new List<Task<string>>();

        foreach (var url in urls)
        {
            downloads.Add(DownloadWebSiteContent(url));
        }

        var response = await Task.WhenAll(downloads);

        var responseString = string.Join(Environment.NewLine + new String('-', 100), response);

        await File.WriteAllTextAsync(fileName, responseString);
    }

    private static async Task<string> DownloadWebSiteContent(string url)
    {
        var httpCliet = new HttpClient();
        using (httpCliet)
        {
            var response = await httpCliet.GetAsync(url);

            var html = await response.Content.ReadAsStringAsync();

            return html.Substring(0, 2000);
        }
    }

    private static void AddFormDataAction(Request request, Response response)
    {
        response.Body = "";

        foreach (var (key, value) in request.Form)
        {
            response.Body = $"{key} - {value}";
            response.Body = Environment.NewLine;
        }
    }
}
