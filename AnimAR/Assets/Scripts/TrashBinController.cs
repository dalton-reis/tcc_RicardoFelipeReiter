using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class TrashBinController : MonoBehaviour, CubeMarkerInteractor {

        public GameObject IncinerateEffectGO;

        public bool ObjectReceived(GameObject obj) {
            Destroy(obj);
            var effectObj = GameObject.Instantiate(IncinerateEffectGO);
            effectObj.transform.parent = this.transform;
            effectObj.transform.localPosition = Vector3.zero;
            effectObj.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 90));
            return true;
        }

        public void ObjectRemoved(GameObject obj) {

        }

    }
}
