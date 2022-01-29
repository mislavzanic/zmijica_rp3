using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Snake
{
    using Coord = Tuple<int, int>;

    enum ItemType
    {
        Empty,
        Food,
        Snake,
        Wall,
        Teleport,
        Poison,
        Vegan
    } 

    internal class Board
    {
        private readonly List<List<ItemType>> board;
        private readonly Snek snake;

        private readonly int _Size;
        public int Size { get => _Size; }
        private ItemType[] SpecialFoods = new ItemType[] {
            ItemType.Teleport,
            ItemType.Poison,
            ItemType.Vegan
        };


        private Coord foodLocation;
        private Coord specialFoodLocation;

        private ItemType specialFoodType;
        private readonly Dictionary<ItemType, Color> specialFoodColors = new Dictionary<ItemType, Color>()
        {
            { ItemType.Teleport, (Color)Properties.Settings.Default["skipColor"] },
            { ItemType.Poison, (Color)Properties.Settings.Default["poisonColor"]},
            { ItemType.Vegan, (Color)Properties.Settings.Default["shrinkColor"]}
        };
        private readonly Random randomNumberGenerator = new Random();


        public Board(string filepath, out Snek snake)
        {
            board = Util.LoadMatrix(filepath);
            _Size = board.Count;

            var emptySpaces = FindAllOfType(ItemType.Empty);            
            snake = new Snek(emptySpaces[randomNumberGenerator.Next(emptySpaces.Count)]);
            this.snake = snake;
            board[snake.Head().Item1][snake.Head().Item2] = ItemType.Snake;
        }

        public void Render(BufferedGraphics myBuffer, float rW, float rH)
        {
            // food
            SolidBrush brush = new SolidBrush((Color)Properties.Settings.Default["foodColor"]);
            Util.FillRect(myBuffer.Graphics, brush, foodLocation, rW, rH, 0.2f);

            // random item
            if (specialFoodLocation != null)
            {
                brush.Color = specialFoodColors[specialFoodType];
                Util.FillRect(myBuffer.Graphics, brush, specialFoodLocation, rW, rH, 0.2f);
            }

            // walls
            brush.Color = (Color)Properties.Settings.Default["obstacleColor"];
            foreach(var wallLocation in FindAllOfType(ItemType.Wall))
            {
                Util.FillRect(myBuffer.Graphics, brush, wallLocation, rW, rH, 0.1f);
            }
            snake.Render(myBuffer, rW, rH);
        }

        private Coord InBounds(Coord coord) => new Coord((_Size + coord.Item1) % _Size, (_Size + coord.Item2) % _Size);

        public ItemType Update()
        {
            var newHeadXY = InBounds(new Coord(snake.Head().Item1 + snake.Direction.Item1, snake.Head().Item2 + snake.Direction.Item2));
            var itemAtNewHeadXY = board[newHeadXY.Item1][newHeadXY.Item2];
            
            if (itemAtNewHeadXY.In(SpecialFoods)) {
                specialFoodLocation = null;
            }

            board[newHeadXY.Item1][newHeadXY.Item2] = ItemType.Snake;
            var oldTailXY = snake.Move(newHeadXY);

            if (itemAtNewHeadXY == ItemType.Food)
            {
                snake.Body.Add(oldTailXY);
            }
            else
            {
                board[oldTailXY.Item1][oldTailXY.Item2] = ItemType.Empty;
            }

            if (itemAtNewHeadXY == ItemType.Vegan)
            {
                board[snake.Tail().Item1][snake.Tail().Item2] = ItemType.Empty;
                snake.Body.Remove(snake.Tail());
            }

            return itemAtNewHeadXY;
        }

        private List<Coord> FindAllOfType(ItemType itemtype)
        {
            var indices = new List<Coord>();

            return Enumerable.Range(0, _Size)
                             .Select(
                               i => Enumerable.Range(0, board[i].Count)
                                              .Where(j => board[i][j] == itemtype)
                                              .Select(j => new Coord(i, j))
                                              .ToList()
                              ).SelectMany(x => x).ToList();
        }
        public void GenerateFood(bool addSpecialFood)
        {
            var emptySpaces = FindAllOfType(ItemType.Empty);

            if (emptySpaces.Count <=0) { return; }

            foodLocation = emptySpaces[randomNumberGenerator.Next(emptySpaces.Count)];
            emptySpaces.Remove(foodLocation);
            board[foodLocation.Item1][foodLocation.Item2] = ItemType.Food;

            if (emptySpaces.Count == 1 || !addSpecialFood) { return; }

            specialFoodLocation = emptySpaces[randomNumberGenerator.Next(emptySpaces.Count)];
            specialFoodType = (ItemType) randomNumberGenerator.Next(
                (int) ItemType.Teleport,
                Enum.GetValues(typeof(ItemType)).Cast<int>().Max() + 1
                );
            board[specialFoodLocation.Item1][specialFoodLocation.Item2] = specialFoodType;
        }

    }
}
