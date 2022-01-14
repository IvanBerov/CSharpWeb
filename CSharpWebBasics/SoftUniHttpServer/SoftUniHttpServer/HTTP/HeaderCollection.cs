namespace SoftUniHttpServer.HTTP
{
    public class HeaderCollection
    {
        private readonly Dictionary<string, Header> headers;

        public HeaderCollection()
        {
            this.headers = new Dictionary<string, Header>();
        }

        public int Count => headers.Count;

        public void Add(string name, string value)
        {
            headers.Add(name, new Header(name, value));
        }
    }
}
