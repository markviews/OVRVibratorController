using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VibratorController {

    partial class Form1 : Form {

        internal static OVRVibratorController vc = new OVRVibratorController();
        internal List<Toy> toys = new List<Toy>();
        internal int rows;

        internal Form1() {
            vc.form = this;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            vc.Setup();
        }

        private void add_Click(object sender, EventArgs e) {
            string code = addToyText.Text.Trim();

            if (code.Length != 4) {
                MessageBox.Show("Code should be 4 characters long");
                return;
            }

            addToyText.Text = "";
            vc.Send("join " + code);
        }

        private void setHold_Click(object sender, EventArgs e) {
            //-1 = none, -2 = searching

            if (vc.holdController == -2) {
                //button was clicked twice in a row, set to none
                vc.holdController = -1;
                setHoldText.Text = "None";
                vc.holding = true;
                vc.settings["hold"] = "None";
                vc.SaveSettings();
                return;
            }

            if (vc.lockController == -2) {
                //lockButton was searching.. stop that
                vc.lockController = -1;
                setLockText.Text = "None";
                vc.lockSpeed = false;
                vc.settings["lock"] = "None";
                vc.SaveSettings();
            }

            vc.holdController = -2;
            setHoldText.Text = "Press Button";
        }

        private void setLock_Click(object sender, EventArgs e) {
            //-1 = none, -2 = searching

            if (vc.lockController == -2) {
                //button was clicked twice in a row, set to none
                vc.lockController = -1;
                setLockText.Text = "None";
                vc.lockSpeed = false;
                vc.settings["lock"] = "None";
                vc.SaveSettings();
                return;
            }

            if (vc.holdController == -2) {
                //holdButton was searching.. stop that
                vc.holdController = -1;
                setHoldText.Text = "None";
                vc.holding = true;
                vc.settings["hold"] = "None";
                vc.SaveSettings();
            }

            vc.lockController = -2;
            setLockText.Text = "Press Button";
        }

        internal void setHoldButtonText(string text) {
            setHoldText.Invoke((Action)delegate () { setHoldText.Text = text; });
        }

        internal void setLockButtonText(string text) {
           setLockText.Invoke((Action)delegate () { setLockText.Text = text; });
        }

        internal void setServerStatus(bool connected) {
            if (connected) {
                serverStatus.ForeColor = Color.FromArgb(128, 255, 128);
                serverStatus.Text = "Connected to server";
            } else {
                serverStatus.ForeColor = Color.FromArgb(255, 128, 128);
                serverStatus.Text = "Disconnected from server";
            }
        }

        internal void setOVRStatus(bool connected) {
            if (connected) {
                ovrStatus.ForeColor = Color.FromArgb(128, 255, 128);
                ovrStatus.Text = "Connected to OVR";
            } else {
                ovrStatus.ForeColor = Color.FromArgb(255, 128, 128);
                ovrStatus.Text = "OVR not connected";
            }
        }

        internal void setControllerStatus(string status) {
            if (status == "good") {
                controllerStatus.ForeColor = Color.FromArgb(128, 255, 128);
                controllerStatus.Text = "Found both controllers";
            } else {
                controllerStatus.ForeColor = Color.Khaki;
                controllerStatus.Text = status;
            }
        }

        internal int getSliderMax() {
            return slider1.Maximum;
        }

        internal void addToy(string name, string id) {
            Invoke((Action)delegate () {

                if (rows == 7 || rows == 6 && (name == "Edge" || name == "Max")) {
                    MessageBox.Show("Max toys added. You can open a second instance of the program if you'd like");
                    return;
                }

                Toy toy = new Toy(name, id);

                //copy name box
                Label nameText = new Label();
                toy.nameText = nameText;
                nameText.Location = new Point(9, 388 - (51 * (rows)));
                nameText.Visible = true;
                nameText.BackColor = name1.BackColor;
                nameText.ForeColor = name1.ForeColor;
                nameText.Size = name1.Size;
                nameText.Text = name;
                nameText.TextAlign = name1.TextAlign;
                nameText.Font = name1.Font;
                Controls.Add(nameText);

                //copy dropdown
                ComboBox dropdown = new ComboBox();
                toy.dropdown = dropdown;
                dropdown.Location = new Point(9, 414 - (51 * (rows)));
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
                slider.Location = new Point(110, 388 - (51 * (rows)));
                slider.Visible = true;
                slider.Size = slider1.Size;
                slider.TickStyle = slider1.TickStyle;
                slider.Maximum = slider1.Maximum;
                slider.Minimum = slider1.Minimum;
                slider.LargeChange = slider1.LargeChange;
                slider.SmallChange = slider1.SmallChange;
                slider.MouseDown += (object sender, MouseEventArgs e) => fixClick(e.X, slider, toy);
                if (name == "Edge")  slider.Scroll += (object sender, EventArgs e) => toy.moveSliderEvent(slider.Value, 1);
                else slider.Scroll += (object sender, EventArgs e) => toy.moveSliderEvent(slider.Value);
                Controls.Add(slider);

                if (name == "Edge" || name == "Max") {
                    int max = slider1.Maximum;
                    if (name == "Max") max = 3;

                    //copy slider
                    TrackBar slider2 = new TrackBar();
                    toy.slider2 = slider2;
                    slider2.Location = new Point(110, 388 - (51 * (rows + 1)));
                    slider2.Visible = true;
                    slider2.Size = slider1.Size;
                    slider2.TickStyle = slider1.TickStyle;
                    slider2.Maximum = max;
                    slider2.Minimum = slider1.Minimum;
                    slider2.LargeChange = slider1.LargeChange;
                    slider2.SmallChange = slider1.SmallChange;
                    slider2.MouseDown += (object sender, MouseEventArgs e) => fixClick(e.X, slider2, toy, 2);
                    slider2.Scroll += (object sender, EventArgs e) => toy.moveSliderEvent(slider2.Value, 2);
                    Controls.Add(slider2);
                    rows++;
                }

                if (name == "Nora") {

                    Button rotate = new Button();
                    toy.rotateButton = rotate;
                    rotate.Location = new Point(110, 388 - (51 * (rows + 1)));
                    rotate.Visible = true;
                    rotate.Text = rotateButton.Text;
                    rotate.BackColor = rotateButton.BackColor;
                    rotate.ForeColor = rotateButton.ForeColor;
                    rotate.Size = rotateButton.Size;
                    rotate.Font = rotateButton.Font;
                    rotate.Click += (object sender, EventArgs e) => toy.rotate();
                    Controls.Add(rotate);
                    rows++;
                }

                toys.Add(toy);
                rows++;
            });
        }

        private void fixClick(double x, TrackBar slider, Toy toy, int vibeNum = 0) {
            int val = Convert.ToInt32((x / (double)slider.Width) * (slider.Maximum - slider.Minimum));
            if (Math.Abs(slider.Value - val) >= 1) {
                slider.Value = val;
                if (toy != null) toy.moveSliderEvent(val, vibeNum);
            }
        }

    }

    public class Toy {
        internal string name;
        internal string id;
        internal Hand hand;

        private Form1 form = Form1.vc.form;

        //keep track of these so we can delete them if toy disconnects
        internal Label nameText;
        internal ComboBox dropdown;
        internal TrackBar slider;
        internal TrackBar slider2;
        internal Button rotateButton;

        internal Toy(String name, string id) {
            this.name = name;
            this.id = id;
        }

        internal void rotate() {
            Form1.vc.Send("rotate " + id);
        }

        //move slider with trigger input
        internal void moveSlider(int newValue, int vibeNum = 0) {
            moveSliderEvent(newValue, vibeNum);

            //actually move the slider
            if (vibeNum == 0 || (name == "Edge" && vibeNum == 1)) slider.Invoke((Action)delegate () { slider.Value = newValue; });
            else slider2.Invoke((Action)delegate () { slider2.Value = newValue; });
        }

        //slider moved event
        internal void moveSliderEvent(int newValue, int vibeNum = 0) {
            Console.WriteLine("moveSlider " + name + " " + newValue + " " + vibeNum);

            //send stuff the server
            if (vibeNum == 0) Form1.vc.Send("speed " + id + " " + newValue);
            else {
                switch(name) {
                    case "Max":
                        Form1.vc.Send("air " + id + " " + newValue);
                        break;
                    case "Nora":
                        Form1.vc.Send("rotate " + id);
                        break;
                    case "Edge":
                        Form1.vc.Send("speed " + id + " " + newValue + " " + vibeNum);
                        break;
                }
            }
        }

        internal void moveDropdown(int newValue) {
            switch(newValue) {
                case 0: hand = Hand.SliderOnly; break;
                case 1: hand = Hand.Left; break;
                case 2: hand = Hand.Right; break;
                case 3: hand = Hand.Both; break;
            }
        }

        internal void remove() {
            form.Invoke((Action)delegate () {
                Console.WriteLine("Removing toy: " + name + " " + id);

                form.Controls.Remove(nameText);
                form.Controls.Remove(dropdown);
                form.Controls.Remove(slider);
                form.Controls.Remove(rotateButton);
                if (slider2 != null) form.Controls.Remove(slider2);
                form.toys.Remove(this);

                //scoot toys down
                form.rows = 0;
                foreach (Toy toy in form.toys) toy.fixPox();
            });
        }

        internal void fixPox() {
            dropdown.Location = new Point(9, 414 - (51 * (form.rows)));
            nameText.Location = new Point(9, 388 - (51 * (form.rows)));
            slider.Location = new Point(110, 388 - (51 * (form.rows)));
            if (slider2 != null) {
                slider2.Location = new Point(110, 388 - (51 * (form.rows + 1)));
                form.rows++;
            } 
            if (rotateButton != null) {
                rotateButton.Location = new Point(110, 388 - (51 * (form.rows + 1)));
                form.rows++;
            }
            form.rows++;
        }

        internal enum Hand {
            Left,
            Right,
            Both,
            SliderOnly
        }

    }
}
