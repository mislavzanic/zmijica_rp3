using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        
        private readonly int _Size;
        public int Size { get => _Size; }
        ItemType[] SpecialFoods = new ItemType[] {
            ItemType.Teleport,
            ItemType.Poison,
            ItemType.Vegan
        };


        private readonly List<Coord> wallsLocations = new List<Coord>();
        public List<Coord> Walls { get => wallsLocations; }

        private Coord foodLocation;
        private Coord specialFoodLocation;

        private ItemType specialFoodType;
        private readonly Dictionary<ItemType, Color> specialFoodColors = new Dictionary<ItemType, Color>()
        {
            { ItemType.Teleport, (Color)Properties.Settings.Default["skipColor"] },
            { ItemType.Poison, (Color)Properties.Settings.Default["poisonColor"]},
            { ItemType.Vegan, (Color)Properties.Settings.Default["shrinkColor"]}
        };
        
        public Board(Snek snek, string filepath)
        {
            board = Util.LoadMatrix(filepath);
            _Size = board.Count;
            MakeWalls();
            
            foreach (var element in snek.Body)
            {
                board[element.Item1][element.Item2] = ItemType.Snake;
            }
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
            for (int i = 0; i < wallsLocations.Count; i++)
            {
                Util.FillRect(myBuffer.Graphics, brush, wallsLocations[i], rW, rH, 0.1f);
            }
        }

        private Coord InBounds(Coord coord)
        {
            return new Coord((_Size + coord.Item1) % _Size, (_Size + coord.Item2) % _Size);
        }

        public void Update(Snek snek, Coord tail = null)
        {
            var headXY = InBounds(snek.Body.First());

            if (board[headXY.Item1][headXY.Item2].In(SpecialFoods)) {
                specialFoodLocation = null;
            } 
            
            // move head forward
            board[headXY.Item1][headXY.Item2] = ItemType.Snake;

            // remove tail
            if (tail != null)
            {
                var tailXY = InBounds(tail);
                board[tailXY.Item1][tailXY.Item2] = ItemType.Empty;
            }
        }

        public ItemType GetItem(Coord coord)
        {
            var newCoords = InBounds(coord);
            return board[newCoords.Item1][newCoords.Item2];
        }

        private List<Coord> EmptySpaces()
        {
            var indices = new List<Coord>();

            for( int i = 0; i < _Size; i++)
            {
                var row = this.board[i];
                indices.AddRange(
                    Enumerable.Range(0, row.Count)
                              .Where(j => row[j] == ItemType.Empty)
                              .Select(j => new Coord(i, j))
                );
            }

            return indices;
        }
        public void GenerateFood(bool addSpecialFood)
        {
            var randomNumberGenerator = new Random();
            var emptySpaces = EmptySpaces();

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

        private void MakeWalls()
        {
            for (int i = 0; i < _Size; ++i)
            {
                for (int j = 0; j < _Size; ++j)
                {
                    if (board[i][j] == ItemType.Wall) wallsLocations.Add(new Coord(i, j));
                }
            }
        }

    }
}
