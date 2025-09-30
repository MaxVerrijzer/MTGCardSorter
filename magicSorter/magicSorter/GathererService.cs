using MtgApiManager.Lib.Model;
using MtgApiManager.Lib.Service;

namespace magicSorter
{
   public class GathererService
    {
        IMtgServiceProvider ServiceProvider = new MtgServiceProvider();
        ICardService Service;

        public GathererService()
        {
            Service = ServiceProvider.GetCardService();
        }

        public async Task<List<ICard>> GetCardsOfNames(List<string> cardNames)
        {
            List<ICard> cards = new List<ICard>();
            foreach (string cardName in cardNames)
            {
                var temp = await GetCardsOfName(cardName);
                if (temp != null)
                {
                    cards.AddRange(temp);
                }
            }
            return cards;
        }

        public async Task<List<ICard>> GetCardsOfName(string cardName)
        {
            List<ICard> cards = new List<ICard>();
            MtgApiManager.Lib.Core.IOperationResult<List<ICard>> result = await Service.Where(x => x.Name, cardName)
                                  .AllAsync();
            if (result.IsSuccess)
            {
                cards = result.Value;
            }
            else
            {
                //TODO: add filter
                Console.WriteLine($"can't find card - {cardName}");
                return null;
            }

            return cards;
        }


    }
}