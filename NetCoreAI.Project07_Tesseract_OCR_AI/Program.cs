using Tesseract;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Tesseract OCR ile Resim Üzerindeki karakterleri Okuma");

        try
        {

            Console.Write("Okunacak olan resmin yolunu girin : ");
            string imagePath = Console.ReadLine() ?? "image1.png"; // Okunacak resim dosyasının yolu
            string tessDataPath = @"C:\tessdata"; // Tesseract'ın dil verilerinin bulunduğu klasör
            // Tesseract OCR motorunu başlatma
            using (var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        string text = page.GetText();
                        Console.WriteLine("OCR Sonucu:");
                        Console.WriteLine(text);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Hata: {ex.Message}");
        }
    }
}
