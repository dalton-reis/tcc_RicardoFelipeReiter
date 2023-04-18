using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class AnimationTake : Persistent<AnimationTakePersistData> {

        private Animation animation;
        private AnimationClip clip;
        private GameObject gameObject;
        private GORecorder.TransformCurves transformCurves;

        public Animation Animation {
            get {
                return animation;
            }
            set {
                animation = value;
            }
        }

        public AnimationClip Clip {
            get {
                return clip;
            }
            set {
                clip = value;
            }
        }

        public GameObject GameObject {
            get {
                return gameObject;
            }
            set {
                gameObject = value;
            }
        }

        public GORecorder.TransformCurves TransformCurves {
            get {
                return transformCurves;
            }
            set {
                transformCurves = value;
            }
        }

        public AnimationTake(Animation animation, AnimationClip clip, GameObject gameObject, GORecorder.TransformCurves transformCurves) {
            this.Animation = animation;
            this.Clip = clip;
            this.GameObject = gameObject;
            this.TransformCurves = transformCurves;
        }


        public AnimationTakePersistData GetPersistData() {
            AnimationTakePersistData data = new AnimationTakePersistData();

            data.PosX = GetKeyframePersistentData(transformCurves.posX.keys);
            data.PosY = GetKeyframePersistentData(transformCurves.posY.keys);
            data.PosZ = GetKeyframePersistentData(transformCurves.posZ.keys);

            data.RotX = GetKeyframePersistentData(transformCurves.rotX.keys);
            data.RotY = GetKeyframePersistentData(transformCurves.rotY.keys);
            data.RotZ = GetKeyframePersistentData(transformCurves.rotZ.keys);
            data.RotW = GetKeyframePersistentData(transformCurves.rotW.keys);

            return data;
        }

        private static KeyframePersistData[] GetKeyframePersistentData(Keyframe[] keyframes) {
            KeyframePersistData[] data = new KeyframePersistData[keyframes.Length];

            for (var i = 0; i < keyframes.Length; i++) {
                data[i] = new KeyframePersistData();
                data[i].Time = keyframes[i].time;
                data[i].Value = keyframes[i].value;
            }

            return data;
        }

        public void LoadPersistData(AnimationTakePersistData data) {
            var recorder = new GORecorder();

            var curves = new GORecorder.TransformCurves();
            curves.posX = new AnimationCurve(GetKeyframeFromPersistData(data.PosX));
            curves.posY = new AnimationCurve(GetKeyframeFromPersistData(data.PosY));
            curves.posZ = new AnimationCurve(GetKeyframeFromPersistData(data.PosZ));

            curves.rotX = new AnimationCurve(GetKeyframeFromPersistData(data.RotX));
            curves.rotY = new AnimationCurve(GetKeyframeFromPersistData(data.RotY));
            curves.rotZ = new AnimationCurve(GetKeyframeFromPersistData(data.RotZ));
            curves.rotW = new AnimationCurve(GetKeyframeFromPersistData(data.RotW));

            recorder.SetCurves(curves);

            var clip = new AnimationClip();
            clip.name = "clip";
            recorder.SaveToClip(clip);

            var animation = GameObject.AddComponent<Animation>();
            animation.playAutomatically = false;
            animation.AddClip(clip, "clip");

            this.Animation = animation;
            this.Clip = clip;
            this.TransformCurves = recorder.GetCurves();
        }

        private static Keyframe[] GetKeyframeFromPersistData(KeyframePersistData[] keyframesPersist) {
            Keyframe[] data = new Keyframe[keyframesPersist.Length];

            for (var i = 0; i < keyframesPersist.Length; i++) {
                data[i] = new Keyframe(keyframesPersist[i].Time, keyframesPersist[i].Value);
            }

            return data;
        }
    }
}