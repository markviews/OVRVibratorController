using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Valve.VR;
using WebSocketSharp;

namespace VibratorController {
    class OVRVibratorController {

        /* 
         * This version does not work, pressing the "Add Toy" button just makes a client side toy for testing and does not contact the server
         * 
         * Bug:
         * (rare) OVR dosen't register trigger input, maybe auto detect and restart OVR connection? (trigger button press with no trigger input)
         * 
         * Features to add:
         * Nora's rotate controll
         * Save Hold/Lock prefrences to file and load on startup
         */

        private WebSocket ws;
        private CVRSystem VRSystem;
        private List<uint> controllers = new List<uint>();
        private uint leftIndex, rightIndex;

        internal uint holdButton, LockButton;
        internal int holdController = -1, LockController = -1;//-1 = none, -2 = searching

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        internal void SetupClient() {
            ws = new WebSocket("wss://control.markstuff.net:8080");

            ws.OnMessage += (sender, e) => {
                Console.WriteLine("msg from server: " + e.Data);
            };

            ws.OnOpen += (sender, e) => {
                Console.WriteLine("connected to server!");
                Form1.setServerStatus(true);
            };

            ws.OnClose += (sender, e) => {
                Console.WriteLine("disconnected from server!");
                Form1.setServerStatus(false);
            };

            ws.OnError += (sender, e) => {
                Console.WriteLine("error: " + e.Message);
            };

            ws.Connect();
            ws.Send("join 12P6");
        }

        internal void SetupOVR() {
            //hook vr
            EVRInitError error = EVRInitError.None;
            try {
                VRSystem = OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background);
            } catch (BadImageFormatException e) {
                MessageBox.Show("Failed to hook OpenVR. Incorrect version of openvr_api.dll used (36 or 64 bit)");
                Form1.setOVRStatus(false);
                return;
            }
            
            if (error == EVRInitError.None) {
                Form1.setOVRStatus(true);
            } else {
                Form1.setOVRStatus(false);
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
                if (rightIndex > max) Form1.setControllerStatus("Can't find left or right controllers");
                else Form1.setControllerStatus("Can't find left controller");
            } else if (rightIndex > max) {
                Form1.setControllerStatus("Can't find right controller");
            } else Form1.setControllerStatus("good");
        }

        private void Update() {
            if (VRSystem == null) return;

            //check buttons
            VREvent_t pEvent = new VREvent_t();
            while (VRSystem.PollNextEvent(ref pEvent, (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(VREvent_t)))) {
                if (pEvent.eventType == 200) {
                    uint index = pEvent.trackedDeviceIndex;
                    uint button = pEvent.data.controller.button;

                    Console.WriteLine("press: " + index + " " + button);

                    if (holdController == -2) {
                        holdController = (int)index;
                        holdButton = button;
                        Form1.setHoldButtonText(button.ToString());
                        return;
                    }

                    if (LockController == -2) {
                        LockController = (int)index;
                        LockButton = button;
                        Form1.setLockButtonText(button.ToString());
                        return;
                    }

                } else if (pEvent.eventType == 201) {
                    //un-press
                }
            }

            //check triggers
            foreach (uint index in controllers) {
                VRControllerState_t state = new VRControllerState_t();
                VRSystem.GetControllerState(index, ref state, (uint)System.Runtime.InteropServices.Marshal.SizeOf(state));
                float value = state.rAxis1.x;
                int sliderVal = Convert.ToInt32(value * Form1.getSliderMax());

                foreach (Toy toy in Form1.toys.ToArray()) {
                    switch (toy.hand) {
                        case Toy.Hand.Left:
                            if (index == leftIndex) toy.moveSlider(sliderVal);
                            break;
                        case Toy.Hand.Right:
                            if (index == rightIndex) toy.moveSlider(sliderVal);
                            break;
                        case Toy.Hand.Both:
                            if (toy.name == "Edge" || toy.name == "Max") {
                                if (index == leftIndex) toy.moveSlider(sliderVal);
                                else toy.moveSlider(sliderVal, 2);
                            } else {
                                int left, right;

                                if (index == leftIndex) {
                                    VRSystem.GetControllerState(rightIndex, ref state, (uint)System.Runtime.InteropServices.Marshal.SizeOf(state));
                                    left = sliderVal;
                                    right = Convert.ToInt32(state.rAxis1.x * Form1.getSliderMax());
                                } else {
                                    VRSystem.GetControllerState(leftIndex, ref state, (uint)System.Runtime.InteropServices.Marshal.SizeOf(state));
                                    left = Convert.ToInt32(state.rAxis1.x * Form1.getSliderMax());
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

    }
}
