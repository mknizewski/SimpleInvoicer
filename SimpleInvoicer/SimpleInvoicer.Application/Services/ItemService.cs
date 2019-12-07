using SimpleInvoicer.Domain.Models;
using System.Collections.Generic;

namespace SimpleInvoicer.Application.Services
{
    public interface IItemService
    {
        Item GetEmptyItem(List<Item> currentItems);
        void SortOrder(List<Item> currentItems);
    }

    public class ItemService : IItemService
    {
        public Item GetEmptyItem(List<Item> currentItems)
        {
            return new Item
            {
                Order = currentItems.Count + 1,
                Unit = Units.Piece,
                Tax = ItemTax.Five
            };
        }

        public void SortOrder(List<Item> currentItems)
        {
            int index = 1;
            foreach (var item in currentItems)
                item.Order = index++;
        }
    }
}
