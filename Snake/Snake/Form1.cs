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
            random = new Random();
            keysPressed = new KeysPressed();
            //KeyPreview = true;
            //startGame(); // ne
        }

        private void startGame()
        {
            snek = new List<Tuple<int, int>>();
            snek.Add(new Tuple<int, int>((int)Properties.Settings.Default["boardsize"]/2, (int)Properties.Settings.Default["boardsize"]/2));
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
            timer1.Interval = (int)Properties.Settings.Default["timerInterval"];
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
            if ((bool)Properties.Settings.Default["walls"])
            {
                for (int i = 0; i < (int)Properties.Settings.Default["boardsize"]; i++)
                {
                    walls.Add(new Tuple<int, int>(0, i));
                }
                for (int i = 1; i < (int)Properties.Settings.Default["boardsize"]; i++)
                {
                    walls.Add(new Tuple<int, int>(i, 0));
                }
                for (int i = 1; i < (int)Properties.Settings.Default["boardsize"]; i++)
                {
                    walls.Add(new Tuple<int, int>((int)Properties.Settings.Default["boardsize"] - 1, i));
                }
                for (int i = 1; i < (int)Properties.Settings.Default["boardsize"] - 1; i++)
                {
                    walls.Add(new Tuple<int, int>(i, (int)Properties.Settings.Default["boardsize"] - 1));
                }
            }
            switch ((int)Properties.Settings.Default["difficulty"]) // tu dodat za različite težine
            {
                default:
                    break;
            }
        }

        private void updateBoard()
        {
            board = new int[(int)Properties.Settings.Default["boardsize"], (int)Properties.Settings.Default["boardsize"]];
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
            int itemnum = (int)Properties.Settings.Default["boardsize"] * (int)Properties.Settings.Default["boardsize"] - snek.Count - walls.Count;
            if(itemnum <= 0)
            {
                gameOver();
                return;
            }
            int foodLoc = random.Next((int)itemnum);
            int k = 0;
            for (int i = 0; i < (int)Properties.Settings.Default["boardsize"] * (int)Properties.Settings.Default["boardsize"]; i++)
            {
                Tuple<int, int> coords = new Tuple<int, int>(i / (int)Properties.Settings.Default["boardsize"], i % (int)Properties.Settings.Default["boardsize"]);
                if (board[coords.Item1, coords.Item2] == 0)
                {
                    if (k == foodLoc)
                    {
                        board[coords.Item1, coords.Item2] = 2;
                        food = new Tuple<int, int>(coords.Item1, coords.Item2);
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

                newx = (newx + (int)Properties.Settings.Default["boardsize"]) % (int)Properties.Settings.Default["boardsize"];
                newy = (newy + (int)Properties.Settings.Default["boardsize"]) % (int)Properties.Settings.Default["boardsize"];

                int eat = 0;
                for(int i = 0; i < keysPressed.Num; i++)
                {
                    if (board[(newx + direction.Item1 * i + (int)Properties.Settings.Default["boardsize"]) % (int)Properties.Settings.Default["boardsize"], (newy + direction.Item2 * i + (int)Properties.Settings.Default["boardsize"]) % (int)Properties.Settings.Default["boardsize"]] == 2)
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
                    Tuple<int, int> coords = new Tuple<int, int>((newx + direction.Item1 * i + (int)Properties.Settings.Default["boardsize"]) % (int)Properties.Settings.Default["boardsize"],
                            (newy + direction.Item2 * i + (int)Properties.Settings.Default["boardsize"]) % (int)Properties.Settings.Default["boardsize"]);
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

            myBuffer.Graphics.Clear((Color)Properties.Settings.Default["bgColor"]);
            myBuffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            if (!activeGame)
            {
                myBuffer.Render();
                myBuffer.Dispose();
                return;
            }
            float rW = canvasControl1.Width / (float)(int)Properties.Settings.Default["boardsize"];
            float rH = canvasControl1.Height / (float)(int)Properties.Settings.Default["boardsize"];
            //Pen pen = new Pen(settings.headColor);
            SolidBrush brush = new SolidBrush((Color)Properties.Settings.Default["headColor"]);

            for (int i = 0; i < snek.Count; i++)
            {
                if(i == 1)
                {
                    brush.Color = (Color)Properties.Settings.Default["snekColor"];
                }
                myBuffer.Graphics.FillRectangle(brush, rW * (snek[i].Item1 + 0.1f), rH * (snek[i].Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            }
            brush.Color = (Color)Properties.Settings.Default["foodColor"];
            myBuffer.Graphics.FillRectangle(brush, rW * (food.Item1 + 0.1f), rH * (food.Item2 + 0.1f), 0.8f * rW, 0.8f * rH);
            brush.Color = (Color)Properties.Settings.Default["obstacleColor"];
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
            if (e.KeyCode == (Keys)Properties.Settings.Default["left"])
            {
                keysPressed.Left = true;
            }
            else if (e.KeyCode == (Keys)Properties.Settings.Default["right"])
            {
                keysPressed.Right = true;
            }
            else if (e.KeyCode == (Keys)Properties.Settings.Default["up"])
            {
                keysPressed.Up = true;
            }
            else if (e.KeyCode == (Keys)Properties.Settings.Default["down"])
            {
                keysPressed.Down = true;
            }
            else if (e.KeyCode == (Keys)Properties.Settings.Default["newGame"])
            {
                startGame();
            }
            else if (e.KeyCode == (Keys)Properties.Settings.Default["shift"])
            {
                keysPressed.Shift = true;
            }
            else if (e.KeyCode == (Keys)Properties.Settings.Default["ctrl"])
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
            if(e.KeyCode == (Keys)Properties.Settings.Default["up"] ||
                e.KeyCode == (Keys)Properties.Settings.Default["down"] ||
                e.KeyCode == (Keys)Properties.Settings.Default["left"] ||
                e.KeyCode == (Keys)Properties.Settings.Default["right"] ||
                e.KeyCode == (Keys)Properties.Settings.Default["options"] ||
                e.KeyCode == (Keys)Properties.Settings.Default["newGame"] ||
                e.KeyCode == (Keys)Properties.Settings.Default["shift"] ||
                e.KeyCode == (Keys)Properties.Settings.Default["ctrl"])
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
