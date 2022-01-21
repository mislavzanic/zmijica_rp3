using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Snake
{
    using Coord = Tuple<int, int>;
    internal class Board
    {
        private int[,] board;
        private List<Coord> walls;
        private Coord food;
        private int boardSize;

        public List<Coord> Walls { get => walls; }
        public int BoardSize { get => boardSize; }
        public Board(Snek snek, String filepath)
        {
            board = Util.loadMatrix(filepath);
            boardSize = board.GetLength(0);
            walls = new List<Coord>();
            makeWalls();
            
            foreach (var element in snek.Body)
            {
                board[element.Item1, element.Item2] = 1;
            }
        }

        public void render(BufferedGraphics myBuffer, float rW, float rH)
        {
            SolidBrush brush = new SolidBrush((Color)Properties.Settings.Default["foodColor"]);
            myBuffer.Graphics.FillRectangle(brush, rW * (food.Item1 + 0.1f), rH * (food.Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            brush.Color = (Color)Properties.Settings.Default["obstacleColor"];
            for (int i = 0; i < walls.Count; i++)
            {
                myBuffer.Graphics.FillRectangle(brush, rW * (walls[i].Item1 + 0.1f), rH * (walls[i].Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            }
        }

        public void update(Snek snek)
        {
            board[snek.Body.First().Item1, snek.Body.First().Item2] = 1;
        }

        public void update(Snek snek, Coord tail)
        {
            update(snek);
            board[tail.Item1, tail.Item2] = 0;
        }

        public int getItem(Coord coord)
        {
            int x = coord.Item1 % boardSize, y = coord.Item2 % boardSize;
            return board[x,y];
        }

        public void generateFood(int location)
        {
            int k = 0;
            for (int i = 0; i < boardSize * boardSize; i++)
            {
                Coord coords = new Coord(i / boardSize, i % boardSize);
                if (board[coords.Item1, coords.Item2] == 0)
                {
                    if (k == location)
                    {
                        board[coords.Item1, coords.Item2] = 2;
                        food = new Coord(coords.Item1, coords.Item2);
                        return;
                    }
                    else
                    {
                        k++;
                    }
                }
            }
            throw new Exception();
        }

        private void makeWalls()
        {
            for (int i = 0; i < boardSize; ++i)
            {
                for (int j = 0; j < boardSize; ++j)
                {
                    if (board[i, j] == 3) walls.Add(new Coord(i, j));
                }
            }
        }

    }
}
