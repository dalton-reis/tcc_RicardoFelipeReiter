using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class AnimationController : MonoBehaviour, SceneControllerListener, CubeMarkerListener {

        public SceneController SceneController;
        public RecorderTracker recorderTracker;

        public enum STATUS {
            IDLE, WAITING_OBJECT_TO_ATTACH, RECORDING, PLAYING
        }

        private STATUS status;

        private AnimationTake longestTake;
        private float currentTime = 0.0f;
        private float endTime = 0.0f;

        private GORecorder recorder;

        private CubeMarkerController cubeMarkerController;

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

        public STATUS Status {
            get {
                return status;
            }
            private set {
                status = value;
                NotifyStatusChanged();
            }
        }

        private LinkedList<AnimationControllerListener> listeners = new LinkedList<AnimationControllerListener>();

        void Start() {
            recorder = new GORecorder();
            recorder.source = recorderTracker.gameObject;

            cubeMarkerController = GameObject.FindObjectOfType<CubeMarkerController>();
            cubeMarkerController.AddListener(this);
            GameObject.FindObjectOfType<SceneController>().AddListener(this);

            ResetController();
        }

        void LateUpdate() {
            switch (status) {
                case STATUS.RECORDING:
                    CurrentTime = recorder.currentTime;
                    recorder.TakeSnapshot(Time.deltaTime);
                    NotifyAnimationTimesChanged();
                    break;
                case STATUS.PLAYING:
                    CurrentTime = longestTake.Animation["clip"].time;
                    if (!longestTake.Animation.isPlaying) {
                        CurrentTime = longestTake.Animation["clip"].length;
                        Status = STATUS.IDLE;
                    }
                    NotifyAnimationTimesChanged();
                    break;
            }
        }

        public void CreateNewTakeAtCurrentPos() {
            Animation animation = recorderTracker.source.GetComponent<Animation>();
            if (animation) {
                animation.RemoveClip("clip");
            } else {
                animation = recorderTracker.source.gameObject.AddComponent<Animation>();
            }

            var clip = new AnimationClip();
            clip.name = "clip";
            recorder.SaveToClip(clip);

            animation.playAutomatically = false;
            animation.AddClip(clip, "clip");

            var newTake = new AnimationTake(animation, clip, recorderTracker.source.gameObject, recorder.GetCurves());

            var animationIndex = SceneController.GetCurrentScene().Takes.FindIndex(take => take.Animation == animation);
            // O objeto gravado já pertence à alguma take: substitui-la deverá
            if (animationIndex >= 0) {
                SceneController.GetCurrentScene().Takes[animationIndex] = newTake;
            } else {
                SceneController.GetCurrentScene().Takes.Add(newTake);
                NotifyTakeAdded(SceneController.GetCurrentScene().Takes.Count() - 1);
            }

            CalculateClipTimes();
        }

        private void CalculateClipTimes() {
            EndTime = 0.0f;
            foreach (var take in SceneController.GetCurrentScene().Takes) {
                if (EndTime < take.Clip.length) {
                    EndTime = take.Clip.length;
                    longestTake = take;
                }
            }

            CurrentTime = 0.0f;
        }

        public void PlayAll() {
            // TODO: usar listener animation controller e dai o próprio cubeMarkerControler toma as ações próprias
            cubeMarkerController.ResetAttached();
            InternalPlayAll();
            if (SceneController.GetCurrentScene().Takes.Count > 0) {
                Status = STATUS.PLAYING;
            }
        }

        private void InternalPlayAll() {
            foreach (AnimationTake take in SceneController.GetCurrentScene().Takes) {
                if (recorderTracker.source != null && recorderTracker.source.gameObject == take.GameObject) {
                    continue;
                }
                take.Animation.Play("clip");
            }
        }

        public void StopAll() {
            cubeMarkerController.ResetAttached();
            InternalStopAll();
            Status = STATUS.IDLE;
        }

        private void InternalStopAll() {
            foreach (AnimationTake take in SceneController.GetCurrentScene().Takes) {
                take.Animation.Stop("clip");
            }
        }

        public void RewindAll() {
            StopAll();
            Status = STATUS.IDLE;
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
            CurrentTime = 0.0f;
            NotifyAnimationTimesChanged();
        }

        public void AddListener(AnimationControllerListener listener) {
            listeners.AddLast(listener);
        }

        public void NotifyTakeAdded(int take) {
            foreach (var listener in listeners) {
                listener.TakeAdded(take);
            }
        }

        public void NotifyTakeDeleted(int take) {
            foreach (var listener in listeners) {
                listener.TakeDeleted(take);
            }
        }

        public void NotifyStatusChanged() {
            foreach (var listener in listeners) {
                listener.StatusChanged(Status);
            }
        }

        public void NotifyAnimationTimesChanged() {
            foreach (var listener in listeners) {
                listener.AnimationTimesChanged(CurrentTime, EndTime, GetTakesTime());
            }
        }

        public float[] GetTakesTime() {
            var times = new float[SceneController.GetCurrentScene().Takes.Count];

            for (var i = 0; i < SceneController.GetCurrentScene().Takes.Count; i++) {
                times[i] = SceneController.GetCurrentScene().Takes[i].Clip.length;
            }

            return times;
        }

        public void CurrentSceneIsGoingToBeDeleted() {
        }

        public void CurrentSceneIsGoingToChange() {
            StopRecording();
            StopAll();
            RewindAll();
        }

        public void CurrentSceneChanged(Scene currentScene) {
            ResetController();
        }

        public void RemoveTake(int index) {
            SceneController.GetCurrentScene().Takes.RemoveAt(index);
            NotifyTakeDeleted(index);
            ResetController();
        }

        public void StopRecording() {
            if (status == STATUS.RECORDING) {
                Status = STATUS.IDLE;
                InternalStopAll();
                CreateNewTakeAtCurrentPos();
                this.recorderTracker.source = null;
                recorder.ResetRecording();
                cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.NORMAL);
                RewindAll();

                NotifyAnimationTimesChanged();
            }
        }

        public void PrepareForRecording(bool prepare) {
            if (!prepare) {
                Status = STATUS.IDLE;
                cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.NORMAL);
            } else {
                RewindAll();
                Status = STATUS.WAITING_OBJECT_TO_ATTACH;
                cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.RECORD_MODE);
            }
        }

        private void ResetController() {
            Status = STATUS.IDLE;
            CalculateClipTimes();
            NotifyAnimationTimesChanged();
        }

        public void ObjectAttached(MovableObject obj) {
            if (this.status == STATUS.WAITING_OBJECT_TO_ATTACH && obj.type == MovableObject.TYPE.SCENE_OBJECT) {
                this.recorderTracker.source = obj.transform;
                Status = STATUS.RECORDING;
                InternalPlayAll();
            }
        }

        public void ObjectDetached(MovableObject obj) {
            // Não deveria chegar na situação abaixo, coloco aqui somente por segurança
            if (status == STATUS.RECORDING) {
                Debug.Log("ObjectDetached durante gravação????");
                StopRecording();
            }
        }

        public void MarkerLost() {
            StopRecording();
        }

    }
}
