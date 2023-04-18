using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class RecorderTracker : MonoBehaviour {

        public Transform source;

        void LateUpdate() {
            if (source) {
                this.transform.SetPositionAndRotation(source.localPosition, source.localRotation);
            }
        }
    }
}
