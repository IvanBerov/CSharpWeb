using SoftUniHttpServer.Common;
using SoftUniHttpServer.HTTP;
using SoftUniHttpServer.Responses;

namespace SoftUniHttpServer.Routing
{
    internal class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<Method, Dictionary<string, Response>> routes;

        public RoutingTable() => this.routes = new()
        {
            [Method.Get] = new(),
            [Method.Post] = new(),
            [Method.Put] = new(),
            [Method.Delete] = new()
        };

        public IRoutingTable Map(string url, Method method, Response response)
            => method switch
            {
                Method.Get => this.MapGet(url, response),
                Method.Post => this.MapPost(url, response),
                _ => throw new InvalidOperationException($"Method '{method}' is not supported.")
            };

        public IRoutingTable MapGet(string url, Response response)
        {
            Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(response, nameof(response));

            this.routes[Method.Get][url] = response;

            return this;
        }

        public IRoutingTable MapPost(string url, Response response)
        {
            Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(response, nameof(response));

            this.routes[Method.Post][url] = response;

            return this;
        }

        public Response MatchRequest(Request request)
        {
            var requestedMethod = request.Method;
            var requestedUrl = request.Url;

            if (!this.routes.ContainsKey(requestedMethod)
                || !this.routes[requestedMethod].ContainsKey(requestedUrl))
            {
                return new NotFoundResponse();
            }

            return this.routes[requestedMethod][requestedUrl];
        }
    }
}
