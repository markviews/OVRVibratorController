using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VibratorController {

    partial class Form1 : Form {

        internal static Form1 me;
        internal static OVRVibratorController vc = new OVRVibratorController();
        internal static List<Toy> toys = new List<Toy>();
        private int collums;

        internal Form1() {
            me = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            vc.SetupClient();
            vc.SetupOVR();
        }

        private void add_Click(object sender, EventArgs e) {
            string code = addToyText.Text;

            addToy(code);
            return;

            if (code.Length != 4) {
                MessageBox.Show("Code should be 4 characters long");
                return;
            }
            addToyText.Text = "";
        }

        private void setHold_Click(object sender, EventArgs e) {
            //-1 = none, -2 = searching

            if (vc.holdController == -2) {
                //button was clicked twice in a row, set to none
                vc.holdController = -1;
                setHoldText.Text = "None";
                return;
            }

            if (vc.LockController == -2) {
                //lockButton was searching.. stop that
                vc.LockController = -1;
                setLockText.Text = "None";
            }

            vc.holdController = -2;
            setHoldText.Text = "Press Button";
        }

        private void setLock_Click(object sender, EventArgs e) {
            //-1 = none, -2 = searching

            if (vc.LockController == -2) {
                //button was clicked twice in a row, set to none
                vc.LockController = -1;
                setLockText.Text = "None";
                return;
            }

            if (vc.holdController == -2) {
                //lockButton was searching.. stop that
                vc.holdController = -1;
                setHoldText.Text = "None";
            }

            vc.LockController = -2;
            setLockText.Text = "Press Button";
        }

        internal static void setHoldButtonText(string text) {
            me.setHoldText.Invoke((Action)delegate () { me.setHoldText.Text = text; });
        }

        internal static void setLockButtonText(string text) {
            me.setLockText.Invoke((Action)delegate () { me.setLockText.Text = text; });
        }

        internal static void setServerStatus(bool connected) {
            if (me == null) return;

            if (connected) {
                me.serverStatus.ForeColor = Color.FromArgb(128, 255, 128);
                me.serverStatus.Text = "Connected to server";
            } else {
                me.serverStatus.ForeColor = Color.FromArgb(255, 128, 128);
                me.serverStatus.Text = "Disconnected from server";
            }
        }

        internal static void setOVRStatus(bool connected) {
            if (me == null) return;

            if (connected) {
                me.ovrStatus.ForeColor = Color.FromArgb(128, 255, 128);
                me.ovrStatus.Text = "Connected to OVR";
            } else {
                me.ovrStatus.ForeColor = Color.FromArgb(255, 128, 128);
                me.ovrStatus.Text = "OVR not connected";
            }
        }

        internal static void setControllerStatus(string status) {
            if (me == null) return;

            if (status == "good") {
                me.controllerStatus.ForeColor = Color.FromArgb(128, 255, 128);
                me.controllerStatus.Text = "Found both controllers";
            } else {
                me.controllerStatus.ForeColor = Color.Khaki;
                me.controllerStatus.Text = status;
            }
        }

        internal static int getSliderMax() {
            return me.slider1.Maximum;
        }

        private void addToy(string name) {

            if (collums == 7 || collums == 6 && (name == "Edge" || name == "Max")) {
                MessageBox.Show("Max toys added. You can open a second instance of the program if you'd like");
                return;
            }

            Toy toy = new Toy(name, "temp_id");

            //copy name box
            Label box = new Label();
            toy.nameText = box;
            box.Location = new Point(9, 388 - (51 * (collums)));
            box.Visible = true;
            box.BackColor = name1.BackColor;
            box.ForeColor = name1.ForeColor;
            box.Size = name1.Size;
            box.Text = name;
            box.TextAlign = name1.TextAlign;
            box.Font = name1.Font;
            Controls.Add(box);

            //copy dropdown
            ComboBox dropdown = new ComboBox();
            toy.dropdown = dropdown;
            dropdown.Location = new Point(9, 414 - (51 * (collums)));
            dropdown.Visible = true;
            dropdown.BackColor = dropdown1.BackColor;
            dropdown.ForeColor = dropdown1.ForeColor;
            dropdown.Size = name1.Size;
            dropdown.Font = dropdown1.Font;
            dropdown.DropDownStyle = dropdown1.DropDownStyle;
            dropdown.SelectedIndexChanged += (object sender, EventArgs e) => toy.moveDropdown(dropdown.SelectedIndex);
            foreach (string item in dropdown1.Items) dropdown.Items.Add(item);
            dropdown.SelectedIndex = 0;
            Controls.Add(dropdown);

            //copy slider
            TrackBar slider = new TrackBar();
            toy.slider = slider;
            slider.Location = new Point(110, 388 - (51 * (collums)));
            slider.Visible = true;
            slider.Size = slider1.Size;
            slider.TickStyle = slider1.TickStyle;
            slider.Maximum = slider1.Maximum;
            slider.Minimum = slider1.Minimum;
            slider.LargeChange = slider1.LargeChange;
            slider.SmallChange = slider1.SmallChange;
            slider.MouseDown += (object sender, MouseEventArgs e) => fixClick(e.X, slider, toy);
            slider.Scroll += (object sender, EventArgs e) => toy.moveSliderEvent(slider.Value);
            Controls.Add(slider);

            if (name == "Edge" || name == "Max") {

                //copy slider
                TrackBar slider2 = new TrackBar();
                toy.slider2 = slider2;
                slider2.Location = new Point(110, 388 - (51 * (collums + 1)));
                slider2.Visible = true;
                slider2.Size = slider1.Size;
                slider2.TickStyle = slider1.TickStyle;
                slider2.Maximum = slider1.Maximum;
                slider2.Minimum = slider1.Minimum;
                slider2.LargeChange = slider1.LargeChange;
                slider2.SmallChange = slider1.SmallChange;
                slider2.MouseDown += (object sender, MouseEventArgs e) => fixClick(e.X, slider2, toy, 2);
                slider2.Scroll += (object sender, EventArgs e) => toy.moveSliderEvent(slider2.Value, 2);
                Controls.Add(slider2);
                collums++;
            }

            toys.Add(toy);
            collums++;
        }

        private void fixClick(double x, TrackBar slider, Toy toy, int vibeNum = 1) {
            int val = Convert.ToInt32((x / (double)slider.Width) * (slider.Maximum - slider.Minimum));
            if (Math.Abs(slider.Value - val) >= 1) {
                slider.Value = val;
                if (toy != null) toy.moveSliderEvent(val, vibeNum);
            }
        }

        private void dropdown1_SelectedIndexChanged(object sender, EventArgs e) {

        }
    }

    public class Toy {
        internal string name;
        internal string id;
        internal Hand hand;

        //keep track of these so we can delete them if toy disconnects
        internal Label nameText;
        internal ComboBox dropdown;
        internal TrackBar slider;
        internal TrackBar slider2;

        internal Toy(String name, string id) {
            this.name = name;
            this.id = id;
        }

        //move slider with trigger input
        internal void moveSlider(int newValue, int vibeNum = 1) {
            moveSliderEvent(newValue, vibeNum);

            //actually move the slider
            if (vibeNum == 1) slider.Invoke((Action)delegate () { slider.Value = newValue; });
            else slider2.Invoke((Action)delegate () { slider2.Value = newValue; });
        }

        //slider moved event
        internal void moveSliderEvent(int newValue, int vibeNum = 1) {
            Console.WriteLine("moveSlider " + name + " " + newValue + " " + vibeNum);

            //send stuff the server
        }

        internal void moveDropdown(int newValue) {
            switch(newValue) {
                case 0: hand = Hand.SliderOnly; break;
                case 1: hand = Hand.Left; break;
                case 2: hand = Hand.Right; break;
                case 3: hand = Hand.Both; break;
            }
        }

        internal enum Hand {
            Left,
            Right,
            Both,
            SliderOnly
        }

    }
}
