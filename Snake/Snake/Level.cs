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
        private Snek snek;
        private Board board;
        private Random random;
        private bool activeGame;
        private int score;

        public int Score { get => score; }
        public bool ActiveGame { get => activeGame; }
        
        public Level(string filepath)
        {
            snek = new Snek(10,10);  
            board = new Board(snek, filepath);
            random = new Random();
            generateFood();
            activeGame = true;
            score = 0;
        }

        public void render(BufferedGraphics myBuffer, int windowWidth, int windowHeight)
        {
            myBuffer.Graphics.Clear((Color)Properties.Settings.Default["bgColor"]);
            myBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            if (!activeGame)
            {
                myBuffer.Render();
                myBuffer.Dispose();
                return;
            }
            float rW = windowWidth / (float)board.BoardSize;
            float rH = windowHeight / (float)board.BoardSize;
            snek.render(myBuffer, rW, rH);
            board.render(myBuffer, rW, rH);
        }

        public void tick(int moveCount = 1)
        {
            if (snek.Direction.Item1 + snek.Direction.Item2 == 0) return;
            bool generateNew = false;
            while (moveCount > 0)
            {
                Coord tail = snek.move();
                int item = board.getItem(snek.Body.First());
                if (item == 1 || item == 3)
                {
                    activeGame = false;
                    return;
                }
                else if (item == 2)
                {
                    snek.Body.Add(tail);
                    board.update(snek);
                    generateNew = true;
                    score++;
                }
                else
                {
                    board.update(snek,tail);
                }
                moveCount--;
            }
            if (generateNew) generateFood();
        }

        public void newSnakeDirection(Coord newDirection)
        {
            snek.Direction = newDirection;
        }

        private void generateFood()
        {
            int itemnum = board.BoardSize * board.BoardSize - snek.Body.Count - board.Walls.Count;
            if(itemnum <= 0)
                return;
            board.generateFood(random.Next(itemnum));
        }
    }
}
