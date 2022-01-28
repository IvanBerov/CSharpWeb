using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace DemoStateManagement
{
    public class StartUp
    {
        private static Dictionary<string, int> SessionStorage = new Dictionary<string, int>();
        const string NewLine = "\r\n";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            TcpListener listener = new TcpListener(IPAddress.Loopback, 9090);

            listener.Start();

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                await ProcessClientAsync(client);
            }
        }

        private static async Task ProcessClientAsync(TcpClient client)
        {
            var stream = client.GetStream();

            using (stream)
            {
                byte[] buffer = new byte[1024];
                var length = stream.Read(buffer, 0, buffer.Length);

                var requestString = Encoding.UTF8.GetString(buffer, 0, length);
                Console.WriteLine(requestString);

                var sid = Guid.NewGuid().ToString();
                var match = Regex.Match(requestString, @"sid=[^\n]*\r\n]");

                if (match.Success)
                {
                    sid = match.Value.Substring(4);
                }

                if (!SessionStorage.ContainsKey(sid))
                {
                    SessionStorage.Add(sid, 0);
                }

                SessionStorage[sid]++;

                bool sessionSet = requestString.Contains("sid =");

                // => подготвяме отговор към клиента под формата на четим формат
                string html = $"<h1>Hello from Ivan_Server {DateTime.Now} for the {SessionStorage[sid]} time</h1>" +
                              $"<form action=/tweet method=post><input name=username /><input name=password />" +
                              $"<input type=submit /></form>";

                string response = "HTTP/1.1 200 OK"
                                  + NewLine + "Server: IvanServer 2020"
                                  + NewLine +
                                  //"Location: https://www.google.com" + NewLine +
                                  "Content-Type: text/html; charset=utf-8"
                                  + NewLine +
                                  "X-Server-Version: 1.0"
                                  + NewLine + // => създаваме Cookie само при условие, че вече не е създадено
                                  (!sessionSet 
                                      ? ($"Set-Cookie: sid={sid}; lang=en; Expires: " + DateTime.UtcNow.AddHours(1).ToString("R")) 
                                      : string.Empty)
                                  + NewLine +

                                  // "Content-Disposition: attachment; filename=niki.txt" + NewLine +
                                  "Content-Length: " + html.Length
                                  + NewLine
                                  + NewLine + html // => тук добавяме по-горе подготвеният отговор към клиента
                                  + NewLine; // накрая трябва да има нов ред, за да не цикли браузъра

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

                Console.WriteLine($"sid={sid}");
                Console.WriteLine(new string('*', 70));
            }
        }
    }
}

