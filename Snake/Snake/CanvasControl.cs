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
    public partial class CanvasControl : UserControl
    {
        public CanvasControl()
        {
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);
            InitializeComponent();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                return true;
            }
            else if (keyData == Keys.Down)
            {
                return true;
            }
            else if (keyData == Keys.Left)
            {
                return true;
            }
            else if (keyData == Keys.Right)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

    }
}
