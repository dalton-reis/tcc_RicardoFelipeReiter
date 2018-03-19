using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class SceneController : MonoBehaviour, CubeMarkerObjectReceiver {

        public void ObjectReceived(GameObject obj) {
            obj.transform.parent = this.transform;
        }

    }
}
