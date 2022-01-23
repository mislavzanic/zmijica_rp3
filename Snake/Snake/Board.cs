using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    using Coord = Tuple<int, int>;
    internal class Board
    {
        private int[,] board;
        private List<Coord> walls;
        private Coord food;
        private Tuple<Coord, int> randItem;
        private List<string> items;
        private int boardSize;

        public List<Coord> Walls { get => walls; }
        public int BoardSize { get => boardSize; }
        public Board(Snek snek, string filepath)
        {
            board = Util.loadMatrix(filepath);
            boardSize = board.GetLength(0);
            walls = new List<Coord>();
            makeWalls();
            items = new List<string>();
            items.Add("skipColor");
            items.Add("poisonColor");
            items.Add("shrinkColor");
            
            foreach (var element in snek.Body)
            {
                board[element.Item1, element.Item2] = 1;
            }
        }

        public void render(BufferedGraphics myBuffer, float rW, float rH)
        {
            SolidBrush brush = new SolidBrush((Color)Properties.Settings.Default["foodColor"]);
            myBuffer.Graphics.FillRectangle(brush, rW * (food.Item1 + 0.1f), rH * (food.Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            if (randItem != null)
            {
                brush.Color = (Color)Properties.Settings.Default[items[randItem.Item2]];
                myBuffer.Graphics.FillRectangle(brush, rW * (randItem.Item1.Item1 + 0.1f), rH * (randItem.Item1.Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            }
            brush.Color = (Color)Properties.Settings.Default["obstacleColor"];
            for (int i = 0; i < walls.Count; i++)
            {
                myBuffer.Graphics.FillRectangle(brush, rW * (walls[i].Item1 + 0.1f), rH * (walls[i].Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            }
        }

        private Coord calcCoords(Coord coord)
        {
            return new Coord((boardSize + coord.Item1) % boardSize, (boardSize + coord.Item2) % boardSize);
        }

        public void update(Snek snek)
        {
            var newCoords = calcCoords(snek.Body.First());
            if (board[newCoords.Item1, newCoords.Item2] >= 4) randItem = null;
            board[newCoords.Item1, newCoords.Item2] = 1;
        }

        public void update(Snek snek, Coord tail)
        {
            update(snek);
            var newCoords = calcCoords(tail);
            board[newCoords.Item1, newCoords.Item2] = 0;
        }

        public int getItem(Coord coord)
        {
            var newCoords = calcCoords(coord);
            return board[newCoords.Item1,newCoords.Item2];
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

        public void generateFood(int location, int location2, int item)
        {
            var seen = new List<int>();
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
                        seen.Add(k);
                        if (seen.Count == 2) return;
                        k++;
                    }
                    else if (k == location2)
                    {
                        board[coords.Item1, coords.Item2] = 4 + item;
                        randItem = new Tuple<Coord, int>(new Coord(coords.Item1, coords.Item2), item);
                        seen.Add(k);
                        if (seen.Count == 2) return;
                        k++;
                    }
                    else
                    {
                        k++;
                    }
                }
            }
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
