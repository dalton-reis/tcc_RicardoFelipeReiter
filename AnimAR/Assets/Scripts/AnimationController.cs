using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Vuforia;

namespace Assets.Scripts {
    public class AnimationController : MonoBehaviour {

        public List<Animation> takes = new List<Animation>();
        private int currentTake = 0;
        private float currentTime = 0.0f;
        public bool isPlaying = false;

        void Update() {
            if (isPlaying) {
                currentTime = takes[0]["clip"].time;
                if (!takes[0].isPlaying) {
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
                Debug.Log("Removed");
            } else {
                animation = objectToRecord.AddComponent<Animation>();
            }

            animation.playAutomatically = false;
            if (currentTake == takes.Count) {
                takes.Add(animation);
            } else {
                Debug.Log("added array");
                takes[currentTake] = animation;
            }
        }

        public void StopRecording(GORecorder recorder) {
            var clip = new AnimationClip();
            clip.name = "clip";
            clip.legacy = true;
            recorder.SaveToClip(clip);
            takes[currentTake].AddClip(clip, "clip");
        }

        public void PlayAll() {
            foreach (Animation take in takes) {
                take.Play("clip");
            }
            if (takes.Count > 0) {
                isPlaying = true;
            }
        }

        public void StopAll() {
            foreach (Animation take in takes) {
                take.Stop("clip");
            }
            isPlaying = false;
        }

        public void RewindAll() {
            foreach (Animation take in takes) {
                AnimationState state = take["clip"];
                if (state) {
                    state.enabled = true;
                    state.weight = 1;
                    state.normalizedTime = 0.01f;

                    take.Sample();

                    state.enabled = false;
                }
            }
            currentTime = 0.0f;
        }

    }
}
