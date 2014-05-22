using System.Collections.Generic;
using GildedRose.Console;
using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class TestAssemblyTests
    {
        private readonly Program _app = new Program();

        [Test]
        public void TestTheTruth()
        {
            Assert.IsTrue(true);
        }

        [TestCase("+5 Dexterity Vest", 10, 20, 10)]
        [TestCase("Elixir of the Mongoose", 5, 7, 5)]
        public void Normal_items_degrade_by_1(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].SellIn, Is.EqualTo(initialSellIn - runNTimes));
            Assert.That(items[0].Quality, Is.EqualTo(initialQuality - runNTimes));
        }

        [TestCase("+5 Dexterity Vest", 10, 20, 30)]
        [TestCase("Elixir of the Mongoose", 5, 7, 10)]
        public void Quality_of_item_is_never_less_than_0(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].Quality, Is.GreaterThanOrEqualTo(0));
        }

        [TestCase("+5 Dexterity Vest", 10, 20, 15)]
        [TestCase("Elixir of the Mongoose", 5, 10, 7)]
        public void When_sell_by_date_is_passed_quality_degrades_by_2(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            int qualityAtEndOfSellByDate = initialQuality - initialSellIn;
            int daysOverSellBy = runNTimes - initialSellIn;
            Assert.That(items[0].Quality, Is.EqualTo(qualityAtEndOfSellByDate - 2 * daysOverSellBy));
        }

        [TestCase("Sulfuras, Hand of Ragnaros", 0, 80, 1)]
        [TestCase("Sulfuras, Hand of Ragnaros", 0, 80, 50)]
        public void Sulfuras_never_changes(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].Quality, Is.EqualTo(80));
            Assert.That(items[0].SellIn, Is.EqualTo(0));
        }

        [TestCase("Aged Brie", 2, 0, 2)]
        public void Aged_brie_increases_by_1(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].Quality, Is.EqualTo(initialQuality + runNTimes));
        }

        [TestCase("Aged Brie", 2, 0, 51)]
        public void Aged_brie_never_grows_more_than_50(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].Quality, Is.EqualTo(50));
        }

        [TestCase("Backstage passes to a TAFKAL80ETC concert", 15, 20, 5)]
        public void Backstage_passes_increase_by_1_with_more_than_10_days_left(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].Quality, Is.EqualTo(initialQuality+runNTimes));
        }

        [TestCase("Backstage passes to a TAFKAL80ETC concert", 10, 20, 5)]
        public void Backstage_passes_increase_by_2_with_less_than_10_days_left(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].Quality, Is.EqualTo(initialQuality + 2 * runNTimes));
        }

        [TestCase("Backstage passes to a TAFKAL80ETC concert", 5, 20, 5)]
        public void Backstage_passes_increase_by_3_with_less_than_10_days_left(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].Quality, Is.EqualTo(initialQuality + 3 * runNTimes));
        }

        [TestCase("Backstage passes to a TAFKAL80ETC concert", 1, 20, 2)]
        public void Backstage_passes_quality_drops_to_0_after_sellby(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].Quality, Is.EqualTo(0));
        }

        [TestCase("Conjured Mana Cake", 3, 6, 3)]
        [TestCase("Conjured Mana Cake", 6, 10, 4)]
        public void Conjured_item_quality_decreases_by_2(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = BuildItemsAndRun(itemName, initialSellIn, initialQuality, runNTimes);
            Assert.That(items[0].Quality, Is.EqualTo(initialQuality - 2 * runNTimes));
        }

        private IList<Item> BuildItemsAndRun(string itemName, int initialSellIn, int initialQuality, int runNTimes)
        {
            var items = new List<Item>
            {
                new Item {Name = itemName, SellIn = initialSellIn, Quality = initialQuality},
            };
            _app.SetItems(items);
            UpdateQuality(runNTimes);
            return items;
        }

        private void UpdateQuality(int nTimes)
        {
            for (var i = 0; i < nTimes; i++)
            {
                _app.UpdateQuality();
            }
        }
    }
}