using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Snake
{

    using Coord = Tuple<int, int>;

    
    internal class Level
    {
        private readonly Snek snek;
        private readonly Board board;
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
            snek = new Snek(10,10);  
            board = new Board(snek, filepath);
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
            snek.Render(myBuffer, rW, rH);
            board.Render(myBuffer, rW, rH);
        }

        public void Reactivate()
        {
            activeGame = true;
        }


        public void Tick(int moveCount = 1, bool max = false)
        {
            // snake not moving
            if (snek.Direction.Item1 + snek.Direction.Item2 == 0) 
                return;

            bool generateNew = false;
            while (moveCount > 0 || max)
            {
                // move snake
                var tail = snek.Move(board.Size);
                var itemAtNewHeadPosition = board.GetItem(snek.Head());

                switch (itemAtNewHeadPosition){
                    case ItemType.Food:
                        snek.Body.Add(tail);
                        board.Update(snek);
                        generateNew = true;

                        if (++score == scoreToPass)
                        {
                            activeGame = false;
                            return;
                        }
                        break;
                    case ItemType.Poison:
                        board.Update(snek);
                        score--;
                        break;
                    case ItemType.Vegan:
                        board.Update(snek, tail);
                        board.Update(snek, snek.Body.Last());
                        snek.Body.Remove(snek.Body.Last());
                        score++;
                        if (score == scoreToPass)
                        {
                            activeGame = false;
                            return;
                        }
                        break;
                    case ItemType.Empty:
                        board.Update(snek, tail);
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

        public void UpdateSnakeDirection(Coord newDirection)
        {
            snek.Direction = newDirection;
        }

        private void GenerateFood()
        {
            board.GenerateFood(score > 0 && score % 3 == 0);
        }
    }
}
