namespace SoftUniHttpServer.HTTP
{
    public class Header
    {
        public Header(string _name, string _value)
        {
            Name = _name;
            Value = _value;
        }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
