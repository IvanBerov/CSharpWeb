using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpDemo
{


    class StartUp
    {
        static async Task Main(string[] args)
        {
            // => за четене на кирилица
            Console.OutputEncoding = Encoding.UTF8;
            // => създаване на нов ред, независимо от вида на насрещния сървър или клиент
            const string NewLine = "\r\n";


            // => избираме на кой порт от нашия сървър да работим и да очакваме клиентска заявка
            // => активираме очакването на заявка
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 80);
            tcpListener.Start();

            while (true)
            {
                // => изграждаме клиент
                // => отваряме stream, кпйто клиента да използва  ===> винаги ползваме USING, когато имаме STREAM
                var client = tcpListener.AcceptTcpClient();
                var stream = client.GetStream();

                using (stream)
                {
                    // => създаваме масив от bytes, което да бъде цялата получена информация
                    // => изчисляваме дължината на масива
                    byte[] buffer = new byte[1024];
                    var lenght = stream.Read(buffer, 0, buffer.Length);

                    // => превръщаме масива от bytes във четим формат(string)
                    // => прочитаме изпратената от клиента информация
                    var requestString = Encoding.UTF8.GetString(buffer, 0, lenght);
                    Console.WriteLine(requestString);

                    // => подготвяме отговор към клиента под формата на четим формат (string)
                    string html = $"<h1>Hello from Berov_Server {DateTime.Now}</h1>" +
                            $"<form action=/tweet method=post><input name=username /><input name=password />" +
                            $"<input type=submit /></form>";

                    string response = "HTTP/1.1 200 OK"
                        + NewLine + "Server: NikiServer 2020"
                        + NewLine +
                        //"Location: https://www.google.com" + NewLine +
                        "Content-Type: text/html; charset=utf-8"
                        + NewLine +
                        // "Content-Disposition: attachment; filename=niki.txt" + NewLine +
                        "Content-Lenght: " + html.Length
                        + NewLine
                        + NewLine + html // => тук добавяме по-горе подготвеният отговор към клиента
                        + NewLine; // накрая трябва да има нов ред, за да не цикли браузъра

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