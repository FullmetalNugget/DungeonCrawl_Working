using System.Numerics;

namespace DungeonCrawl
{//test_2
    internal class Item
    {
        public string name;
        public int quality; // means different things depending on the type
        public Vector2 position;
        public ItemType type;

        public Item(string name, ItemType type, int quality, Vector2 position)
        {
            this.name = name;
            this.type = type;
            this.quality = quality;
            this.position = position;
        }

        static Item CreateItem(string name, ItemType type, int quality, Vector2 position)
        {

            return new Item(name, type, quality, position);
        }

        internal static Item CreateRandomItem(Random random, Vector2 position)
        {
            ItemType type = Enum.GetValues<ItemType>()[random.Next(4)];
            Item i = type switch
            {
                ItemType.Treasure => CreateItem("Book", type, 2, position),
                ItemType.Weapon => CreateItem("Sword", type, 3, position),
                ItemType.Armor => CreateItem("Helmet", type, 1, position),
                ItemType.Potion => CreateItem("Apple Juice", type, 1, position)
            };
            return i;
        }

    }
}