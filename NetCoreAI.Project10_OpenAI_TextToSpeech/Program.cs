using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {

        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var apiKey = config["ApiKey"];

        Console.Write("Lütfen metni girin: ");

        string text = Console.ReadLine();

        if (!string.IsNullOrEmpty(text))
        {
            Console.WriteLine("Metin başarıyla alındı. Ses dosyası oluşturuluyor...");

            string path = await ConvertTextToSpeech(text, apiKey);

            Console.WriteLine($"Ses dosyası başarıyla oluşturuldu: {path}");

            Console.WriteLine("Ses dosyasını dinlemek isterseniz evet yazabilirsiniz : ");

            string answer = Console.ReadLine();

            if(answer == "evet")
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }

        }

    }

    static async Task<string> ConvertTextToSpeech(string text, string apiKey)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var requestBody = new
            {
                model = "gpt-4o-mini-tts",
                input = text,
                voice = "shimmer",
                audio_format = "mp3"
            };
            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/audio/speech", content);

            if (response.IsSuccessStatusCode)
            {
                byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();
                string filePath = "output.mp3";
                await File.WriteAllBytesAsync(filePath, audioBytes);
                return filePath;
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Hata: {response.StatusCode}");
                Console.WriteLine(errorContent);
                throw new Exception($"API çağrısı başarısız: {response.StatusCode}");
            }

        }
    }
}