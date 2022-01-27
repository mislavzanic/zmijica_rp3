﻿using System;
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
        private int scoreToPass;
        private bool skipLevel;

        public int Score { get => score; }
        public int ScoreToPass { get => scoreToPass; }
        public bool ActiveGame { get => activeGame; }
        public bool SkipLevel { get => skipLevel; }
        public Tuple<int, int> snekDirection { get => snek.Direction; }
        public Level(string filepath, int scoreToPass)
        {
            this.scoreToPass = scoreToPass;
            snek = new Snek(10,10);  
            board = new Board(snek, filepath);
            random = new Random();
            generateFood();
            activeGame = true;
            skipLevel = false;
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
                Coord tail = snek.move(board.BoardSize);
                int item = board.getItem(snek.Body.First());
                if (item == 1 || item == 3 || item == 4)
                {
                    skipLevel = item == 4;
                    activeGame = false;
                    return;
                }
                else if (item == 2)
                {
                    snek.Body.Add(tail);
                    board.update(snek);
                    generateNew = true;
                    score++;
                    if (score == scoreToPass)
                    {
                        activeGame = false;
                        return;
                    }
                }
                else if (item == 5)
                {
                    board.update(snek);
                    score--;
                }
                else if (item == 6)
                {
                    board.update(snek, tail);
                    board.update(snek, snek.Body.Last());
                    snek.Body.Remove(snek.Body.Last());
                    score++;
                    if (score == scoreToPass)
                    {
                        activeGame = false;
                        return;
                    }
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
            if (itemnum > 1 && score > 0 && score % 3 == 0)
            {
                var pos = random.Next(itemnum);
                var pos2 = 0;
                if (itemnum - pos > pos)
                {
                    pos2 = random.Next(itemnum - pos) + pos;
                }
                else
                {
                    pos2 = random.Next(pos);
                }
                if (pos2 == pos) pos2 = pos + 1;
                board.generateFood(pos, pos2, random.Next(3));
                return;
            }
            board.generateFood(random.Next(itemnum));
        }
    }
}
