using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class AnimationController : MonoBehaviour {

        public bool isPlaying = false;
        public SceneController SceneController;

        private AnimationTake longestTake;
        private float currentTime = 0.0f;
        private float endTime = 0.0f;
        private int currentTake = 0;

        public float CurrentTime {
            get {
                return currentTime;
            }
            private set {
                currentTime = value;
            }
        }

        public float EndTime {
            get {
                return endTime;
            }
            private set {
                endTime = value;
            }
        }

        public int CurrentTake {
            get {
                return currentTake;
            }
            set {
                currentTake = value;
                NotifyCurrentTakeChanged();
            }
        }

        private LinkedList<AnimationControllerListener> listeners = new LinkedList<AnimationControllerListener>();

        void Update() {
            if (isPlaying) {
                currentTime = longestTake.Animation["clip"].time;
                if (!longestTake.Animation.isPlaying) {
                    isPlaying = false;
                }
            }
        }

        public void CreateNewTakeAtCurrentPos(GORecorder recorder, GameObject recordedObject) {
            Animation animation = recordedObject.GetComponent<Animation>();
            if (animation) {
                animation.RemoveClip("clip");
            } else {
                animation = recordedObject.AddComponent<Animation>();
            }

            var clip = new AnimationClip();
            clip.name = "clip";
            clip.legacy = true;
            recorder.SaveToClip(clip);

            animation.playAutomatically = false;
            animation.AddClip(clip, "clip");

            var newTake = new AnimationTake(animation, clip, recordedObject);

            var animationIndex = SceneController.GetCurrentScene().Takes.FindIndex(take => take.Animation == animation);
            // O objeto gravado já pertence à alguma take: substitui-la deverá
            if (animationIndex >= 0) {
                CurrentTake = animationIndex;
                SceneController.GetCurrentScene().Takes[CurrentTake] = newTake;
            } else {
                CurrentTake = SceneController.GetCurrentScene().Takes.Count;
                SceneController.GetCurrentScene().Takes.Add(newTake);
            }

            NotifyCurrentTakeChanged();
            CalculateClipTimes();

            currentTime = 0.0f;
        }

        public void CalculateClipTimes() {
            foreach (var take in SceneController.GetCurrentScene().Takes) {
                if (endTime < take.Clip.length) {
                    endTime = take.Clip.length;
                    longestTake = take;
                }
            }
        }

        public void PlayAll() {
            foreach (AnimationTake take in SceneController.GetCurrentScene().Takes) {
                take.Animation.Play("clip");
            }
            if (SceneController.GetCurrentScene().Takes.Count > 0) {
                isPlaying = true;
            }
        }

        public void StopAll() {
            foreach (AnimationTake take in SceneController.GetCurrentScene().Takes) {
                take.Animation.Stop("clip");
            }
            isPlaying = false;
        }

        public void RewindAll() {
            foreach (AnimationTake take in SceneController.GetCurrentScene().Takes) {
                AnimationState state = take.Animation["clip"];
                if (state) {
                    state.enabled = true;
                    state.weight = 1;
                    state.normalizedTime = 0.01f;

                    take.Animation.Sample();

                    state.enabled = false;
                }
            }
            currentTime = 0.0f;
        }

        public void AddListener(AnimationControllerListener listener) {
            listeners.AddLast(listener);
        }

        public void NotifyCurrentTakeChanged() {
            foreach (var listener in listeners) {
                listener.CurrentTakeChanged(currentTake);
            }
        }

        public float[] GetTakesTime() {
            var times = new float[SceneController.GetCurrentScene().Takes.Count];

            for (var i = 0; i < SceneController.GetCurrentScene().Takes.Count; i++) {
                times[i] = SceneController.GetCurrentScene().Takes[i].Clip.length;
            }

            return times;
        }

        public void SceneChanged() {
            CurrentTake = 0;
            NotifyCurrentTakeChanged();
        }

    }
}
