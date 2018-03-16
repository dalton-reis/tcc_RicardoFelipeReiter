using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class CubeMarkerController : MonoBehaviour {

        public CubeMarkerIndicatorController indicatorController;

        private GameObject scene = null;
        private GameObject objectMarkerOver = null;
        private GameObject attachedObject = null;
        private CubeMarkerStatus currentStatus = CubeMarkerStatus.NOP;

        void OnTriggerEnter(Collider other) {
            Debug.Log(other);
            switch (other.tag) {
                case "Scene":
                    scene = other.gameObject;
                    break;
                case "Movable":
                    objectMarkerOver = other.gameObject;
                    break;
            }
            UpdateStatus();
        }

        void OnTriggerExit(Collider other) {
            Debug.Log(other);
            switch (other.tag) {
                case "Scene":
                    if (other.gameObject == scene) {
                        scene = null;
                    }
                    break;
                case "Movable":
                    if (other.gameObject == objectMarkerOver) {
                        objectMarkerOver = null;
                    }
                    break;
            }
            UpdateStatus();
        }

        private void UpdateStatus() {
            if (attachedObject) {
                currentStatus = CubeMarkerStatus.OBJECT_ATTACHED;
            } else if (objectMarkerOver) {
                currentStatus = CubeMarkerStatus.MARKER_OVER_OBJECT;
            } else {
                currentStatus = CubeMarkerStatus.NOP;
            }
            indicatorController.SetStatus(currentStatus);
        }

    }
}
