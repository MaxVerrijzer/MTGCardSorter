using magicSorter.Models;

namespace magicSorter
{
   public class CardProcessor
   {
      private static readonly Dictionary<string, string> colorNames = new()
      {
          { "W", "White" },
          { "U", "Blue" },
          { "B", "Black" },
          { "R", "Red" },
          { "G", "Green" }
      };

      private static Dictionary<char, int> colorValues = new()
      {
         { 'C', 0 },
         { 'W', 1 },
         { 'U', 2 },
         { 'B', 3 },
         { 'R', 4 },
         { 'G', 5 },
      };

      public CardProcessor() { }

      public async Task<Dictionary<string, List<ScryfallCard>>> SortCardsIntoSets(List<ScryfallCard> cards) {
         Dictionary<string, List<ScryfallCard>> result = cards
            .GroupBy(x => x.SetCode).ToList().ToDictionary(y => y.Key, y => y.GroupBy(c => c.Name).Select(cg => cg.First()).ToList());
         foreach(var cardLists in result)
         {
            cardLists.Value.GroupBy(x => x.Name).Select(s => s.First()).ToList();
         }
         return result;
      }

      public async Task<Dictionary<string, List<ScryfallCard>>> SortRaresIntoColors(List<ScryfallCard> rares)
      {
         Dictionary<string, List<ScryfallCard>> result = new();
         foreach( var rare in rares )
         {
            string key = "";
            if (rare.ColorIdentity.Count == 0)
            {
               key = "Colorless";
            }
            else
            {
               foreach (string color in rare.ColorIdentity)
               {
                  key += colorNames[color];
               }
            }

            if (result.ContainsKey(key))
            {
               result[key].Add(rare);
            }
            else
            {
               result.Add(key, new List<ScryfallCard> { rare });
            }
         }
         return result;
      }

      public List<RareCard> SortRaresIntoTree(List<ScryfallCard> rares)
      {
         Dictionary<string, List<ScryfallCard>> result = new();
         foreach (var rare in rares)
         {
            string key = "";
            if (rare.ColorIdentity.Count == 0)
            {
               key = "C";
            }
            else
            {
               foreach (string color in rare.ColorIdentity)
               {
                  key += color;
               }
            }

            if (result.ContainsKey(key))
            {
               result[key].Add(rare);
            }
            else
            {
               result.Add(key, new List<ScryfallCard> { rare });
            }
         }
         List<RareCard> temp = new();
         foreach (var sortedRares in result)
         {
            List<CardTreeRare> tempRares = new();
            foreach (var tempcard in sortedRares.Value)
            {
               tempRares.Add(new CardTreeRare
               {
                  RareCard = tempcard,
                  Amount = 0
               });
            }
            var tempval = 0;
            foreach (char letter in sortedRares.Key)
            {
               tempval += colorValues[letter];
            }
            tempval = (tempval + 1 )* sortedRares.Key.Length;
            temp.Add(new RareCard
            {
               ColorIdentity = sortedRares.Key,
               RareCards = tempRares,
               ColorValue = tempval
            });
         }

         var testsort = temp.OrderBy(x => x.ColorValue);
         temp.Sort((a, b) => a.ColorValue.CompareTo(b.ColorValue));

         return temp;
      }


      public Tuple<List<ScryfallCard>, List<ScryfallCard>> FilterRares(List<ScryfallCard> cards)
      {
         List<ScryfallCard> result = new List<ScryfallCard>();
         List<ScryfallCard> filtered = new List<ScryfallCard>();
         foreach (ScryfallCard card in cards)
         {
            if(card.Rarity.ToLower().Equals("rare") || card.Rarity.ToLower().Equals("mythic"))
            {
               filtered.Add(card);
            }
            else
            {
               result.Add(card);
            }
         }
         return new Tuple<List<ScryfallCard>, List<ScryfallCard>>(filtered, result);
      }
   }
}