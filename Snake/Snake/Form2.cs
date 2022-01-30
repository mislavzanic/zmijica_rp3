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
        class KeyItem
        {
            public static IReadOnlyList<KeyItem> List { get; } = CreateList();

            private static IReadOnlyList<KeyItem> CreateList()
            {
                String[] keynames = Enum.GetNames(typeof(Keys));
                Array keyvalues = Enum.GetValues(typeof(Keys));

                return Enumerable
                    .Range(0, keynames.Length)
                    .Select(i => new KeyItem((int)keyvalues.GetValue(i), keynames[i]))
                    .ToList();
            }

            private KeyItem(int key, String name)
            {
                this.Code = key;
                this.Name = name ?? throw new ArgumentNullException(nameof(name));
            }

            public int Code { get; }
            public String Name { get; }
        }
        public Form2()
        {
            InitializeComponent();

            prepare(controlsComboBoxLeft);
            prepare(controlsComboBoxRight);
            prepare(controlsComboBoxUp);
            prepare(controlsComboBoxDown);
            prepare(controlsComboBoxShift);
            prepare(controlsComboBoxCtrl);
            prepare(controlsComboBoxNewGame);
            prepare(controlsComboBoxOptions);

            String[] keynames = Enum.GetNames(typeof(Keys));
            Array keyvalues = Enum.GetValues(typeof(Keys));

            controlsComboBoxLeft.SelectedIndex = Array.FindIndex(keynames, element => element == ((Keys)Properties.Settings.Default["left"]).ToString());
            controlsComboBoxRight.SelectedIndex = Array.FindIndex(keynames, element => element == ((Keys)Properties.Settings.Default["right"]).ToString());
            controlsComboBoxUp.SelectedIndex = Array.FindIndex(keynames, element => element == ((Keys)Properties.Settings.Default["up"]).ToString());
            controlsComboBoxDown.SelectedIndex = Array.FindIndex(keynames, element => element == ((Keys)Properties.Settings.Default["down"]).ToString());
            controlsComboBoxShift.SelectedIndex = Array.FindIndex(keynames, element => element == ((Keys)Properties.Settings.Default["shift"]).ToString());
            controlsComboBoxCtrl.SelectedIndex = Array.FindIndex(keynames, element => element == ((Keys)Properties.Settings.Default["ctrl"]).ToString());
            controlsComboBoxNewGame.SelectedIndex = Array.FindIndex(keynames, element => element == ((Keys)Properties.Settings.Default["newGame"]).ToString());
            controlsComboBoxOptions.SelectedIndex = Array.FindIndex(keynames, element => element == ((Keys)Properties.Settings.Default["options"]).ToString());
            
        }

        void prepare(ComboBox combobox)
        {
            combobox.BindingContext = new BindingContext();
            combobox.DropDownStyle = ComboBoxStyle.DropDownList;
            combobox.DisplayMember = nameof(KeyItem.Name);
            combobox.ValueMember = nameof(KeyItem.Code);
            combobox.DataSource = KeyItem.List;
        }

        private void left_index_change(object sender, EventArgs e)
        {
            if (controlsComboBoxLeft.SelectedIndex == 0) return;
            var keyvalues = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            Properties.Settings.Default["left"] = keyvalues[controlsComboBoxLeft.SelectedIndex];
        }

        private void up_index_change(object sender, EventArgs e)
        {
            if (controlsComboBoxLeft.SelectedIndex == 0) return;
            var keyvalues = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            Properties.Settings.Default["up"] = keyvalues[controlsComboBoxUp.SelectedIndex];
        }

        private void to_wall_index_change(object sender, EventArgs e)
        {
            if (controlsComboBoxLeft.SelectedIndex == 0) return;
            var keyvalues = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            Properties.Settings.Default["shift"] = keyvalues[controlsComboBoxShift.SelectedIndex];

        }

        private void right_index_change(object sender, EventArgs e)
        {
            if (controlsComboBoxLeft.SelectedIndex == 0) return;
            var keyvalues = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            Properties.Settings.Default["right"] = keyvalues[controlsComboBoxRight.SelectedIndex];

        }

        private void down_index_change(object sender, EventArgs e)
        {
            if (controlsComboBoxLeft.SelectedIndex == 0) return;
            var keyvalues = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            Properties.Settings.Default["down"] = keyvalues[controlsComboBoxDown.SelectedIndex];

        }

        private void to_body_index_change(object sender, EventArgs e)
        {
            if (controlsComboBoxLeft.SelectedIndex == 0) return;
            var keyvalues = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            Properties.Settings.Default["ctrl"] = keyvalues[controlsComboBoxCtrl.SelectedIndex];
        }

        private void close(object sender, EventArgs e)
        {
            this.Close();
        }

        private void new_game_index_change(object sender, EventArgs e)
        {
            if (controlsComboBoxLeft.SelectedIndex == 0) return;
            var keyvalues = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            Properties.Settings.Default["newGame"] = keyvalues[controlsComboBoxNewGame.SelectedIndex];
        }

        private void options_index_change(object sender, EventArgs e)
        {
            if (controlsComboBoxLeft.SelectedIndex == 0) return;
            var keyvalues = Enum.GetValues(typeof(Keys)).Cast<Keys>().ToList();
            Properties.Settings.Default["options"] = keyvalues[controlsComboBoxOptions.SelectedIndex];
        }

        private void settingsSaveAsDefaultButton1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
