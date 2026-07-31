using System.Numerics;

namespace DungeonCrawl
{
    internal class Map
    {
        public enum Tile : sbyte
        {
            Floor,
            Wall,
            Door,
            Monster,
            Item,
            Player,
            Stairs
        }
        public int width;
        public int height;
        public Tile[] Tiles;

        internal static Map CreateMap(Random random)
        {
            Map level = new Map();

            level.width = Console.WindowWidth - Program.COMMANDS_WIDTH;
            level.height = Console.WindowHeight - Program.INFO_HEIGHT;
            level.Tiles = new Tile[level.width * level.height];

            // Create perimeter wall
            for (int y = 0; y < level.height; y++)
            {
                for (int x = 0; x < level.width; x++)
                {
                    int ti = y * level.width + x;
                    if (y == 0 || x == 0 || y == level.height - 1 || x == level.width - 1)
                    {
                        level.Tiles[ti] = Tile.Wall;
                    }
                    else
                    {
                        level.Tiles[ti] = Tile.Floor;
                    }
                }
            }

            int roomRows = 3;
            int roomsPerRow = 6;
            int boxWidth = (Console.WindowWidth - Program.COMMANDS_WIDTH - 2) / roomsPerRow;
            int boxHeight = (Console.WindowHeight - Program.INFO_HEIGHT - 2) / roomRows;
            for (int roomRow = 0; roomRow < roomRows; roomRow++)
            {
                for (int roomColumn = 0; roomColumn < roomsPerRow; roomColumn++)
                {
                    Map.AddRoom(level, roomColumn * boxWidth + 1, roomRow * boxHeight + 1, boxWidth, boxHeight, random);
                }
            }

            // Add enemies and items
            for (int y = 0; y < level.height; y++)
            {
                for (int x = 0; x < level.width; x++)
                {
                    int ti = y * level.width + x;
                    if (level.Tiles[ti] == Tile.Floor)
                    {
                        int chance = random.Next(100);
                        if (chance < Program.ENEMY_CHANCE)
                        {
                            level.Tiles[ti] = Tile.Monster;
                            continue;
                        }

                        chance = random.Next(100);
                        if (chance < Program.ITEM_CHANCE)
                        {
                            level.Tiles[ti] = Tile.Item;
                        }
                    }
                }
            }

            // Find starting place for player
            for (int i = 0; i < level.Tiles.Length; i++)
            {
                if (level.Tiles[i] == Tile.Floor)
                {
                    level.Tiles[i] = Tile.Player;
                    break;
                }
            }

            return level;
        }
        internal static void DrawTile(byte x, byte y, Tile tile)
        {
            Console.SetCursorPosition(x, y);
            switch (tile)
            {
                case Tile.Floor:
                    Program.Print(".", ConsoleColor.Gray); break;

                case Tile.Wall:
                    Program.Print("#", ConsoleColor.DarkGray); break;

                case Tile.Door:
                    Program.Print("+", ConsoleColor.Yellow); break;
                case Tile.Stairs:
                    Program.Print(">", ConsoleColor.Yellow); break;

                default: break;
            }
        }
        internal void DrawMap(List<int> dirtyTiles)
        {
            if (dirtyTiles.Count == 0)
            {
                Program.DrawMapAll(this);
            }
            else
            {
                foreach (int dt in dirtyTiles)
                {
                    byte x = (byte)(dt % width);
                    byte y = (byte)(dt / width);
                    Tile tile = Tiles[dt];
                    DrawTile(x, y, tile);
                }
            }
        }
        internal static Tile GetTileAtMap(Map level, Vector2 position)
        {
            if (position.X >= 0 && position.X < level.width)
            {
                if (position.Y >= 0 && position.Y < level.height)
                {
                    int ti = (int)position.Y * level.width + (int)position.X;
                    return level.Tiles[ti];
                }
            }
            return Tile.Wall;
        }
        internal static void AddRoom(Map level, int boxX, int boxY, int boxWidth, int boxHeight, Random random)
        {
            int width = random.Next(Program.ROOM_MIN_W, boxWidth);
            int height = random.Next(Program.ROOM_MIN_H, boxHeight);
            int sx = boxX + random.Next(0, boxWidth - width);
            int sy = boxY + random.Next(0, boxHeight - height);
            int doorX = random.Next(1, width - 1);
            int doorY = random.Next(1, height - 1);

            // Create perimeter wall
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int ti = (sy + y) * level.width + (sx + x);
                    if (y == 0 || x == 0 || y == height - 1 || x == width - 1)
                    {

                        if (y == doorY || x == doorX)
                        {
                            level.Tiles[ti] = Tile.Door;
                        }
                        else
                        {
                            level.Tiles[ti] = Tile.Wall;
                        }
                    }
                }
            }
        }
    }

}