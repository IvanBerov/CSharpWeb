using SoftUniHttpServer.HTTP;

namespace SoftUniHttpServer.Responses
{
    public class TextFileResponse : Response
    {
        public TextFileResponse(string fileName)
            : base(StatusCode.OK)
        {
            FileName = fileName;

            this.Headers.Add(Header.ContentType, ContentType.PlainText);
        }

        public string FileName { get; init; }

        // Should set the correct headers and the body, if a file with the given name exists.
        public override string ToString()
        {
            if (File.Exists(FileName))
            {
                this.Body = File.ReadAllTextAsync(FileName).Result;

                var fileBytesCount = new FileInfo(FileName).Length;

                this.Headers.Add(Header.ContentLength, fileBytesCount.ToString());
                this.Headers.Add(Header.ContentDisposition, $"attachment; filename\"{this.FileName}\"");
            }
            return base.ToString();
        }
    }
}
