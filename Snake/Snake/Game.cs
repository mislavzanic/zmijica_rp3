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
        private Stack<Level> levelStack;
        private bool activeGame;
        private int score;

        public event EventHandler<int> scoreChange;
        public int Score { get => score; }
        public bool ActiveGame { get => activeGame; }
        
        public Game()
        {
            levelStack = new Stack<Level>();
            levelStack.Push(new Level("..\\..\\assets\\level2.txt", 15));
            levelStack.Push(new Level("..\\..\\assets\\level1.txt", 10));
            activeGame = true;
            score = 0;
        }

        public void render(BufferedGraphics myBuffer, int windowWidth, int windowHeight)
        {
            levelStack.First().render(myBuffer, windowWidth, windowHeight);
        }

        public void tick(int moveCount = 1)
        {
            levelStack.First().tick(moveCount);
            if (!levelStack.First().ActiveGame)
            {
                score += levelStack.First().Score;
                if (levelStack.First().Score < levelStack.First().ScoreToPass)
                {
                    levelStack.Clear();
                    activeGame = false;
                    return;
                }
                levelStack.Pop();
                activeGame = levelStack.Count > 0;
                return;
            }
        }

        public void handleInput(Coord newDirection)
        {
            levelStack.First().newSnakeDirection(newDirection);
        }
    }
}
