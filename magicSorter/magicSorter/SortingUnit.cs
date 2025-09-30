using magicSorter.Models;
using System.Text.RegularExpressions;

namespace magicSorter
{
   public class SortingUnit
   {
      private static readonly ScryfallService scryfallService = new();
      private static readonly CardProcessor cardProcessor = new();

      private List<ScryfallCard> CardsRaw = new();
      private List<string> Unrecognised = new();   
      private Dictionary<string, int> CardAmounts = new();
      private Dictionary<string, ScryfallSet> SetDictionary = new();
      private List<ScryfallCard> Rares = new();
      private List<ScryfallCard> Commons = new();
      private Dictionary<string, List<ScryfallCard>> SetSortedCards = new();
      private Dictionary<string, List<ScryfallCard>> SortedRares = new();
      private CardTreeModel CardTree = new(); 

      public async Task ProcessCards(List<string> cardNames)
      {      
         List<string> processedCardNames = await ProcessAmounts(cardNames.Distinct().ToList());
         CardsRaw = await scryfallService.GetCards(processedCardNames);
         Unrecognised = await scryfallService.GetUnrecognised();
         var processedCards = cardProcessor.FilterRares(CardsRaw);
         Rares = processedCards.Item1
            .GroupBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToList();
         Commons = processedCards.Item2;
         SortedRares = await cardProcessor.SortRaresIntoColors(Rares);


         List<RareCard> temp = cardProcessor.SortRaresIntoTree(Rares);
         AddAmountToRares(temp);


         SetSortedCards = await cardProcessor.SortCardsIntoSets(Commons);
         await FillSetDictionary();
      }

      public async Task AddCards(List<string> cardNames)
      {
         Unrecognised.Clear();
         List<string> processedCardNames = await ProcessAmounts(cardNames.Distinct().ToList());
         var newCards = await scryfallService.GetCards(processedCardNames);
         CardsRaw.AddRange(newCards);
         Unrecognised = await scryfallService.GetUnrecognised();
         var processedCards = cardProcessor.FilterRares(newCards);
         var newRares = processedCards.Item1
            .GroupBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToList();
         Dictionary<string, List<ScryfallCard>> newSortedRares = await cardProcessor.SortRaresIntoColors(newRares);
         Rares.AddRange(newRares);
         Rares = Rares.Distinct().ToList();
         var newSetSortedCards = await cardProcessor.SortCardsIntoSets(processedCards.Item2);
         await MergeDictionaries(newSortedRares, SortedRares);
         await MergeDictionaries(newSetSortedCards, SetSortedCards);
         foreach(var set in newSetSortedCards)
         {
            if(!SetDictionary.ContainsKey(set.Key)) {
               var newSet = await scryfallService.GetSetAsync(set.Key);
               SetDictionary.Add(set.Key, newSet);
            }
         }
      }

      public void AddAmountToRares(List<RareCard> treecCards)
      {
         foreach(var cards in treecCards)
         {
            foreach (CardTreeRare card in cards.RareCards)
            {
               card.Amount = CardAmounts[card.RareCard.Name];
            }
         }
      }

      public async Task RemoveCard(string cardName)
      {
         if (!CardAmounts.ContainsKey(cardName))
         {
            //error maybe?
            return;
         }

         if (CardAmounts[cardName] > 1)
         {
            CardAmounts[cardName] -= 1;
         }
         else
         {
            CardAmounts.Remove(cardName);
            Rares.RemoveAll(r => r.Name == cardName);
            Commons.RemoveAll(c => c.Name == cardName);
            CardsRaw.RemoveAll(c => c.Name == cardName);
         }
         
         await CreateSortedDictionaries();
      }

      public async Task CreateSortedDictionaries()
      {
         SortedRares = await cardProcessor.SortRaresIntoColors(Rares);
         SetSortedCards = await cardProcessor.SortCardsIntoSets(Commons);
      }

      public async Task GenerateCardTree()
      {
         CardTree.SortedRares = SortedRares;
         CardTree.CommonSets = new();
         foreach (var setCards in SetSortedCards)
         {
            var set = SetDictionary[setCards.Key];
            List<CardTreeCard> tempList = new();
            foreach (ScryfallCard card in setCards.Value)
            {
               tempList.Add(new CardTreeCard
               {
                  Card = card,
                  Amount = CardAmounts[card.Name]
               });
            }
            CardTree.CommonSets.Add(new CardTreeSet
            {
               SetName = set.Name,
               SetDate = set.ReleasedAt,
               Cards = tempList,
            });
         }
         List<RareCard> temp = cardProcessor.SortRaresIntoTree(Rares);
         AddAmountToRares(temp);
         CardTree.RaresNewSorted = temp;
      }

      public async Task<CardTreeModel> GetCardTree()
      {
         return CardTree;
      }

      private async Task MergeDictionaries(Dictionary<string, List<ScryfallCard>> newDict, Dictionary<string, List<ScryfallCard>> oldDict)
      {
         foreach(var kvp in newDict)
         {
            if(oldDict.TryGetValue(kvp.Key, out var list))
            {
               list.AddRange(kvp.Value);
            }
            else
            {
               oldDict.Add(kvp.Key, new List<ScryfallCard>(kvp.Value));
            }
         }
      }

      private async Task<List<string>> ProcessAmounts(List<string> cards)
      {
         List<string> result = new();
         foreach ( var card in cards)
         {
            var match = Regex.Match(card, @"^\s*(\d+)\s*(.+)$");
            if (match.Success)
            {
               int amount = int.Parse(match.Groups[1].Value);
               string cardName = match.Groups[2].Value.Trim();

               if (CardAmounts.TryGetValue(cardName, out int existing))
               {
                  CardAmounts[cardName] = existing + amount;
               } else {
                  this.CardAmounts.Add(cardName, amount);
                  result.Add(cardName);
               }
            }
         }
         return result;
      }

      public async Task<List<ScryfallCard>> GetCards()
      {
         return CardsRaw;
      }

      public async Task<List<string>> GetUnrecognised()
      {
         return Unrecognised;
      }

      public async Task<List<string>> GetFormattedCards()
      {
         List<string> outputStrings = new List<string>();
         outputStrings.AddRange(await GetFormattedRares());
         outputStrings.Add("");
         outputStrings.Add("Commons/Uncommons");
         foreach (var cardList in SetSortedCards)
         {
            string key;
            if (SetDictionary.ContainsKey(cardList.Key))
            {
               key = SetDictionary[cardList.Key].Name;
            }
            else
            {
               key = cardList.Key;
            }
            outputStrings.Add(key);
            foreach (var c in cardList.Value)
            {
               outputStrings.Add($"   {CardAmounts[c.Name]} {c.Name}");
            }
         }
         return outputStrings;
      }

      public async Task<List<string>> GetFormattedRares()
      {
         List<string> result = new()
         {
               "rares/mythics"
         };
         foreach (var rareList in SortedRares)
         {
            result.Add(rareList.Key);
            foreach(var c in rareList.Value)
            {
               result.Add($"   {CardAmounts[c.Name]} {c.Name} - {c.SetCode}");
            }
         }
         result.Add("-----------------------------");
         return result;
      }

      public async Task<List<string>> GetArchidectCards()
      {
         List<string> output = new();
         var cards = CardsRaw;
         foreach(ScryfallCard card in cards)
         {
            output.Add($"{1} {card.Name} ({card.SetCode})");
         }
         return output.Distinct().ToList();
      }

      public async Task Clear()
      {
         CardsRaw.Clear();
         Unrecognised.Clear();
         Rares.Clear();
         CardAmounts.Clear();
         SortedRares.Clear();
         Commons.Clear();
         SetDictionary.Clear();
         SetSortedCards.Clear();
         CardTree = new();
      }

      private async Task FillSetDictionary()
      {
         if(SetSortedCards.Count < 1)
         {
            return;
         }
         List<string> setcodes = SetSortedCards.Keys.ToList();
         SetDictionary = await scryfallService.GetSetDictionaryAsync(setcodes);
      }
   }
}
