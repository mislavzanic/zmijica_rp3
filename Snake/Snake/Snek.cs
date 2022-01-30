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
        private readonly Dictionary<string, Color> colors = new Dictionary<string, Color>()
        {
            { "snake", (Color)Properties.Settings.Default["snekColor"] },
            { "head", (Color)Properties.Settings.Default["headColor"] },
            { "otherSnake", (Color)Properties.Settings.Default["otherSnekColor"]},
            { "otherHead", (Color)Properties.Settings.Default["otherHeadColor"]},
            { "outline", (Color)Properties.Settings.Default["snakeOutline"]}
        };

        public List<Coord> Body { get => bodyCoords; }
        public Coord Direction
        {
            get => direction;
            set {
                if (direction.Item1 * value.Item1 == 0 && direction.Item2 * value.Item2 == 0)
                {
                    direction = value;
                }
            }
        }

        public Snek(Coord startLocation)
        {
            bodyCoords.Add(startLocation);
        }

        public Coord Head() => bodyCoords.First();
        public Coord Tail() => bodyCoords.Last();
        public void ShrinkTail() => bodyCoords.Remove(bodyCoords.Last());
        public void Render(BufferedGraphics myBuffer, float rW, float rH, bool isOther = false)
        {
            var pen = new Pen(colors["outline"]);

            var brush = isOther ? new SolidBrush(colors["otherHead"]) : new SolidBrush(colors["head"]);

            Util.FillAndOutlineRect(myBuffer.Graphics, brush, pen, bodyCoords[0], rW, rH);

            brush.Color = isOther ? colors["otherSnake"] : colors["snake"];
            foreach (var bodyCoord in bodyCoords.Skip(1))
            {
                Util.FillAndOutlineRect(myBuffer.Graphics, brush, pen, bodyCoord, rW, rH);
            }
        }

        public Coord Move(Coord newHead)
        {
            Coord tail = bodyCoords.Last();
            bodyCoords.Remove(tail);
            bodyCoords.Insert(0, newHead);
            return tail;
        }

        public bool Moving()
        {
            return direction.Item1 != 0 || direction.Item2 != 0;
        }

    }
}
