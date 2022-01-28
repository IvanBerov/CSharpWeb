namespace TaskDemo
{
    public class StartUp
    {
        static async Task Main(string[] args)  
        {

            HttpClient httpClient = new HttpClient(); //създаваме HttpClient, за можем да работим с мрежата

            string url = "https://softuni.bg"; // Адресът, към който отиваме

            HttpResponseMessage httpResponse = await httpClient.GetAsync(url); //поискваме търсената иформация

            string result = await httpResponse.Content.ReadAsStringAsync(); // получаваме поисканата инфоормация

            Console.WriteLine(result);


            Console.WriteLine("Bac Ivan e Super !!!!!!!");  
        }
    }
}
