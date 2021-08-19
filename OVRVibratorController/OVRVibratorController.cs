using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using Valve.VR;
using WebSocketSharp;

namespace VibratorController {
    class OVRVibratorController {

        internal Dictionary<string, string> settings = new Dictionary<string, string>();
        private WebSocket ws;
        private CVRSystem VRSystem;
        private List<uint> controllers = new List<uint>();
        private uint leftIndex, rightIndex;
        internal Form1 form;
        internal bool lockSpeed = false, holding = true;
        internal uint holdButton, LockButton;
        internal int holdController = -1, lockController = -1;//-1 = none, -2 = searching

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        internal void Setup() {
            SetupSettings();
            SetupClient();
            SetupOVR();
        }

        private void SetupSettings() {
            if (!File.Exists("settings.txt")) {
                settings.Add("hold", "None");
                settings.Add("lock", "None");
                SaveSettings();
            }

            settings = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(File.ReadAllText("settings.txt"));
            form.setLockButtonText(settings["lock"]);
            form.setHoldButtonText(settings["hold"]);
        }

        internal void SaveSettings() {
            File.WriteAllText("settings.txt", new JavaScriptSerializer().Serialize(settings));
        }

        private void SetupClient() {
            ws = new WebSocket("wss://control.markstuff.net:8080");

            ws.OnOpen += (sender, e) => {
                Console.WriteLine("connected to server!");
                form.setServerStatus(true);
            };

            ws.OnClose += (sender, e) => {
                Console.WriteLine("disconnected from server!");
                form.setServerStatus(false);
            };

            ws.OnError += (sender, e) => {
                Console.WriteLine("error: " + e.Message);
            };

            ws.OnMessage += (sender, e) => {
                Console.WriteLine("msg from server: " + e.Data);

                string[] args = e.Data.Split(' ');
                string[] data;

                switch (args[0]) {
                    case "toys":
                    case "add"://add toy

                        for (int i = 1; i < args.Length; i++) {
                            data = args[i].Split(':');
                            form.addToy(data[0], data[1]);
                        }

                        break;
                    case "remove"://remove toy
                        data = args[1].Split(':');
                        string id = data[1];

                        foreach (Toy toy in form.toys.ToArray()) {
                            if (toy.id == id) {
                                toy.remove();
                                break;
                            }
                        }

                        break;
                    case "notFound":
                        MessageBox.Show("invalid code");
                        break;
                    case "left":
                        foreach (Toy toy in form.toys.ToArray()) toy.remove();
                        MessageBox.Show("user left the session");
                        break;
                }

            };

            ws.Connect();
        }

        private void SetupOVR() {
            //hook vr
            EVRInitError error = EVRInitError.None;
            try {
                VRSystem = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background);
            } catch (BadImageFormatException) {
                MessageBox.Show("Failed to hook OpenVR. Incorrect version of openvr_api.dll used (36 or 64 bit)");
                form.setOVRStatus(false);
                return;
            }
            
            if (error == EVRInitError.None) {
                form.setOVRStatus(true);
            } else {
                form.setOVRStatus(false);
                return;
            }

            //find controllers
            uint max = OpenVR.k_unMaxTrackedDeviceCount;
            for (uint i = 0; i < max + 1; i++) {
                ETrackedDeviceClass c = VRSystem.GetTrackedDeviceClass(i);
                if (c == ETrackedDeviceClass.Controller) {
                    Console.WriteLine("Found controller with index: " + i);
                    controllers.Add(i);
                }
            }

            //button press event
            System.Timers.Timer tmr = new System.Timers.Timer();
            tmr.Elapsed += (sender, args) => Update();
            tmr.AutoReset = true;
            tmr.Interval = 100;
            tmr.Start();

            leftIndex = VRSystem.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.LeftHand);
            rightIndex = VRSystem.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);

            if (leftIndex > max) {
                if (rightIndex > max) form.setControllerStatus("Can't find left or right controllers");
                else form.setControllerStatus("Can't find left controller");
            } else if (rightIndex > max) {
                form.setControllerStatus("Can't find right controller");
            } else form.setControllerStatus("good");
        }

        private void Update() {
            if (VRSystem == null) return;

            //check buttons
            VREvent_t pEvent = new VREvent_t();
            while (VRSystem.PollNextEvent(ref pEvent, (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(VREvent_t)))) {
                uint button = pEvent.data.controller.button;
                uint index = pEvent.trackedDeviceIndex;

                if (pEvent.eventType == 200) {
                    Console.WriteLine("press: " + index + " " + button);

                    if (holdController == -2) {
                        holdController = (int)index;
                        holdButton = button;
                        form.setHoldButtonText(button.ToString());
                        settings["hold"] = button.ToString();
                        holding = false;
                        SaveSettings();
                        return;
                    }

                    if (lockController == -2) {
                        lockController = (int)index;
                        LockButton = button;
                        form.setLockButtonText(button.ToString());
                        settings["lock"] = button.ToString();
                        SaveSettings();
                        return;
                    }

                    if (button.ToString() == settings["hold"]) holding = true;
                    if (button.ToString() == settings["lock"]) {
                        lockSpeed = !lockSpeed;
                        Console.WriteLine("lock: " + lockSpeed);
                    }
                } else if (pEvent.eventType == 201) {
                    //un-press
                    if (button.ToString() == settings["hold"]) holding = false;
                }
            }

            //check triggers
            foreach (uint index in controllers) {
                VRControllerState_t state = new VRControllerState_t();
                VRSystem.GetControllerState(index, ref state, (uint)System.Runtime.InteropServices.Marshal.SizeOf(state));
                float value = state.rAxis1.x;
                int sliderVal = Convert.ToInt32(value * form.getSliderMax());

                if (lockSpeed) return;

                foreach (Toy toy in form.toys.ToArray()) {

                    if (!holding) {
                        if (toy.name == "Edge") {
                            toy.moveSlider(0, 1);
                            toy.moveSlider(0, 2);
                        } else
                            toy.moveSlider(0);
                        return;
                    }

                    switch (toy.hand) {
                        case Toy.Hand.Left:
                            if (index == leftIndex) {
                                if (toy.name == "Edge") {
                                    toy.moveSlider(sliderVal, 1);
                                    toy.moveSlider(sliderVal, 2);
                                } else
                                    toy.moveSlider(sliderVal);
                            }
                            break;
                        case Toy.Hand.Right:
                            if (index == rightIndex) {
                                if (toy.name == "Edge") {
                                    toy.moveSlider(sliderVal, 1);
                                    toy.moveSlider(sliderVal, 2);
                                } else
                                    toy.moveSlider(sliderVal);
                            }
                            break;
                        case Toy.Hand.Both:
                            if (toy.name == "Edge" || toy.name == "Max") {
                                if (index == leftIndex) toy.moveSlider(sliderVal, 1);
                                else toy.moveSlider(sliderVal, 2);
                            } else {
                                int left, right;

                                if (index == leftIndex) {
                                    VRSystem.GetControllerState(rightIndex, ref state, (uint)System.Runtime.InteropServices.Marshal.SizeOf(state));
                                    left = sliderVal;
                                    right = Convert.ToInt32(state.rAxis1.x * form.getSliderMax());
                                } else {
                                    VRSystem.GetControllerState(leftIndex, ref state, (uint)System.Runtime.InteropServices.Marshal.SizeOf(state));
                                    left = Convert.ToInt32(state.rAxis1.x * form.getSliderMax());
                                    right = sliderVal;
                                }

                                if (left > right) toy.moveSlider(left);
                                else toy.moveSlider(right);
                            }
                            break;
                    }
                }
                    
            }
        }

        internal void Send(string msg) {
            ws.Send(msg);
            /*
            ws.SendAsync(msg, (Action<bool>)delegate (bool success) {
                Console.WriteLine(success + " " + msg);
            });
            */
        }

    }
}
