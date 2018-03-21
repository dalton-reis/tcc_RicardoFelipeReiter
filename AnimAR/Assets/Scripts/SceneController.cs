using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class SceneController : MonoBehaviour, CubeMarkerInteractor {

        public bool ObjectReceived(GameObject obj) {
            obj.transform.parent = this.transform;
            return true;
        }

        public void ObjectRemoved(GameObject obj) {

        }

    }
}
