using System;
using System.Collections.Generic;

namespace GildedRose.Console
{
    public class Program
    {
        private const string Vest = "+5 Dexterity Vest";
        private const string Elixir = "Elixir of the Mongoose";
        private const string ConjuredManaCake = "Conjured Mana Cake";
        private const string AgedBrie = "Aged Brie";
        private const string BackstagePass = "Backstage passes to a TAFKAL80ETC concert";
        private const string Sulfuras = "Sulfuras, Hand of Ragnaros";

        IList<Item> Items;

        static void Main(string[] args)
        {
            var app = new Program()
                          {
                              Items = new List<Item>
                                          {
                                              new Item {Name = Vest, SellIn = 10, Quality = 20},
                                              new Item {Name = AgedBrie, SellIn = 2, Quality = 0},
                                              new Item {Name = Elixir, SellIn = 5, Quality = 7},
                                              new Item {Name = Sulfuras, SellIn = 0, Quality = 80},
                                              new Item
                                                  {
                                                      Name = BackstagePass,
                                                      SellIn = 15,
                                                      Quality = 20
                                                  },
                                              new Item {Name = ConjuredManaCake, SellIn = 3, Quality = 6}
                                          }

                          };

            app.UpdateQuality();

            System.Console.ReadKey();

        }

        public void SetItems(IList<Item> items)
        {
            Items = items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                item.Quality += QualityDelta(item);
                item.SellIn += SellInDelta(item);
                if (item.Name != Sulfuras)
                    item.Quality = Math.Max(0, Math.Min(item.Quality, 50));
            }
        }

        private int SellInDelta(Item item)
        {
            switch (item.Name)
            {
                case Sulfuras:
                    return 0;
                case AgedBrie:
                case BackstagePass:
                case Vest:
                case Elixir:
                case ConjuredManaCake:
                default:
                    return -1;
            }
        }

        private int QualityDelta(Item item)
        {
            switch (item.Name)
            {
                case Sulfuras:
                    return 0;
                case AgedBrie:
                    return 1;
                case BackstagePass:
                    if (item.SellIn <= 0)
                        return -1 * item.Quality;
                    if (item.SellIn <= 5)
                        return 3;
                    if (item.SellIn <= 10)
                        return 2;
                    return 1;
                case ConjuredManaCake:
                    return -2;
                case Vest:
                case Elixir:
                default:
                    if (item.SellIn <= 0)
                        return -2;
                    return -1;
            }
        }
    }

    public class Item
    {
        public string Name { get; set; }

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }

}
