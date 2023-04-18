using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class CubeMarkerIndicatorController : MonoBehaviour {

        public Material nopMaterial;
        public Material markerOverObjMaterial;
        public Material attachedObjMaterial;

        void Start() {
            GetComponent<MeshRenderer>().material = nopMaterial;
        }

        public void SetStatus(CubeMarkerStatus status) {
            switch (status) {
                case CubeMarkerStatus.NOP:
                    GetComponent<MeshRenderer>().material = nopMaterial;
                    break;
                case CubeMarkerStatus.OBJECT_ATTACHED:
                    GetComponent<MeshRenderer>().material = attachedObjMaterial;
                    break;
                case CubeMarkerStatus.MARKER_OVER_OBJECT:
                    GetComponent<MeshRenderer>().material = markerOverObjMaterial;
                    break;
            }
        }

    }
}
