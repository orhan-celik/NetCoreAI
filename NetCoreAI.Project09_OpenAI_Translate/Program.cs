using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("OpenAI Translate Example");

        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var apiKey = config["ApiKey"];

        Console.Write("Çevirmek istediğiniz metni girin : ");
        string textToTranslate = Console.ReadLine();

        Console.Write("Hedef dili girin (örneğin 'tr' Türkçe için) : ");
        string targetLanguage = Console.ReadLine();

        if(!new List<string> { "tr", "en" }.Contains(targetLanguage))
        {
            Console.WriteLine("Geçersiz hedef dil. Sadece 'tr' veya 'en' kullanabilirsiniz.");
        }

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var requestBody = new
            {
                model = "gpt-3.5-turbo-instruct",
                prompt = $"Translate the following text to {targetLanguage}: {textToTranslate}",
                max_tokens = 1000,
                temperature = 0.7
            };
            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/completions", content);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Çeviri Sonucu: ");
            Console.WriteLine(responseContent);
        }

    }
}