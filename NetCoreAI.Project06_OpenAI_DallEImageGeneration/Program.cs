using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("DALL-E Image Generation Example");

        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var apiKey = config["ApiKey"];

        Console.Write("Örnek prompt : ");
        string prompt = Console.ReadLine();

        using (HttpClient client = new HttpClient()) {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var requestBody = new {
                prompt = prompt,
                n = 1,
                size = "1024x1024"
            };

            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json,System.Text.Encoding.UTF8,"application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/images/generations", content);
            string responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseContent);

        }

    }
}