﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

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
        private readonly List<Snek> otherSnakes = new List<Snek>();
        private readonly Coord[] possibleDirections = new Coord[]
            {
                new Coord(-1, 0),
                new Coord(1, 0),
                new Coord(0, -1),
                new Coord(0, 1)
            };

<<<<<<< HEAD
        public int Size { get => board.Count; }
        
        private readonly HashSet<ItemType> SpecialFoods = new HashSet<ItemType> {
=======
        private bool genFood;
        private readonly int _Size;
        public int Size { get => _Size; }
        public bool GenFood { get => genFood; }
        private ItemType[] SpecialFoods = new ItemType[] {
>>>>>>> cc7b9ac (rj bug sa dvije zmijice)
            ItemType.Teleport,
            ItemType.Poison,
            ItemType.Vegan
        };

        private ItemType specialFoodType;
        private readonly Dictionary<ItemType, Color> specialFoodColors = new Dictionary<ItemType, Color>()
        {
            { ItemType.Teleport, (Color)Properties.Settings.Default["skipColor"] },
            { ItemType.Poison, (Color)Properties.Settings.Default["poisonColor"]},
            { ItemType.Vegan, (Color)Properties.Settings.Default["shrinkColor"]}
        };
        private readonly Random randomNumberGenerator = new Random();


        public Board(string filepath, out Snek snake, out List<Snek> otherSnakes, uint othersCnt = 0)
        {
            board = Util.LoadMatrix(filepath);
<<<<<<< HEAD
=======
            genFood = false;
            _Size = board.Count;
>>>>>>> cc7b9ac (rj bug sa dvije zmijice)

            var emptySpaces = FindAllOfType(ItemType.Empty);

            this.snake = snake = createSnake(ref emptySpaces);

            for(var i=0; i<othersCnt; i++)
            {
                this.otherSnakes.Add(createSnake(ref emptySpaces));
            }
            otherSnakes = this.otherSnakes;
        }

        private Snek createSnake(ref List<Coord> emptySpaces)
        {
            var location = randomNumberGenerator.Next(emptySpaces.Count);
            var snake = new Snek(emptySpaces[location]);
            
            emptySpaces.RemoveAt(location);
            board[snake.Head().Item1][snake.Head().Item2] = ItemType.Snake;
           
            return snake;
        }

        public void Render(BufferedGraphics myBuffer, float rW, float rH)
        {
            // food
            SolidBrush brush = new SolidBrush((Color)Properties.Settings.Default["foodColor"]);
            Pen pen = new Pen((Color)Properties.Settings.Default["foodOutline"]);
            var foodLocation = FindFirstOfType(ItemType.Food);
            Util.FillAndOutlineRect(myBuffer.Graphics, brush,  pen, foodLocation, rW, rH);

            // special food
            var specialFoodLocation = MaybeFindFirstOfTypes(SpecialFoods);
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

            foreach(var otherSnake in otherSnakes)
            {
                otherSnake.Render(myBuffer, rW, rH, true);
            }
        }

        private Coord InBounds(Coord coord) => new Coord(InBounds(coord.Item1), InBounds(coord.Item2));
        private int InBounds(int x) => (Size + x) % Size;

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
            var toRemove = new List<int>();
            var snakeUpdateResult = updateSnake(snake);

            for (var i=0; i<otherSnakes.Count; ++i)
            {
                if (!updateOtherSnake(otherSnakes[i]))
                {
                    toRemove.Add(i);
                    //MessageBox.Show($"couldn't update snake {i}");
                }
                if (otherSnakes[i].Body.Count == 0)
                {
                    toRemove.Add(i);
                }
            }

            foreach(var i in toRemove)
            {
                otherSnakes.RemoveAt(i);
            }

            return snakeUpdateResult;
        }

        private bool updateOtherSnake(Snek otherSnake)
        {
            var headXY = otherSnake.Head();
            var randDirections = possibleDirections.OrderBy(x => randomNumberGenerator.Next()).ToArray();
            foreach (var direction in randDirections)
            {
                var possiblePosition = getNewHead(headXY, direction);
                
                if (board[possiblePosition.Item1][possiblePosition.Item2].In(ItemType.Wall, ItemType.Snake))
                {
                    continue;
                }
                else if (direction.Item1 == otherSnake.Direction.Item1 * -1 && direction.Item2 == otherSnake.Direction.Item2 * -1)
                {
                    continue;
                }

                otherSnake.Direction = direction;
                var itemSnakeAte = updateSnake(otherSnake);

                if (itemSnakeAte == ItemType.Food){ GenerateFood(false); }
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var boardrepr = new List<string>();
            foreach(var row in board) // remove public
            {
                var rowrepr = new List<string>();
                foreach(var cell in row)
                {
                    rowrepr.Add(((int) cell).ToString());
                }
                boardrepr.Add(string.Join(" ", rowrepr));
            }
            return string.Join("\n", boardrepr);
        }

        private ItemType updateSnake(Snek snakeToUpdate)
        {
            var oldTailXY = snakeToUpdate.Tail();
            var headXY = snakeToUpdate.Head();
            
            var newHeadXY = getNewHead(headXY, snakeToUpdate.Direction);
            snakeToUpdate.Move(newHeadXY);

            var itemSnakeAte = board[newHeadXY.Item1][newHeadXY.Item2];
            board[newHeadXY.Item1][newHeadXY.Item2] = ItemType.Snake;

            processItemSnakeAte(itemSnakeAte, snakeToUpdate, oldTailXY);
            return itemSnakeAte;
        }

        private void processItemSnakeAte(ItemType item, Snek snakeToUpdate, Coord oldTail)
        { 
            if (item == ItemType.Food)
            {
                snakeToUpdate.Body.Add(oldTail);
                genFood = true;
            }
            else
            {
                board[oldTail.Item1][oldTail.Item2] = ItemType.Empty;
            }

            if (item == ItemType.Vegan && snakeToUpdate.Body.Count > 2)
            {
                var tailXY = snakeToUpdate.Tail();
                board[tailXY.Item1][tailXY.Item2] = ItemType.Empty;
                snakeToUpdate.Body.Remove(tailXY);
            }
        }

        private Coord getNewHead(Coord headXY, Coord direction) => InBounds(new Coord(headXY.Item1 + direction.Item1, headXY.Item2 + direction.Item2));
      
        private List<Coord> FindAllOfType(ItemType itemtype)
        {
            var indices = new List<Coord>();

            return Enumerable.Range(0, Size)
                             .Select(
                               i => Enumerable.Range(0, board[i].Count)
                                              .Where(j => board[i][j] == itemtype)
                                              .Select(j => new Coord(i, j))
                                              .ToList()
                              ).SelectMany(x => x).ToList();
        }

        private Coord MaybeFindFirst(object query, Comparator comparator)
        {
            for (var row = 0; row < board.Count; row++)
            {
                for (var col = 0; col < board.Count; col++)
                {
                    if (comparator(query, board[row][col]))
                    {
                        return new Coord(row, col);
                    }
                }
            }
            return null;
        }
        private Coord FindFirstOfType(ItemType type_)
        {
            var result = MaybeFindFirst(type_, new Comparator((container, query) => (ItemType)container == query));

            if (result == null)
            {
                throw new ApplicationException($"Could not find {type_}");
            }

            return result;
        }

        private Coord MaybeFindFirstOfTypes(HashSet<ItemType> types)
        {
            return MaybeFindFirst(types, new Comparator((container, query) => ((HashSet<ItemType>)container).Contains(query) ));
        }

        public void GenerateFood(bool addSpecialFood)
        {
            var emptySpaces = FindAllOfType(ItemType.Empty);

            genFood = false;
            if (emptySpaces.Count <=0) { return; }

            var foodLocation = emptySpaces[randomNumberGenerator.Next(emptySpaces.Count)];
            emptySpaces.Remove(foodLocation);
            board[foodLocation.Item1][foodLocation.Item2] = ItemType.Food;

            if (emptySpaces.Count == 1 || !addSpecialFood) { return; }

            var specialFoodLocation = emptySpaces[randomNumberGenerator.Next(emptySpaces.Count)];
            specialFoodType = (ItemType) randomNumberGenerator.Next(
                (int) ItemType.Teleport,
                Enum.GetValues(typeof(ItemType)).Cast<int>().Max() + 1
                );
            board[specialFoodLocation.Item1][specialFoodLocation.Item2] = specialFoodType;
        }

    }
}
