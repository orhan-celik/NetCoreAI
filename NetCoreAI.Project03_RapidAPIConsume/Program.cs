using NetCoreAI.Project03_RapidAPIConsume;
using Newtonsoft.Json;
using System.Net.Http.Headers;

List<Movie> seasons = new List<Movie>();

var client = new HttpClient();
var request = new HttpRequestMessage
{
    Method = HttpMethod.Get,
    RequestUri = new Uri("https://netflix54.p.rapidapi.com/season/episodes/?ids=80077209%2C80117715&offset=0&limit=25&lang=tr"),
    Headers =
    {
        { "x-rapidapi-key", "a62d3ea27fmshcc12485941ec67ep1abb7djsndcae9fa54c91" },
        { "x-rapidapi-host", "netflix54.p.rapidapi.com" },
    },
};
using (var response = await client.SendAsync(request))
{
    response.EnsureSuccessStatusCode();
    var body = await response.Content.ReadAsStringAsync();
    seasons = JsonConvert.DeserializeObject<List<Movie>>(body)!;
    foreach(var season in seasons)
    {
        Console.WriteLine($"Sezon : {season.Episodes[0].summary.season}");

        foreach (var episode in season.Episodes)
        {
            Console.WriteLine($"Bölüm : {episode.summary.episode}, Film : {episode.title}");
            Console.WriteLine($"Açıklama : {episode.contextualSynopsis.text}");
        }
    }
}

Console.ReadLine();