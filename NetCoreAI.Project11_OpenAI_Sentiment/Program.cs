using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

class Program
{

    protected static string apiKey;

    static async Task Main(string[] args)
    {
        Console.WriteLine("Open AI ile Duygu Durum Analizi : ");

        var config  = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        apiKey = config["apiKey"];

        Console.WriteLine("Bir metin giriniz : ");
        string text = Console.ReadLine();

        if (!string.IsNullOrEmpty(text))
        {
            Console.WriteLine("Duygu analizi yapılıyor...");
            string sentiment = await AnalyzeSentiment(text);

            Console.WriteLine($"Duygu Analizi : {sentiment}");

        }
    }

    static async Task<string> AnalyzeSentiment(string text)
    {

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var requestBody = new {
                model = "gpt-3.5-turbo",
                messages = new[] {
                    new{role="system",content="You are an AI that analyzes sentiment. You categorize text as Positive, Negative or Neutral."},
                    new {role="user",content=$"Analyze the sentiment of this text: \"{text}\" and return only Positive, Negative or Neutral" }
                }
            };

            string json = JsonConvert.SerializeObject(requestBody);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            string responseJson = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                return result.choices[0].message.content.ToString();
            }
            else
            {
                Console.WriteLine("Bir hata oluştu" + responseJson);
                return "Hata";
            }
        }
    }
}