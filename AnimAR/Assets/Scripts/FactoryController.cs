using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class FactoryController : MonoBehaviour, IVirtualButtonEventHandler, CubeMarkerInteractor {

        public GameObject NextButton;
        public GameObject PrevButton;
        public GameObject[] objList;
        public Transform showingObjectRoot;

        private int currentObjIndex = 0;
        private GameObject currentObject = null;

        void Start() {
            NextButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            PrevButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            ChangeIndex(0);
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
            NewCurrentObject(objList[currentObjIndex]);
        }

        public bool ObjectReceived(GameObject obj) {
            return false;
        }

        public void ObjectRemoved(GameObject obj) {
            Debug.Log(currentObject);
            Debug.Log(obj);
            if (currentObject == obj) {
                Debug.Log("lol2");
                NewCurrentObject(obj);
            }
        }

        public void NewCurrentObject(GameObject objToCopy) {
            currentObject = GameObject.Instantiate(objToCopy, showingObjectRoot);
            currentObject.transform.localPosition = new Vector3(0, 0, 0);
        }

    }
}
