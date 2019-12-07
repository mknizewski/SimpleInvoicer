using SimpleInvoicer.Domain.Models;
using System;
using System.Collections.Generic;

namespace SimpleInvoicer.Application.Services
{
    public interface IItemService
    {
        Item GetEmptyItem(List<Item> currentItems);
        void SortOrder(List<Item> currentItems);
        void CalulateItem(Item item);
    }

    public class ItemService : IItemService
    {
        public void CalulateItem(Item item)
        {
            if (item.Quantity == default)
                return;

            if (item.Price == default)
                return;

            item.Value = Math.Round(item.Quantity * item.Price, 2, MidpointRounding.AwayFromZero);
            item.TaxAmmount = Math.Round(item.Value * ((decimal)item.Tax / 100.0m), 2, MidpointRounding.AwayFromZero);
            item.ValueGross = Math.Round(item.Value + item.TaxAmmount, 2, MidpointRounding.AwayFromZero);
        }

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
