using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class TrashBinController : CubeMarkerInteractorImpl {

        public AnimationController AnimationController;
        public SceneController SceneController;
        public GameObject IncinerateEffectGO;

        public override bool CanReceiveObject(MovableObject obj) {
            return true;
        }

        public override void ObjectReceived(MovableObject obj) {
            var takeNumber = -1;
            switch (obj.type) {
                case MovableObject.TYPE.TAKE_OBJECT:
                    takeNumber = obj.GetComponent<NumberIcon>().Number;
                    break;
                case MovableObject.TYPE.SCENE_OBJECT:
                    takeNumber = SceneController.GetCurrentScene().Takes.FindIndex(take => take.GameObject == obj.gameObject);
                    break;
            }
            Destroy(obj.gameObject);

            if (takeNumber > -1) {
                AnimationController.RemoveTake(takeNumber);
            }

            var effectObj = GameObject.Instantiate(IncinerateEffectGO);
            effectObj.transform.parent = this.transform;
            effectObj.transform.localPosition = Vector3.zero;
            effectObj.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 90));
        }

        public override void ObjectRemoved(MovableObject obj) {

        }

    }
}
