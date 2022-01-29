using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    using Coord = Tuple<int, int>;
    internal class Snek
    {
        private readonly List<Coord> bodyCoords = new List<Coord>();
        private Coord direction = new Coord(0, 0);
        
        public List<Coord> Body { get => bodyCoords; }
        public Coord Direction
        {
            get => direction;
            set => direction = value;
        }

        public Snek(int startX, int startY)
        {
            bodyCoords.Add(new Coord(startX, startY));
        }

        public Coord Head() => bodyCoords.First();
        public Coord Tail() => bodyCoords.Last();
        public void ShrinkTail() => bodyCoords.Remove(bodyCoords.Last());
        public void Render(BufferedGraphics myBuffer, float rW, float rH)
        {
            var brush = new SolidBrush((Color)Properties.Settings.Default["headColor"]);

            Util.FillRect(myBuffer.Graphics, brush, bodyCoords[0], rW, rH, 0.2f);

            brush.Color = (Color)Properties.Settings.Default["snekColor"];
            foreach(var bodyCoord in bodyCoords.Skip(1))
            {
                Util.FillRect(myBuffer.Graphics, brush, bodyCoord, rW, rH, 0.2f);
            }
        }

        public Coord Move(int boardSize)
        {
            Coord newHead = new Coord((boardSize + bodyCoords.First().Item1 + direction.Item1) % boardSize, (boardSize + bodyCoords.First().Item2 + direction.Item2) % boardSize);
            Coord tail = bodyCoords.Last();
            bodyCoords.Remove(tail);
            bodyCoords.Insert(0, newHead);
            return tail;
        }

    }
}
