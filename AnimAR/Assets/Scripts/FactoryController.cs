using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class FactoryController : MonoBehaviour, IVirtualButtonEventHandler {

        public VirtualButtonBehaviour NextButton;
        public VirtualButtonBehaviour PrevButton;
        public GameObject[] objList;

        private int currentObjIndex = 0;
        private GameObject currentObject = null;

        void Start() {
            NextButton.RegisterEventHandler(this);
            PrevButton.RegisterEventHandler(this);
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb) {
            switch (vb.VirtualButtonName) {
                case "Next":
                    ChangeIndex(currentObjIndex + 1);
                    break;
                case "Prev":
                    ChangeIndex(currentObjIndex - 1);
                    break;
            }
        }

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
        }

        private void ChangeIndex(int index) {
            currentObjIndex = Math.Abs(index % objList.Length);
            if (currentObject) {
                Destroy(currentObject);
            }
            currentObject = GameObject.Instantiate(objList[currentObjIndex]);
            currentObject.transform.parent = this.transform;
            currentObject.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
    }
}
