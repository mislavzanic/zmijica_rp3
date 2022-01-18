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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            controlsButtonLeft.Text = "Left: " + ((Keys)Properties.Settings.Default["left"]).ToString();
            controlsButtonRight.Text = "Right: " + ((Keys)Properties.Settings.Default["right"]).ToString();
            controlsButtonUp.Text = "Up: " + ((Keys)Properties.Settings.Default["up"]).ToString();
            controlsButtonDown.Text = "Down: " + ((Keys)Properties.Settings.Default["down"]).ToString();
            controlsButtonShift.Text = "To Wall: " + ((Keys)Properties.Settings.Default["shift"]).ToString();
            controlsButtonCtrl.Text = "To Body: " + ((Keys)Properties.Settings.Default["ctrl"]).ToString();
            controlsButtonNewGame.Text = "New Game: " + ((Keys)Properties.Settings.Default["newGame"]).ToString();
            controlsButtonOptions.Text = "Options: " + ((Keys)Properties.Settings.Default["options"]).ToString();
        }
    }
}
