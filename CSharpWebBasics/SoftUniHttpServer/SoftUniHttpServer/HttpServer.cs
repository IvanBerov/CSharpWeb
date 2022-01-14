using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SoftUniHttpServer
{
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;


        public HttpServer(string _ipAddress, int _port)
        {
            ipAddress = IPAddress.Parse(_ipAddress);
            port = _port;

            serverListener = new TcpListener(ipAddress, port);
        }

        public void Start() 
        {
            serverListener.Start();

            Console.WriteLine($"Server is listening on port {port}...");
            Console.WriteLine("Listening for request...");

            var connection = serverListener.AcceptTcpClient();
            var networkStream = connection.GetStream();

            WriteResponse(networkStream, "Hello from the server!");

            //????????????
            //connection.Close(); 
        }

        private void WriteResponse(NetworkStream networkStream, string message)
        {
            var contenLength = Encoding.UTF8.GetByteCount(message);

            string response = $@"HTTP/1.1 200 OK
Content-Type: text/plain; charset=UTF-8
Content-Length: {contenLength}

{message}";

            var responseBytes = Encoding.UTF8.GetBytes(response);

            networkStream.Write(responseBytes, 0, responseBytes.Length);
            //networkStream.Write(responseBytes);
        }
    }
}
