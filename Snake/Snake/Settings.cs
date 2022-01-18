using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class Settings
    {
        public bool walls;
        public uint boardsize;
        public int timerInterval;
        public Keys left;
        public Keys right;
        public Keys up;
        public Keys down;
        public Keys newGame;
        public Keys options;
        public Color bgColor;
        public Color headColor;
        public Color snekColor;
        public Color foodColor;
        public Color obstacleColor;

        public Settings()
        {
            walls = true;
            boardsize = 40;
            timerInterval = 50;

            left = Keys.Left;
            right = Keys.Right;
            up = Keys.Up;
            down = Keys.Down;
            newGame = Keys.N;
            options = Keys.O;

            bgColor = Color.White;
            headColor = Color.Blue;
            snekColor = Color.Black;
            foodColor = Color.Green;
            obstacleColor = Color.Red;
        }
    }
}
