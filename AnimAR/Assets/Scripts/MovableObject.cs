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

        void Awake() {
            currentInteractor = GetComponentInParent<CubeMarkerInteractor>();
        }

    }
}
