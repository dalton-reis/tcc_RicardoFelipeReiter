using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class TrashBinController : MonoBehaviour, CubeMarkerObjectReceiver {

        public GameObject IncinerateEffectGO;

        public void ObjectReceived(GameObject obj) {
            Destroy(obj);
            var effectObj = GameObject.Instantiate(IncinerateEffectGO);
            effectObj.transform.parent = this.transform;
            effectObj.transform.localPosition = Vector3.zero;
            effectObj.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 90));
        }

    }
}
