using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class MovableObject : MonoBehaviour {

        public enum TYPE {
            SCENE_OBJECT, SCENE_INFO_OBJECT, TAKE_OBJECT
        }

        public TYPE type;
        public CubeMarkerInteractor currentInteractor;
        public ObjectOutliner outliner;

        void Awake() {
            currentInteractor = GetComponentInParent<CubeMarkerInteractor>();
        }

        void OnTriggerEnter(Collider other) {
            //var cubeMarker = other.GetComponent<CubeMarkerController>();
            var inspector = other.GetComponent<InspectorController>();

            if (inspector) {
                if (outliner) {
                    outliner.SetColor(Color.green);
                    outliner.SetEnabled(true);
                }
            }
        }

        void OnTriggerExit(Collider other) {
            var inspector = other.GetComponent<InspectorController>();

            if (inspector) {
                if (outliner) {
                    outliner.SetEnabled(false);
                }
            }
        }

    }
}
