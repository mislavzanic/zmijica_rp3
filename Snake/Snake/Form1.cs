using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        Settings settings;
        KeysPressed keysPressed;
        List<Tuple<int, int>> snek;
        Tuple<int, int> direction;
        Tuple<int, int> food;
        Random random;
        int[,] board;
        bool activeGame;
        int score;
        List<Tuple<int, int>> walls;

        public Form1()
        {
            InitializeComponent();
            settings = new Settings();
            random = new Random();
            keysPressed = new KeysPressed();
            //KeyPreview = true;
            //startGame(); // ne
        }

        private void startGame()
        {
            snek = new List<Tuple<int, int>>();
            snek.Add(new Tuple<int, int>((int)settings.boardsize/2, (int)settings.boardsize/2));
            direction = new Tuple<int, int>(1, 0);
            walls = new List<Tuple<int, int>>();
            makeWalls();
            updateBoard();
            generateFood();
            activeGame = true;
            render(canvasControl1.CreateGraphics());
            score = 0;
            scoreLabel.Text = "Score: " + score.ToString();
            canvasControl1.Focus();
            timer1.Interval = settings.timerInterval;
            timer1.Start();
        }

        private void gameOver()
        {
            activeGame = false;
            timer1.Stop();
            MessageBox.Show("rekt");
        }

        private void makeWalls()
        {
            if (settings.walls)
            {
                for (int i = 0; i < settings.boardsize; i++)
                {
                    walls.Add(new Tuple<int, int>(0, i));
                }
                for (int i = 1; i < settings.boardsize; i++)
                {
                    walls.Add(new Tuple<int, int>(i, 0));
                }
                for (int i = 1; i < settings.boardsize; i++)
                {
                    walls.Add(new Tuple<int, int>((int)settings.boardsize - 1, i));
                }
                for (int i = 1; i < settings.boardsize - 1; i++)
                {
                    walls.Add(new Tuple<int, int>(i, (int)settings.boardsize - 1));
                }
            }
            switch (settings.difficulty) // tu dodat za različite težine
            {
                default:
                    break;
            }
        }

        private void updateBoard()
        {
            board = new int[settings.boardsize, settings.boardsize];
            foreach (var element in snek)
            {
                board[element.Item1, element.Item2] = 1;
            }
            foreach (var element in walls)
            {
                board[element.Item1, element.Item2] = 3;
            }
        }

        private void generateFood()
        {
            uint itemnum = settings.boardsize * settings.boardsize - (uint)snek.Count - (uint)walls.Count;
            int foodLoc = random.Next((int)itemnum);
            int k = 0;
            for (int i = 0; i < settings.boardsize * settings.boardsize; i++)
            {
                if (board[i / settings.boardsize, i % settings.boardsize] == 0)
                {
                    if (k == foodLoc)
                    {
                        board[i / settings.boardsize, i % settings.boardsize] = 2;
                        food = new Tuple<int, int>(i / (int)settings.boardsize, i % (int)settings.boardsize);
                        break;
                    }
                    else
                    {
                        k++;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (keysPressed.Left && direction.Item1 == 0)
            {
                direction = new Tuple<int, int>(-1, 0);
            }
            else if (keysPressed.Right && direction.Item1 == 0)
            {
                direction = new Tuple<int, int>(1, 0);
            }
            else if (keysPressed.Up && direction.Item2 == 0)
            {
                direction = new Tuple<int, int>(0, -1);
            }
            else if (keysPressed.Down && direction.Item2 == 0)
            {
                direction = new Tuple<int, int>(0, 1);
            }
            if (keysPressed.Shift)
            {

            }
            else if (keysPressed.Ctrl)
            {

            }
            else
            {
                int newx = snek[0].Item1 + direction.Item1;
                int newy = snek[0].Item2 + direction.Item2;

                newx = (newx + (int)settings.boardsize) % (int)settings.boardsize;
                newy = (newy + (int)settings.boardsize) % (int)settings.boardsize;

                int eat = 0;
                for(int i = 0; i < keysPressed.Num; i++)
                {
                    if (board[(newx + direction.Item1 * i + (int)settings.boardsize) % (int)settings.boardsize, (newy + direction.Item2 * i + (int)settings.boardsize) % (int)settings.boardsize] == 2)
                    {
                        eat = 1;
                    }
                }
                int newsize = snek.Count + eat;
                for (int i = 0; i < keysPressed.Num - eat && i < newsize; i++)
                {
                    board[snek[snek.Count - 1].Item1, snek[snek.Count - 1].Item2] = 0;
                    snek.RemoveAt(snek.Count - 1);
                }
                for(int i = 0; i < keysPressed.Num; i++)
                {
                    Tuple<int, int> coords = new Tuple<int, int>((newx + direction.Item1 * i + (int)settings.boardsize) % (int)settings.boardsize,
                            (newy + direction.Item2 * i + (int)settings.boardsize) % (int)settings.boardsize);
                    if (board[coords.Item1, coords.Item2] == 0 || board[coords.Item1, coords.Item2] == 2)
                    {
                        if (i >= keysPressed.Num - newsize)
                        {
                            snek.Insert(0, coords);
                            board[coords.Item1, coords.Item2] = 1;
                        }
                    }
                    else // 1 ili 3
                    {
                        keysPressed.reset();
                        gameOver();
                        return;
                    }
                }
                
                if (eat > 0)
                {
                    score += 1;
                    scoreLabel.Text = "Score: " + score.ToString();
                    generateFood();
                }
            }

            keysPressed.reset();

            canvasControl1.Invalidate();
        }

        private void render(Graphics g)
        {
            //var g = canvasControl1.CreateGraphics();
            BufferedGraphicsContext currentContext;
            BufferedGraphics myBuffer;
            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(g, canvasControl1.DisplayRectangle);

            myBuffer.Graphics.Clear(settings.bgColor);
            myBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            if (!activeGame)
            {
                myBuffer.Render();
                myBuffer.Dispose();
                return;
            }
            float rW = canvasControl1.Width / (float)settings.boardsize;
            float rH = canvasControl1.Height / (float)settings.boardsize;
            //Pen pen = new Pen(settings.headColor);
            SolidBrush brush = new SolidBrush(settings.headColor);

            for(int i = 0; i < snek.Count; i++)
            {
                if(i == 1)
                {
                    brush.Color = settings.snekColor;
                }
                myBuffer.Graphics.FillRectangle(brush, rW * (snek[i].Item1 + 0.1f), rH * (snek[i].Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            }
            brush.Color = settings.foodColor;
            myBuffer.Graphics.FillRectangle(brush, rW * (food.Item1 + 0.1f), rH * (food.Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            brush.Color = settings.obstacleColor;
            for (int i = 0; i < walls.Count; i++)
            {
                myBuffer.Graphics.FillRectangle(brush, rW * (walls[i].Item1 + 0.1f), rH * (walls[i].Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            }
            myBuffer.Render();
            myBuffer.Dispose();
        }

        private void canvasControl1_Paint(object sender, PaintEventArgs e)
        {
            render(e.Graphics);
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == settings.left)
            {
                keysPressed.Left = true;
            }
            else if (e.KeyCode == settings.right)
            {
                keysPressed.Right = true;
            }
            else if (e.KeyCode == settings.up)
            {
                keysPressed.Up = true;
            }
            else if (e.KeyCode == settings.down)
            {
                keysPressed.Down = true;
            }
            else if (e.KeyCode == settings.newGame)
            {
                startGame();
            }
            else if (e.KeyCode == settings.shift)
            {
                keysPressed.Shift = true;
            }
            else if (e.KeyCode == settings.ctrl)
            {
                keysPressed.Ctrl = true;
            }
            else if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9)
            {
                keysPressed.Num = e.KeyCode - Keys.D0;
            }
            else if (e.KeyCode >= Keys.NumPad1 && e.KeyCode <= Keys.NumPad9)
            {
                keysPressed.Num = e.KeyCode - Keys.NumPad0;
            }
        }

        private void KeyReleased(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == settings.left)
            {
                keysPressed.Left = false;
            }
            else if (e.KeyCode == settings.right)
            {
                keysPressed.Right = false;
            }
            else if (e.KeyCode == settings.up)
            {
                keysPressed.Up = false;
            }
            else if (e.KeyCode == settings.down)
            {
                keysPressed.Down = false;
            
            }
            */
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == settings.up ||
                e.KeyCode == settings.down ||
                e.KeyCode == settings.left ||
                e.KeyCode == settings.right ||
                e.KeyCode == settings.options ||
                e.KeyCode == settings.newGame ||
                e.KeyCode == settings.shift ||
                e.KeyCode == settings.ctrl)
            {
                e.IsInputKey = true;
            }
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            startGame();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            // prikazati settings
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
