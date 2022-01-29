using System;
using System.Drawing;

namespace Snake
{

    using Coord = Tuple<int, int>;

    
    internal class Level
    {
        private readonly Board board;
        private readonly Snek snake;
        private bool activeGame = true;
        private int score = 0;
        private readonly int scoreToPass;
        private bool skipLevel = false;

        public int Score { get => score; }
        public int ScoreToPass { get => scoreToPass; }
        public bool ActiveGame { get => activeGame; }
        public bool SkipLevel { get => skipLevel; }
        public Level(string filepath, int scoreToPass)
        {
            this.scoreToPass = scoreToPass;
            board = new Board(filepath, out snake);
            GenerateFood();
        }

        public void Render(BufferedGraphics myBuffer, int windowWidth, int windowHeight)
        {
            myBuffer.Graphics.Clear((Color)Properties.Settings.Default["bgColor"]);
            myBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            if (!activeGame)
            {
                myBuffer.Render();
                myBuffer.Dispose();
                return;
            }
            float rW = windowWidth / (float)board.Size;
            float rH = windowHeight / (float)board.Size;
            board.Render(myBuffer, rW, rH);
        }

        public void Reactivate()
        {
            activeGame = true;
            skipLevel = false;
        }


        public void Tick(int moveCount = 1, bool max = false)
        {
            if (snake.Direction.Item1 == 0 && snake.Direction.Item2 == 0) { return; }

            bool generateNew = false;
            
            while (moveCount > 0 || max)
            {
                switch (board.Update())
                {
                    case ItemType.Food:
                        generateNew = true;
                        newPoint();
                        break;
                    case ItemType.Poison:
                        score--;
                        break;
                    case ItemType.Vegan:
                        newPoint();
                        break;
                    case ItemType.Empty:
                        break;
                    case ItemType.Teleport:
                        skipLevel = true;
                        activeGame = false;
                        return;
                    default:
                        activeGame = false;
                        return;
                }
                moveCount--;
            }
            if (generateNew) 
                GenerateFood();
        }

        private void newPoint()
        {
            if (++score == scoreToPass)
            {
                activeGame = false;
            }
        }

        public void UpdateSnakeDirection(Coord newDirection)
        {
            snake.Direction = newDirection;
        }

        private void GenerateFood()
        {
            board.GenerateFood(score > 0 && score % 3 == 0);
        }
    }
}
