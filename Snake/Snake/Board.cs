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
            Pen pen = new Pen((Color)Properties.Settings.Default["foodOutline"]);
            Util.FillAndOutlineRect(myBuffer.Graphics, brush,  pen, foodLocation, rW, rH);

            // random item
            if (specialFoodLocation != null)
            {
                brush.Color = specialFoodColors[specialFoodType];
                Util.FillAndOutlineRect(myBuffer.Graphics, brush, pen, specialFoodLocation, rW, rH);
            }

            // walls
            brush.Color = (Color)Properties.Settings.Default["obstacleColor"];
            pen.Color = (Color)Properties.Settings.Default["obstacleOutline"];
            foreach (var wallLocation in FindAllOfType(ItemType.Wall))
            {
                Util.FillAndOutlineRect(myBuffer.Graphics, brush, pen, wallLocation, rW, rH);
            }
            snake.Render(myBuffer, rW, rH);
        }

        private Coord InBounds(Coord coord) => new Coord(InBounds(coord.Item1), InBounds(coord.Item2));
        private int InBounds(int x) => (_Size + x) % _Size;

        // returns -1 if infinite
        public int MaxSteps(Coord direction)
        {
            var head = snake.Head();
           
            if (direction.Item1 != 0)
            {
                return MaxStepsY(head, direction.Item1);
            }
            else if (direction.Item2 != 0)
            {
                return MaxStepsX(head, direction.Item2);
            }
            throw new ApplicationException("Received (0, 0) direction");
        }

        private int MaxStepsX(Coord start, int direction)
        {
            var row = board[start.Item1];

            for (var i = 1; i < row.Count; i++)
            {
                if (row[InBounds(start.Item2 + i * direction)].In(ItemType.Wall, ItemType.Snake))
                {
                    return i-1;
                }
            }

            return -1;
        }

        private int MaxStepsY(Coord start, int direction)
        {
            for (var i = 1; i < board.Count; i++)
            {
                if (board[InBounds(start.Item1 + i * direction)][start.Item2].In(ItemType.Wall, ItemType.Snake))
                {
                    return i-1;
                }
            }

            return -1;
        }

        public ItemType Update()
        {
            var oldTailXY = snake.Tail();
            var headXY = snake.Head();
            var newHeadXY = InBounds(new Coord(headXY.Item1 + snake.Direction.Item1, headXY.Item2 + snake.Direction.Item2));
            snake.Move(newHeadXY);
            headXY = newHeadXY;

            var itemAtHeadXY = board[headXY.Item1][headXY.Item2];
            
            if (itemAtHeadXY.In(SpecialFoods)) {
                specialFoodLocation = null;
            }

            board[headXY.Item1][headXY.Item2] = ItemType.Snake;

            if (itemAtHeadXY == ItemType.Food)
            {
                snake.Body.Add(oldTailXY);
            }
            else
            {
                board[oldTailXY.Item1][oldTailXY.Item2] = ItemType.Empty;
            }

            if (itemAtHeadXY == ItemType.Vegan)
            {
                var tailXY = snake.Tail();
                board[tailXY.Item1][tailXY.Item2] = ItemType.Empty;
                snake.Body.Remove(tailXY);
            }

            return itemAtHeadXY;
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
