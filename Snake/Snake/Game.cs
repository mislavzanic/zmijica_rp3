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
        private bool isPaused;
        private int availablePauses;

        public int Score { get => (levelStack.Count > 0) ? score + levelStack.First().Score : score; }
        public bool ActiveGame { get => activeGame; }
        public Coord SnakeDirection { get => levelStack.First().SnakeDirection; }
        
        public Game()
        {
            levelStack = new Stack<Level>();
            levelStack.Push(new Level("..\\..\\assets\\level3.txt", 90, 15));
            levelStack.Push(new Level("..\\..\\assets\\level3.txt", 75, 8));
            levelStack.Push(new Level("..\\..\\assets\\level3.txt", 50, 3));
            levelStack.Push(new Level("..\\..\\assets\\level3.txt", 40));
            levelStack.Push(new Level("..\\..\\assets\\level2.txt", 30, 5));
            levelStack.Push(new Level("..\\..\\assets\\level2.txt", 25));
            levelStack.Push(new Level("..\\..\\assets\\level1.txt", 10));
            activeGame = true;
            score = 0;
            isPaused = false;
            availablePauses = 3;
        }

        public void render(BufferedGraphics myBuffer, int windowWidth, int windowHeight)
        {
            if (levelStack.Count() > 0)
            {
                levelStack.First().Render(myBuffer, windowWidth, windowHeight);
            }
        }

        public void tick(int moveCount = 1, bool max = false)
        {
            if (isPaused)
            {
                return;
            }
            var currentLevel = levelStack.First();
            currentLevel.Tick(moveCount, max);

            if (currentLevel.ActiveGame) {return;}

            score += currentLevel.Score;
            
            if (currentLevel.Defeat)
            {
                levelStack.Clear();
                activeGame = false;
                return;
            }

            if (levelStack.Count > 1) { 
                levelStack.Pop(); 
            }
            else
            {
                currentLevel.Reactivate();
            }
        }

        public void pause()
        {
            if (isPaused)
            {
                isPaused = false;
            }
            else
            {
                if (availablePauses > 0)
                {
                    --availablePauses;
                    isPaused = true;
                }
            }
        }
        public void handleInput(Coord newDirection)
        {
            levelStack.First().UpdateSnakeDirection(newDirection);
        }
    }
}
