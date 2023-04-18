using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class VirtualButtonHandler : MonoBehaviour //, IVirtualButtonEventHandler 
    {

        public float holdOnTime = 1;
        public GameObject[] Buttons;

        private Dictionary<String, float> pressedTime = new Dictionary<String,float>();
        private List<VirtualButtonListener> listeners = new List<VirtualButtonListener>();

        void Start() {
            foreach (var button in Buttons) {
                
                //button.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
                
                //new
                button.GetComponent<VirtualButtonBehaviour>().RegisterOnButtonPressed(OnButtonPressed);
                button.GetComponent<VirtualButtonBehaviour>().RegisterOnButtonReleased(OnButtonReleased);

            }
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb) {
            if (pressedTime.ContainsKey(vb.VirtualButtonName)) {
                pressedTime.Remove(vb.VirtualButtonName);
            }
            pressedTime.Add(vb.VirtualButtonName, Time.time);
        }

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
            float time;
            pressedTime.TryGetValue(vb.VirtualButtonName, out time);
            if ((Time.time - time) >= holdOnTime) {
                foreach (var listener in listeners) {
                    listener.ButtonPressed(vb);
                }
            }
        }

        public void AddListener(VirtualButtonListener listener) {
            listeners.Add(listener);
        }

    }
}
