using BasicWebServer.Demo.Models;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BasicWebServer.Demo.Controllers
{
    public class HomeController : Controller
    {
        private const string FileName = "content.txt";

        public HomeController(Request request)
            :base(request)
        {
        }

        public Response Index() => Text("Hello from the server!");

        public Response Redirect() => Redirect("https://softuni.bg");

        public Response Html() => View();

        public Response HtmlFormPost()
        {
            string name = Request.Form["Name"];

            string age = Request.Form["Age"];

            var model = new FormViewModel()
            {
                Name = name,
                Age = int.Parse(age)
            };

            return View(model);
        }

        public Response Content() => View();

        public Response DownloadContent()
        {
            DownloadSitesAsTextFile(HomeController.FileName, new[] { "https://softuni.bg/", "https://judge.softuni.org/" })
                .Wait();

            return File(HomeController.FileName);
        }

        private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContentAsync(url));
            }

            var responses = await Task.WhenAll(downloads);

            var responsesString = string.Join(Environment.NewLine + new string('-', 100), responses);

            await System.IO.File.WriteAllTextAsync(fileName, responsesString);
        }

        private static async Task<string> DownloadWebSiteContentAsync(string url)
        {
            var httpClient = new HttpClient();

            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);

                var html = await response.Content.ReadAsStringAsync();

                return html.Substring(0, 2000);
            }
        }

        public Response Cookies()
        {
            var requestHasCookies = Request.Cookies
                .Any(c => c.Name != Server.HTTP.Session.SessionCookieName);

            var bodyText = "";

            if (requestHasCookies)
            {
                var cookieText = new StringBuilder();

                cookieText
                    .AppendLine("<h1>Cookies</h1>");

                cookieText
                    .Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in Request.Cookies)
                {
                    cookieText
                        .Append("<tr>");

                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");

                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");

                    cookieText
                        .Append("</tr>");
                }

                cookieText.Append("</table>");

                bodyText = cookieText.ToString();

                return Html(bodyText);
            }
            else
            {
                bodyText = "<h1>Cookies set!</h1>";
            }

            var cookies = new CookieCollection();

            if (!requestHasCookies)
            {
                cookies.Add("My-Cookie", "My-Value");
                cookies.Add("My-Second-Cookie", "My-Second-Value");
            }

            return Html(bodyText, cookies);
        }

        public Response Session()
        {
            var sessionExists = Request.Session.ContainsKey(Server.HTTP.Session.SessionCurrentDateKey);

            var bodyText = "";

            if (sessionExists)
            {
                var currentDate = Request.Session[Server.HTTP.Session.SessionCurrentDateKey];

                bodyText = $"Stored date: {currentDate}!";
            }
            else
            {
                bodyText = "Current date stored!";
            }

            return Text(bodyText);
        }
    }
}
