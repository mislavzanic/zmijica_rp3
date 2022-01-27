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
        private List<Coord> body;
        private Coord direction;

        public List<Coord> Body { get => body; }
        public Coord Direction
        {
            get => direction;
            set => direction = value;
        }

        public Snek(int startX, int startY)
        {
            body = new List<Coord>();
            body.Add(new Coord(startX, startY));
            direction = new Coord(0, 0);
        }

        public void render(BufferedGraphics myBuffer, float rW, float rH)
        {
            SolidBrush brush = new SolidBrush((Color)Properties.Settings.Default["headColor"]);
            Util.FillRect(myBuffer.Graphics, brush, body[0], rW, rH, 0.2f);

            brush.Color = (Color)Properties.Settings.Default["snekColor"];
            for (int i = 1; i < body.Count; i++)
            {
                Util.FillRect(myBuffer.Graphics, brush, body[i], rW, rH, 0.2f);
            }
        }

        public Coord move(int boardSize)
        {
            Coord newHead = new Coord((boardSize + body.First().Item1 + direction.Item1) % boardSize, (boardSize + body.First().Item2 + direction.Item2) % boardSize);
            Coord tail = body.Last();
            body.Remove(tail);
            body.Insert(0, newHead);
            return tail;
        }

    }
}
