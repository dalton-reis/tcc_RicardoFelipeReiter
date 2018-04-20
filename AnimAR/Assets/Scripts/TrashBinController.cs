using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class TrashBinController : MonoBehaviour, CubeMarkerInteractor {

        public AnimationController AnimationController;
        public GameObject IncinerateEffectGO;

        public bool CanReceiveObject(MovableObject obj) {
            return true;
        }

        public void ObjectReceived(MovableObject obj) {
            switch (obj.type) {
                case MovableObject.TYPE.TAKE_OBJECT:
                    AnimationController.RemoveTake(obj.GetComponent<NumberIcon>().Number);
                    break;
            }

            Destroy(obj.gameObject);
            var effectObj = GameObject.Instantiate(IncinerateEffectGO);
            effectObj.transform.parent = this.transform;
            effectObj.transform.localPosition = Vector3.zero;
            effectObj.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 90));
        }

        public void ObjectRemoved(MovableObject obj) {

        }

    }
}
