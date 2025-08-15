using Google.Cloud.Vision.V1;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Google Cloud Vision OCR ile Resim üzerindeki yazıları okuma");

        Console.Write("Resim yolunu giriniz:");
        string imagePath = Console.ReadLine() ?? "image1.png"; // Okunacak resim dosyasının yolu

        Console.WriteLine();

        string credentialPath = @"C:\Users\orhancelik\Desktop\my-ai-project-469105-05a07b5a8af1.json";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);

        try
        {
            var client = ImageAnnotatorClient.Create();
            var image = Image.FromFile(imagePath);
            var response = client.DetectText(image);
            Console.WriteLine("Resimdeki Metin:");
            Console.WriteLine();
            foreach (var annotination in response)
            {
                if (!string.IsNullOrEmpty(annotination.Description))
                {
                    Console.WriteLine(annotination.Description);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Bir hata oluştu {ex.Message}");
        }
    }
}