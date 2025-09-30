using System.Text.Json.Serialization;

namespace magicSorter.Models
{
   // using System.Text.Json.Serialization;
   public class ScryfallCard
   {
      [JsonPropertyName("id")]
      public string Id { get; set; }

      [JsonPropertyName("name")]
      public string Name { get; set; }

      [JsonPropertyName("mana_cost")]
      public string ManaCost { get; set; }

      [JsonPropertyName("type_line")]
      public string TypeLine { get; set; }

      [JsonPropertyName("oracle_text")]
      public string OracleText { get; set; }

      [JsonPropertyName("set")]
      public string SetCode { get; set; }

      [JsonPropertyName("rarity")]
      public string Rarity { get; set; }

      [JsonPropertyName("image_uris")]
      public ImageUris ImageUris { get; set; }

      [JsonPropertyName("colors")] 
      public List<string> Colors { get; set; }
      
      [JsonPropertyName("color_identity")] 
      public List<string> ColorIdentity { get; set; }
   }

   public class ImageUris
   {
      [JsonPropertyName("small")]
      public string Small { get; set; }
      [JsonPropertyName("normal")]
      public string Normal { get; set; }
      [JsonPropertyName("large")]
      public string Large { get; set; }
   }

   public class PagedResult<T>
   {
      [JsonPropertyName("data")]
      public List<T> Data { get; set; }

      [JsonPropertyName("has_more")]
      public bool HasMore { get; set; }

      [JsonPropertyName("next_page")]
      public string NextPage { get; set; }
   }

   public class ScryfallSet
   {
      [JsonPropertyName("id")]
      public Guid Id { get; set; }

      [JsonPropertyName("code")]
      public string Code { get; set; }
      [JsonPropertyName("name")]
      public string Name { get; set; }

      // Official set release date (YYYY-MM-DD). May be null for certain promos/unreleased sets.
      [JsonPropertyName("released_at")]
      public DateTime? ReleasedAt { get; set; }
   }

}
