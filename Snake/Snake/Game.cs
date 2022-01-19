using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Snake
{
    using Coord = Tuple<int, int>;
    internal class Game
    {
        private Snek snek;
        private bool activeGame;
        private int score;
        private Level currLevel;

        public event EventHandler<int> scoreChange;
        public int Score { get => score; }
        public bool ActiveGame { get => activeGame; }
        
        public Game()
        {
            currLevel = new Level("..\\..\\assets\\level1.txt");
            activeGame = true;
            score = 0;
        }

        public void render(BufferedGraphics myBuffer, int windowWidth, int windowHeight)
        {
            currLevel.render(myBuffer, windowWidth, windowHeight);
        }

        public void tick(int moveCount = 1)
        {
            currLevel.tick(moveCount);
            if (!currLevel.ActiveGame)
            {
                activeGame = false;
                return;
            }
            if (currLevel.Score > 10)
            {
                score = currLevel.Score;
                if (scoreChange != null) scoreChange(this, score);
                currLevel = new Level("..\\..\\assets\\level2.txt");
            }

            if (currLevel.Score > 11)
            {
                activeGame = false;
                return;
            }
        }

        public void handleInput(Coord newDirection)
        {
            currLevel.newSnakeDirection(newDirection);
        }
    }
}
