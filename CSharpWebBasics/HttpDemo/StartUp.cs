using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpDemo
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // => за четене на кирилица
            
            const string NewLine = "\r\n"; // => създаване на нов ред, независимо от вида на насрещния сървър или клиент

            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 80);

            tcpListener.Start(); // => активираме очакването на заявка

            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync(); // => изграждаме клиент

                var stream = client.GetStream(); // => отваряме stream, кпйто клиента да използва

                await using (stream)
                {
                    byte[] buffer = new byte[1024]; // => масив от bytes, което да бъде цялата получена информация

                    var lenght = stream.Read(buffer, 0, buffer.Length);

                    var requestString = Encoding.UTF8.GetString(buffer, 0, lenght); // => превръщаме масива от bytes във четим формат
                    Console.WriteLine(requestString);

                    // => подготвяме отговор към клиента под формата на четим формат (string)
                    string html = $"<h1>Hello from Berov_Server {DateTime.Now}</h1>" +
                            $"<form action=/tweet method=post><input name=username /><input name=password />" +
                            $"<input type=submit /></form>";

                    string response = "HTTP/1.1 200 OK"
                        + NewLine + "Server: IvanServer 2020"
                        + NewLine +
                        //"Location: https://www.google.com" + NewLine +
                        "Content-Type: text/html; charset=utf-8"
                        + NewLine +
                        // "Content-Disposition: attachment; filename=niki.txt" + NewLine +
                        "Content-Lenght: " + html.Length
                        + NewLine
                        + NewLine + html // => тук добавяме по-горе подготвеният отговор към клиента
                        + NewLine; // нов ред, за да не цикли браузъра

                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                    stream.Write(responseBytes);

                    Console.WriteLine(new string('=', 70));
                }
            }
        }

        public static async Task ReadData()
        {
            string url = "https://softuni.bg/courses/csharp-web-basics";

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(string.Join(Environment.NewLine, response.Headers.Select(x => x.Key + ": " + x.Value.First())));
            // var html = await httpClient.GetStringAsync(url);
            // Console.WriteLine(html);
        }
    }
}