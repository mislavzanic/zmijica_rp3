namespace Snake
{
    partial class GameWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.canvasControl1 = new Snake.CanvasControl();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.scoreLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // canvasControl1
            // 
            this.canvasControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.canvasControl1.Location = new System.Drawing.Point(0, 0);
            this.canvasControl1.Name = "canvasControl1";
            this.canvasControl1.Size = new System.Drawing.Size(488, 450);
            this.canvasControl1.TabIndex = 0;
            this.canvasControl1.Load += new System.EventHandler(this.NewGame);
            this.canvasControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.paint);
            this.canvasControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyPressed);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(614, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 55);
            this.button1.TabIndex = 1;
            this.button1.Text = "New Game";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.NewGame);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(614, 92);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 51);
            this.button2.TabIndex = 2;
            this.button2.Text = "Settings";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.SettingsPreview);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.tick);
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Location = new System.Drawing.Point(625, 170);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(38, 13);
            this.scoreLabel.TabIndex = 4;
            this.scoreLabel.Text = "Score:";
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.canvasControl1);
            this.Name = "GameWindow";
            this.Text = "GameWindow";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyPressed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CanvasControl canvasControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label scoreLabel;
    }
}