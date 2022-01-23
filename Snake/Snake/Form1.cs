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
        Game game;

        public Form1()
        {
            InitializeComponent();
            keysPressed = new KeysPressed();
            //KeyPreview = true;
            //startGame(); // ne
        }

        private void startGame()
        {
            game = new Game();
            render(canvasControl1.CreateGraphics());
            scoreLabel.Text = "Score: " + game.Score.ToString();
            canvasControl1.Focus();
            timer1.Interval = (int)Properties.Settings.Default["timerInterval"];
            timer1.Start();
        }

        private void gameOver()
        {
            timer1.Stop();
            MessageBox.Show("rekt");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (keysPressed.Left)
            {
                game.handleInput(new Tuple<int,int>(-1,0));
            }
            else if (keysPressed.Right)
            {
                game.handleInput(new Tuple<int,int>(1,0));
            }
            else if (keysPressed.Up)
            {
                game.handleInput(new Tuple<int,int>(0,-1));
            }
            else if (keysPressed.Down)
            {
                game.handleInput(new Tuple<int,int>(0,1));
            }

            if (keysPressed.Shift)
            {
                //nesto
            }
            else if (keysPressed.Ctrl)
            {
                //nesto
            }
            else
            {
                game.tick(keysPressed.Num);
            }
            
            scoreLabel.Text = "Score: " + game.Score.ToString();
            keysPressed.reset();
            if (!game.ActiveGame)
            {
                gameOver();
                return;
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
            if (game != null) game.render(myBuffer, canvasControl1.Width, canvasControl1.Height);
            myBuffer.Render();
            myBuffer.Dispose();
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
            Form2 settingsForm = new Form2();
            settingsForm.ShowDialog();
        }

        private void canvasControl1_Paint(object sender, PaintEventArgs e)
        {
            render(e.Graphics);
        }
    }
}
