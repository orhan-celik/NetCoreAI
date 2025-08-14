using Microsoft.Extensions.Configuration;
using OpenAI_API;

class Program
{
    static async Task Main(string[] args)
    {

        try
        {
            var config = new ConfigurationBuilder()
                        .AddUserSecrets<Program>()   // Program sınıfının UserSecretsId'sini kullan
                        .Build();

            var apiKey = config["ApiKey"]; // UserSecrets'den API anahtarını al

            OpenAIAPI api = new OpenAIAPI(apiKey);

            var chat = api.Chat.CreateConversation();

            //chat.Model = Model.GPT4_Turbo;

            Console.WriteLine("Birşeyler sorun : ");
            string prompt = Console.ReadLine();
            
            chat.AppendUserInput(prompt);

            await foreach (var res in chat.StreamResponseEnumerableFromChatbotAsync())
            {
                Console.Write(res);
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.ReadLine();
        
    }
}