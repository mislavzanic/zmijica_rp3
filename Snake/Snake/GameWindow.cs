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
    public partial class GameWindow : Form
    {

        KeysPressed keysPressed;
        Game game;
        bool shift = false;

        public GameWindow()
        {
            InitializeComponent();
            this.SetStyle(
               System.Windows.Forms.ControlStyles.UserPaint |
               System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
               System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
               true);
            keysPressed = new KeysPressed();
            renderInfoTab();
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

        private void tick(object sender, EventArgs e)
        {
            if (keysPressed.Shift)
            {
                shift = true;
            }
            else if (keysPressed.Ctrl)
            {
                game.pause();
            }
            else
            {
                if (keysPressed.Left && game.SnakeDirection.Item1 == 0)
                {
                    game.handleInput(new Tuple<int,int>(-1,0));
                }
                else if (keysPressed.Right && game.SnakeDirection.Item1 == 0)
                {
                    game.handleInput(new Tuple<int,int>(1,0));
                }
                else if (keysPressed.Up && game.SnakeDirection.Item2 == 0)
                {
                    game.handleInput(new Tuple<int,int>(0,-1));
                }
                else if (keysPressed.Down && game.SnakeDirection.Item2 == 0)
                {
                    game.handleInput(new Tuple<int,int>(0,1));
                }
           
                if (shift)
                {
                    game.tick(0, true);
                }
                else
                {
                    game.tick(keysPressed.Num);
                }
                shift = false;
                
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

        private void renderInfoTab()
        {
            pictureBox1.BringToFront();
            pictureBox2.BringToFront();
            pictureBox3.BringToFront();
            pictureBox4.BringToFront();
            pictureBox5.BringToFront();
            pictureBox1.CreateGraphics().Clear((Color)Properties.Settings.Default["headColor"]);
            pictureBox2.CreateGraphics().Clear((Color)Properties.Settings.Default["foodColor"]);
            pictureBox3.CreateGraphics().Clear((Color)Properties.Settings.Default["shrinkColor"]);
            pictureBox4.CreateGraphics().Clear((Color)Properties.Settings.Default["poisonColor"]);
            pictureBox5.CreateGraphics().Clear((Color)Properties.Settings.Default["skipColor"]);
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

        private void paint(object sender, PaintEventArgs e)
        {
            render(e.Graphics);
        }

        private void NewGame(object sender, EventArgs e)
        {
            startGame();
        }

        private void SettingsPreview(object sender, EventArgs e)
        {

            Form2 settingsForm = new Form2();
            settingsForm.ShowDialog();
            renderInfoTab();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString("Poison", myFont, Brushes.Black, new Point(2, 2));
            }
        }

        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
            renderInfoTab();
        }
    }
}
