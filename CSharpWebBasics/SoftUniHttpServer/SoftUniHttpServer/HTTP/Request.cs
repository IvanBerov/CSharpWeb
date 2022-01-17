﻿namespace SoftUniHttpServer.HTTP
{
    public class Request
    {
        public Method Method { get; private set; }
        
        public string Url { get; private set; }

        public HeaderCollection Headers { get; private set; }

        public string Body { get; private set; }

        public static Request Parse(string request)
        {
            var lines = request.Split("\r\n");
            var startLine = lines.First()
                .Split(" ");
            var url = startLine[1];
            Method method = ParseMethod(startLine[0]);
            HeaderCollection headers = ParseHeaders(lines.Skip(1));

            var bodyLines = lines.Skip(headers.Count + 2).ToArray();
            var body = string.Join("\r\n", bodyLines);

            return new Request
            {
                Method = method,
                Url = url,
                Headers = headers,
                Body = body
            };
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> headerlines)
        {
            var headers = new HeaderCollection();

            foreach (var line in headerlines)
            {
                if (line == string.Empty)
                {
                    break;
                }

                var parts = line.Split(':', 2); //?

                if (parts.Length != 2)
                {
                    throw new InvalidOperationException("Request headers is not valid");
                }

                var headerName = parts[0];
                var headerValue = parts[1].Trim();

                headers.Add(headerName, headerValue);
            }

            return headers;
        }

        private static Method ParseMethod(string method)
        {
            try
            {
                return Enum.Parse<Method>(method);
            }
            catch (Exception)
            {

                throw new InvalidOperationException($"Method '{method}' is not supported");
            }
        }
    }
}