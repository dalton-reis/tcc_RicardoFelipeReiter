using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public abstract class VuforiaUtils : MonoBehaviour {

        public static void EnableTargetObject(GameObject obj) {
            var rendererComponents = obj.GetComponentsInChildren<Renderer>(true);
            var colliderComponents = obj.GetComponentsInChildren<Collider>(true);
            var canvasComponents = obj.GetComponentsInChildren<Canvas>(true);

            // Enable rendering:
            foreach (var component in rendererComponents)
                component.enabled = true;

            // Enable colliders:
            foreach (var component in colliderComponents)
                component.enabled = true;

            // Enable canvas':
            foreach (var component in canvasComponents)
                component.enabled = true;
        }

    }
}
