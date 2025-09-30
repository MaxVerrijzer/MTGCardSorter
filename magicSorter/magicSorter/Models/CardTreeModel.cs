namespace magicSorter.Models
{
   public class CardTreeModel
   {
      public Dictionary<string, List<ScryfallCard>> SortedRares { get; set; }
      public List<CardTreeSet> CommonSets { get; set; }
      public List<RareCard> RaresNewSorted { get; set; }
   }

   public class CardTreeSet
   {
      public string SetName { get; set; }
      public DateTime? SetDate { get; set; }
      public List<CardTreeCard> Cards { get; set; }
   }

   public class CardTreeCard
   {
      public ScryfallCard Card { get; set; }
      public int Amount { get; set; }
   }

   public class RareCard
   {
      public string ColorIdentity { get; set; }
      public int ColorValue { get; set; }
      public List<CardTreeRare> RareCards { get; set; }
   }

   public class CardTreeRare
   {
      public int Amount { get; set; }
      public ScryfallCard RareCard { get; set; }
   }
}
