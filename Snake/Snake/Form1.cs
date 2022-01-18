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
            snek.Add(new Tuple<int, int>(19, 19));
            direction = new Tuple<int, int>(1, 0);
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

        private void updateBoard()
        {
            board = new int[settings.boardsize, settings.boardsize];
            foreach (var element in snek)
            {
                board[element.Item1, element.Item2] = 1;
            }
        }

        private void generateFood()
        {
            uint itemnum = settings.boardsize * settings.boardsize - (uint)snek.Count;
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
            keysPressed.reset();
            int newx = snek[0].Item1 + direction.Item1;
            int newy = snek[0].Item2 + direction.Item2;
            if(settings.walls && (newx < 0 || newx >= settings.boardsize || newy < 0 || newy >= settings.boardsize))
            {
                gameOver();
                return;
            }
            else
            {
                newx = newx % (int)settings.boardsize;
                newy = newy % (int)settings.boardsize;
            }
            if (board[newx, newy] == 2)
            {
                snek.Insert(0, new Tuple<int, int>(newx, newy));
                board[newx, newy] = 1;
                score += 1;
                scoreLabel.Text = "Score: " + score.ToString();
                generateFood();
            }
            else
            {
                board[snek[snek.Count - 1].Item1, snek[snek.Count - 1].Item2] = 0;
                snek.RemoveAt(snek.Count - 1);
                if(board[newx, newy] == 0)
                {
                    snek.Insert(0, new Tuple<int, int>(newx, newy));
                    board[newx, newy] = 1;
                }
                else // 1
                {
                    gameOver();
                    return;
                }
            }

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
                e.KeyCode == settings.newGame)
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
