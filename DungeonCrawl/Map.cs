using System.Drawing;
using System.Numerics;
using System.Threading;
using System.Xml.Linq;

namespace DungeonCrawl
{
    internal class Monster
    {
        public string name;
        public Vector2 position;
        public int hitpoints;
        public char symbol;
        public ConsoleColor color;


        public Monster(string name, int hitpoints, char symbol, ConsoleColor color, Vector2 position)
        {
            this.name = name;
            this.hitpoints = hitpoints;
            this.symbol = symbol;
            this.color = color;
            this.position = position;
        }

        static Monster CreateMonster(string name, int hitpoints, char symbol, ConsoleColor color, Vector2 position)
        {

            return new Monster(name, hitpoints, symbol, color, position);
        }

        internal static Monster CreateRandomMonster(Random random, Vector2 position)
        {
            int type = random.Next(4);
            return type switch
            {
                0 => CreateMonster("Goblin", 5, 'g', ConsoleColor.Green, position),
                1 => CreateMonster("Bat Man", 2, 'M', ConsoleColor.Magenta, position),
                2 => CreateMonster("Orc", 15, 'o', ConsoleColor.Red, position),
                3 => CreateMonster("Bunny", 1, 'B', ConsoleColor.Yellow, position)
            };
        }

        internal static void ProcessEnemies(List<Monster> enemies, Map level, PlayerCharacter character, List<int> dirtyTiles, List<string> messages)
        {
            foreach (Monster enemy in enemies)
            {

                if (Program.GetDistanceBetween(enemy.position, character.position) < 5)
                {
                    Vector2 enemyMove = new Vector2(0, 0);

                    if (character.position.X < enemy.position.X)
                    {
                        enemyMove.X = -1;
                    }
                    else if (character.position.X > enemy.position.X)
                    {
                        enemyMove.X = 1;
                    }
                    else if (character.position.Y > enemy.position.Y)
                    {
                        enemyMove.Y = 1;
                    }
                    else if (character.position.Y < enemy.position.Y)
                    {
                        enemyMove.Y = -1;
                    }

                    int startTile = Program.PositionToTileIndex(enemy.position, level);
                    Vector2 destinationPlace = enemy.position + enemyMove;
                    if (destinationPlace == character.position)
                    {
                        // TODO: Random change for armor to protect?
                        int damage = 1;
                        damage -= PlayerCharacter.GetCharacterDefense(character);
                        if (damage <= 0)
                        {
                            damage = 1;
                        }
                        character.hitpoints -= damage;
                        messages.Add($"{enemy.name} hits you for {damage} damage!, {enemy.name} Hp: {enemy.hitpoints} ");
                    }
                    else
                    {
                        Map.Tile destination = Map.GetTileAtMap(level, destinationPlace);
                        if (destination == Map.Tile.Floor)
                        {
                            enemy.position = destinationPlace;
                            dirtyTiles.Add(startTile);
                        }
                        else if (destination == Map.Tile.Door)
                        {
                            enemy.position = destinationPlace;
                            dirtyTiles.Add(startTile);
                        }
                        else if (destination == Map.Tile.Wall)
                        {
                            // NOP
                        }
                    }
                }
            }
        }

    }
}