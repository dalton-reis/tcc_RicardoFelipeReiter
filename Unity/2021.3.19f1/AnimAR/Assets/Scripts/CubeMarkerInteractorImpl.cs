using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts {

    public abstract class CubeMarkerInteractorImpl : MonoBehaviour, CubeMarkerInteractor {

        public GameObject outlineObject;

        public void OnCubeMarkerEnter() {
            if (outlineObject) outlineObject.SetActive(true);
        }

        public void OnCubeMarkerExit() {
            if (outlineObject) outlineObject.SetActive(false);
        }

        public abstract bool CanReceiveObject(MovableObject obj);

        public abstract bool ObjectReceived(MovableObject obj);

        public abstract void ObjectRemoved(MovableObject obj);
    }
}
