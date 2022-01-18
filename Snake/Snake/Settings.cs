using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public class Settings
    {
        public bool walls;
        public uint boardsize;
        public int timerInterval;
        public int difficulty;
        public Keys left;
        public Keys right;
        public Keys up;
        public Keys down;
        public Keys shift;
        public Keys ctrl;
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
            boardsize = 20;
            timerInterval = 50;
            difficulty = 0;

            left = Keys.Left;
            right = Keys.Right;
            up = Keys.Up;
            down = Keys.Down;
            shift = Keys.Shift;
            ctrl = Keys.Control;
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
