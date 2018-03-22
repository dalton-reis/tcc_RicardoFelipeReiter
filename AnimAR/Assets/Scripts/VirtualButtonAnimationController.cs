using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class VirtualButtonAnimationController : MonoBehaviour, IVirtualButtonEventHandler {

        public Material standyByMaterial;
        public Material overMaterial;
        public GameObject VirtualButton;

        void Start() {
            VirtualButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            GetComponent<MeshRenderer>().material = standyByMaterial;
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb) {
 	        GetComponent<MeshRenderer>().material = overMaterial;
        }

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
 	        GetComponent<MeshRenderer>().material = standyByMaterial;
        }
    }
}
