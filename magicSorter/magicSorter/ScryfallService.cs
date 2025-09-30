using magicSorter.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace magicSorter
{
   public class ScryfallService : IDisposable
   {

      private List<string> unrecognised = new();
      private readonly HttpClient scryfallHttpClient = new HttpClient
      {
         BaseAddress = new Uri("https://api.scryfall.com/")
      };

      public void Dispose() => scryfallHttpClient.Dispose();

      public ScryfallService() {
         scryfallHttpClient.DefaultRequestHeaders.Accept.Clear();
         scryfallHttpClient.DefaultRequestHeaders.Accept.Add(
             new MediaTypeWithQualityHeaderValue("application/json") { Quality = 0.9 });
         scryfallHttpClient.DefaultRequestHeaders.Accept.Add(
             new MediaTypeWithQualityHeaderValue("*/*") { Quality = 0.8 });

         scryfallHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
             "MyMagicApp/1.0 (https://mywebsite.example; mailto:me@example.com)");
      }
      public async Task<List<string>> GetUnrecognised()
      {
         var temp = unrecognised;
         unrecognised.Clear();
         return temp;
      }

      public async Task<List<ScryfallCard>> GetCards(List<string> cards)
      {
         var result = new List<ScryfallCard>();
         foreach (var card in cards)
         {
            List<ScryfallCard>? temp = await GetAllPrintingsByExactName(card);
            if (temp != null && temp.Count > 0)
            {
               var filteredTemp = temp.Where(x => x.Name.Equals(card));
               result.AddRange(filteredTemp);
            }
            else
            {
               unrecognised.Add(card);
            }
            await Task.Delay(150);
         }
         return result;
      }

      private async Task<ScryfallCard> GetCardByExactNameAsync(string exactName)
      {
         var encoded = Uri.EscapeDataString(exactName);
         var url = $"cards/named?exact={encoded}";

         using var res = await scryfallHttpClient.GetAsync(url);
         res.EnsureSuccessStatusCode();

         var json = await res.Content.ReadAsStringAsync();
         var card = JsonSerializer.Deserialize<ScryfallCard>(
             json,
             new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
         );

         return card;
      }

      public async Task<List<ScryfallCard>> GetAllPrintingsByExactName(string exactName)
      {
         string query = $"cards/search?order=released&unique=prints&q=!\"{Uri.EscapeDataString(exactName)}\"";
         try{
            PagedResult<ScryfallCard>? searchResult = await scryfallHttpClient.GetFromJsonAsync<PagedResult<ScryfallCard>>(query);
            return searchResult.Data;
         }
         catch
         {
            return null;
         }
      }

      public async Task<Dictionary<string, ScryfallSet>> GetSetDictionaryAsync(List<string> setCodes)
      {
         Dictionary<string, ScryfallSet> result = new();
         foreach (var setCode in setCodes)
         {
            var set = await GetSetAsync(setCode);
            if(set != null)
            {
               result.Add(setCode, set);
            }
            await Task.Delay(150);
         }
         return result;
      }


      public async Task<ScryfallSet> GetSetAsync(string setCode)
      {
         var url = $"sets/{Uri.EscapeDataString(setCode.ToLowerInvariant())}";
         using var res = await scryfallHttpClient.GetAsync(url);

         if (res.StatusCode == HttpStatusCode.NotFound)
         {
            return null; // unknown set code
         }

         res.EnsureSuccessStatusCode();

         var set = await res.Content.ReadFromJsonAsync<ScryfallSet>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

         return set;
      }
   }
}
