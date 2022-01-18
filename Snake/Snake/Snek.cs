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
            //{
                //direction = value;
                //move();
            //}
        }

        public Snek()
        {
            body = new List<Coord>();
            body.Add(new Coord((int)Properties.Settings.Default["boardsize"]/2, (int)Properties.Settings.Default["boardsize"]/2));
            direction = new Coord(1, 0);
        }

        public void render(BufferedGraphics myBuffer, float rW, float rH)
        {
            SolidBrush brush = new SolidBrush((Color)Properties.Settings.Default["headColor"]);

            for (int i = 0; i < body.Count; i++)
            {
                if(i == 1)
                {
                    brush.Color = (Color)Properties.Settings.Default["snekColor"];
                }
                myBuffer.Graphics.FillRectangle(brush, rW * (body[i].Item1 + 0.1f), rH * (body[i].Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            }
        }

        public Coord move()
        {
            Coord newHead = new Coord(body.First().Item1 + direction.Item1, body.First().Item2 + direction.Item2);
            Coord tail = body.Last();
            body.Remove(tail);
            body.Insert(0, newHead);
            return tail;
        }

    }
}
