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
        public AnimationUIController UIController;

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
                UIController.SetRecorderStatus(status);
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
                    recorder.TakeSnapshot(Time.deltaTime);
                    UIController.SetTime(recorder.currentTime, EndTime, GetTakesTime());
                    break;
                case STATUS.PLAYING:
                    currentTime = longestTake.Animation["clip"].time;
                    UIController.SetTime(CurrentTime, EndTime, GetTakesTime());
                    if (!longestTake.Animation.isPlaying) {
                        Status = STATUS.IDLE;
                    }
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
            clip.legacy = true;
            recorder.SaveToClip(clip);

            animation.playAutomatically = false;
            animation.AddClip(clip, "clip");

            var newTake = new AnimationTake(animation, clip, recorder.source);

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
            foreach (var take in SceneController.GetCurrentScene().Takes) {
                if (endTime < take.Clip.length) {
                    endTime = take.Clip.length;
                    longestTake = take;
                }
            }

            currentTime = 0.0f;
        }

        public void PlayAll() {
            // TODO: usar listener animation controller e dai o próprio cubeMarkerControler toma as ações próprias
            cubeMarkerController.ResetAttached();
            foreach (AnimationTake take in SceneController.GetCurrentScene().Takes) {
                take.Animation.Play("clip");
            }
            if (SceneController.GetCurrentScene().Takes.Count > 0) {
                Status = STATUS.PLAYING;
            }
        }

        public void StopAll() {
            cubeMarkerController.ResetAttached();
            foreach (AnimationTake take in SceneController.GetCurrentScene().Takes) {
                take.Animation.Stop("clip");
            }
            Status = STATUS.IDLE;
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
            currentTime = 0.0f;
            UIController.SetTime(CurrentTime, EndTime, GetTakesTime());
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

        public float[] GetTakesTime() {
            var times = new float[SceneController.GetCurrentScene().Takes.Count];

            for (var i = 0; i < SceneController.GetCurrentScene().Takes.Count; i++) {
                times[i] = SceneController.GetCurrentScene().Takes[i].Clip.length;
            }

            return times;
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
                CreateNewTakeAtCurrentPos();
                recorder.ResetRecording();
                cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.NORMAL);

                // Mudar para função mais generica
                UIController.SetTime(CurrentTime, EndTime, GetTakesTime());
            }
        }

        public void PrepareForRecording(bool prepare) {
            if (prepare) {
                Status = STATUS.IDLE;
                cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.NORMAL);
            } else {
                Status = STATUS.WAITING_OBJECT_TO_ATTACH;
                cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.RECORD_MODE);
            }
        }

        private void ResetController() {
            Status = STATUS.IDLE;
            CalculateClipTimes();
            UIController.SetTime(CurrentTime, EndTime, GetTakesTime());
        }

        public void ObjectAttached(MovableObject obj) {
            if (this.status == STATUS.WAITING_OBJECT_TO_ATTACH && obj.type == MovableObject.TYPE.SCENE_OBJECT) {
                this.recorderTracker.source = obj.transform;
                Status = STATUS.RECORDING;
            }
            UIController.SetRecorderStatus(status);
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
