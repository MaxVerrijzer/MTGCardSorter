//using MtgApiManager.Lib.Model;
//using MtgApiManager.Lib.Service;
using magicSorter.Models;
using MtgApiManager.Lib.Model;

namespace magicSorter
{
   class Program
   {
      //private static GathererService GathererService = new GathererService();
      private static ScryfallService ScryfallServicetest = new ScryfallService();
      private static CardProcessor CardProcessor = new CardProcessor();

      static async Task Main(string[] args)
      {
         Console.WriteLine("Welcome to the most awesome card sorter test");
         try
         {
            List<string> cardNames = File.ReadAllLines("input.txt").ToList();
            string outputFileName = "result.txt";
            List<string> formattedCardData = new List<string>();
            string cardName = "Counterspell";
                //Models.ScryfallCard cardTest = await ScryfallServicetest.GetCardByExactNameAsync(cardName);
            List<ScryfallCard> testCards = await ScryfallServicetest.GetCards(cardNames);

            List<string> outputStrings = new();
            outputStrings.Add("All results:");
            foreach(var testCard in testCards)
            {
               outputStrings.Add($"{testCard.Name} - {testCard.SetCode} - {testCard.Rarity}");
            }
            outputStrings.Add("");
            outputStrings.Add("");

            Tuple<List<ScryfallCard>, List<ScryfallCard>> processedCards = CardProcessor.FilterRares(testCards);
            List<ScryfallCard> filtered = processedCards.Item1;
            var result = processedCards.Item2;

            outputStrings.Add("rares/mythics");
            foreach (ScryfallCard card in filtered)
            {
               var cardString = $"   {card.Name} - {card.SetCode} - {card.Rarity}";
               outputStrings.Add(cardString);
            }
            outputStrings.Add("-----------------------------");
            outputStrings.Add("");
            outputStrings.Add("");
            Dictionary<string, List<ScryfallCard>> setSortedCards = await CardProcessor.SortCardsIntoSets(result);
            foreach (var temp in setSortedCards)
            {
               outputStrings.Add(temp.Key);
               foreach(var c in temp.Value)
               {
                  outputStrings.Add($"   {c.Name} - {c.SetCode} - {c.Rarity}");
               }
            }

            File.AppendAllLines(outputFileName, outputStrings);
         }
         catch (IOException e)
         {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
         }
      }
   }
}








