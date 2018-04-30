using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class GORecorder {

        public GameObject source;
        public float currentTime = 0;

        private TransformCurves curves = new TransformCurves();

        public class TransformCurves {
            public AnimationCurve posX = new AnimationCurve();
            public AnimationCurve posY = new AnimationCurve();
            public AnimationCurve posZ = new AnimationCurve();

            public AnimationCurve rotX = new AnimationCurve();
            public AnimationCurve rotY = new AnimationCurve();
            public AnimationCurve rotZ = new AnimationCurve();
            public AnimationCurve rotW = new AnimationCurve();

            public void Snapshot(Transform transform, float time) {
                if (TransformHasChanged(transform)) {
                    posX.AddKey(time, transform.localPosition.x);
                    posY.AddKey(time, transform.localPosition.y);
                    posZ.AddKey(time, transform.localPosition.z);

                    rotX.AddKey(time, transform.localRotation.x);
                    rotY.AddKey(time, transform.localRotation.y);
                    rotZ.AddKey(time, transform.localRotation.z);
                    rotW.AddKey(time, transform.localRotation.w);
                }
            }

            public bool TransformHasChanged(Transform transform) {
                if (posX.keys.Count() == 0) return true;
                return (posX.keys.ElementAt(posX.keys.Count() - 1).value != transform.localPosition.x)
                    || (posY.keys.ElementAt(posY.keys.Count() - 1).value != transform.localPosition.y)
                    || (posZ.keys.ElementAt(posZ.keys.Count() - 1).value != transform.localPosition.z)
                    || (rotX.keys.ElementAt(rotX.keys.Count() - 1).value != transform.localRotation.x)
                    || (rotY.keys.ElementAt(rotY.keys.Count() - 1).value != transform.localRotation.y)
                    || (rotZ.keys.ElementAt(rotZ.keys.Count() - 1).value != transform.localRotation.z)
                    || (rotW.keys.ElementAt(rotW.keys.Count() - 1).value != transform.localRotation.w);
            }

        }

        public void TakeSnapshot(float time) {
            if (currentTime == 0) {
                curves.Snapshot(source.transform, 0);
            }
            currentTime += time;
            curves.Snapshot(source.transform, currentTime);
        }

        public void ResetRecording() {
            curves = new TransformCurves();
            currentTime = 0;
        }

        public void SaveToClip(AnimationClip clip) {
            clip.legacy = true;
            clip.SetCurve("", typeof(Transform), "localPosition.x", curves.posX);
            clip.SetCurve("", typeof(Transform), "localPosition.y", curves.posY);
            clip.SetCurve("", typeof(Transform), "localPosition.z", curves.posZ);

            clip.SetCurve("", typeof(Transform), "localRotation.x", curves.rotX);
            clip.SetCurve("", typeof(Transform), "localRotation.y", curves.rotY);
            clip.SetCurve("", typeof(Transform), "localRotation.z", curves.rotZ);
            clip.SetCurve("", typeof(Transform), "localRotation.w", curves.rotW);

            clip.EnsureQuaternionContinuity();

            Debug.Log("Curve KeyFrame Quantity: " + curves.posX.keys.Count());
        }

        public TransformCurves GetCurves() {
            return this.curves;
        }

        public void SetCurves(TransformCurves curves) {
            this.curves = curves;
        }

    }
}
