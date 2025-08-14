﻿using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
                        .AddUserSecrets<Program>()   // Program sınıfının UserSecretsId'sini kullan
                        .Build();

        var apiKey = config["ApiKey"]; // UserSecrets'den API anahtarını al

        string audioFilePath = "audio1.mp3";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var form = new MultipartFormDataContent();

            var audioContent = new ByteArrayContent(File.ReadAllBytes(audioFilePath));
            audioContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/mpeg");
            form.Add(audioContent, "file", Path.GetFileName(audioFilePath));
            form.Add(new StringContent("whisper-1"), "model");

            Console.WriteLine("Transkript oluşturuluyor...");

            var response = await client.PostAsync("https://api.openai.com/v1/audio/transcriptions", form);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Transkript Başarılı: ");
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine($"Hata: {response.StatusCode}");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(errorContent);
            }
        }
    }
}