using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class KeysPressed
    {
        bool left;
        bool right;
        bool up;
        bool down;
        bool shift;
        bool ctrl;
        int num;

        public bool Left { get => left; set => left = value; }
        public bool Right { get => right; set => right = value; }
        public bool Up { get => up; set => up = value; }
        public bool Down { get => down; set => down = value; }
        public bool Shift { get => shift; set => shift = value; }
        public bool Ctrl { get => ctrl; set => ctrl = value; }
        public int Num { get => num; set => num = value; }

        public void reset()
        {
            left = right = up = down = false;
            shift = ctrl = false;
            num = 1;
        }
    }
}
