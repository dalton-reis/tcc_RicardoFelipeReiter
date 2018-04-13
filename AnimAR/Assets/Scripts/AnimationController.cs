using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class AnimationController : MonoBehaviour {

        public List<AnimationTake> takes = new List<AnimationTake>();
        public bool isPlaying = false;

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

        public void StartRecording(GameObject objectToRecord) {
            StopAll();
            RewindAll();

            Animation animation = objectToRecord.GetComponent<Animation>();
            if (animation) {
                animation.RemoveClip("clip");
            } else {
                animation = objectToRecord.AddComponent<Animation>();
            }

            var animationIndex = takes.FindIndex(take => take.Animation == animation);
            if (animationIndex >= 0) {
                CurrentTake = animationIndex;
            }

            animation.playAutomatically = false;
            var newTake = new AnimationTake(animation, null, objectToRecord);

            if (CurrentTake == takes.Count) {
                takes.Add(newTake);
            } else {
                takes[CurrentTake] = newTake;
            }

            NotifyCurrentTakeChanged();
            currentTime = 0.0f;
        }

        public void StopRecording(GORecorder recorder) {
            var clip = new AnimationClip();
            clip.name = "clip";
            clip.legacy = true;
            recorder.SaveToClip(clip);

            takes[CurrentTake].Animation.AddClip(clip, "clip");
            takes[CurrentTake].Clip = clip;

            foreach (var take in takes) {
                if (endTime < take.Clip.length) {
                    endTime = take.Clip.length;
                    longestTake = take;
                }
            }

            currentTime = 0.0f;
        }

        public void PlayAll() {
            foreach (AnimationTake take in takes) {
                take.Animation.Play("clip");
            }
            if (takes.Count > 0) {
                isPlaying = true;
            }
        }

        public void StopAll() {
            foreach (AnimationTake take in takes) {
                take.Animation.Stop("clip");
            }
            isPlaying = false;
        }

        public void RewindAll() {
            foreach (AnimationTake take in takes) {
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

    }
}
