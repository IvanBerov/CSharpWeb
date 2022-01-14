﻿using System.Net;
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

            while (true)
            {
                var connection = serverListener.AcceptTcpClient();
                var networkStream = connection.GetStream();
                string request = ReadRequest(networkStream);
                Console.WriteLine(request);

                WriteResponse(networkStream, "Hello from the server!");

                //????????????
                //connection.Close();
            }
             
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

        private string ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new byte[bufferLength];
            var totalBytes = 0;

            var requesStringBuilder = new StringBuilder();

            do
            {
                var bytesRead = networkStream.Read(buffer, 0, bufferLength);
                totalBytes += bytesRead;

                if (totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large.");
                }

                requesStringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            while (networkStream.DataAvailable); //May not run correctly over the Internet

            return requesStringBuilder.ToString();
        }
    }
}
