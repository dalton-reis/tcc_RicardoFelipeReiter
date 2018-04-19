using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class RecorderController : MonoBehaviour, IVirtualButtonEventHandler, CubeMarkerListener {

        public GameObject RecButton;
        public GameObject PlayButton;
        public GameObject RewindButton;
        public CubeMarkerController cubeMarkerController;
        public RecorderTracker recorderTracker;
        public AnimationController animationController;
        public RecorderUIController recorderUIController;

        private GORecorder recorder;
        private GameObject gameObjectToRecord;
        private RecorderStatus status;

        // TODO: Mover recorder para AnimationController.

        void Start() {
            recorder = new GORecorder();
            recorder.source = recorderTracker.gameObject;

            RecButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            PlayButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            RewindButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            cubeMarkerController.AddListener(this);
        }

        void LateUpdate() {
            switch (status) {
                case RecorderStatus.RECORDING:
                    recorder.TakeSnapshot(Time.deltaTime);
                    recorderUIController.SetTime(recorder.currentTime, animationController.EndTime, animationController.GetTakesTime());
                    break;
                case RecorderStatus.PLAYING:
                    if (animationController.isPlaying) {
                        recorderUIController.SetTime(animationController.CurrentTime, animationController.EndTime, animationController.GetTakesTime());
                    } else {
                        recorderUIController.SetTime(0, animationController.EndTime, animationController.GetTakesTime());
                        SetStatus(RecorderStatus.IDLE);
                    }
                    break;
            }
        }

        public void StopRecording() {
            if (status == RecorderStatus.RECORDING) {
                SetStatus(RecorderStatus.IDLE);
                cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.NORMAL);
                animationController.CreateNewTakeAtCurrentPos(recorder, this.gameObjectToRecord);
                recorder.ResetRecording();
                cubeMarkerController.ResetAttached();
            }
        }

        private void SetStatus(RecorderStatus status) {
            this.status = status;
            recorderUIController.SetRecorderStatus(status, animationController.CurrentTake);
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb) {
            switch (vb.VirtualButtonName) {
                case "Record":
                    switch (status) {
                        case RecorderStatus.RECORDING:
                            StopRecording();
                            recorderUIController.SetTime(animationController.CurrentTime, animationController.EndTime, animationController.GetTakesTime());
                            break;
                        case RecorderStatus.WAITING_OBJECT_TO_ATTACH:
                            SetStatus(RecorderStatus.IDLE);
                            cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.NORMAL);
                            cubeMarkerController.ResetAttached();
                            break;
                        case RecorderStatus.IDLE:
                            SetStatus(RecorderStatus.WAITING_OBJECT_TO_ATTACH);
                            cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.RECORD_MODE);
                            cubeMarkerController.ResetAttached();
                            break;
                    }
                    break;
                case "Play":
                    switch (status) {
                        case RecorderStatus.PLAYING:
                            SetStatus(RecorderStatus.IDLE);
                            animationController.StopAll();
                            break;
                        case RecorderStatus.IDLE:
                            SetStatus(RecorderStatus.PLAYING);
                            cubeMarkerController.ResetAttached();
                            animationController.PlayAll();
                            break;
                    }
                    break;
                case "Rewind":
                    if (status == RecorderStatus.IDLE || status == RecorderStatus.PLAYING) {
                        SetStatus(RecorderStatus.IDLE);
                        cubeMarkerController.ResetAttached();
                        animationController.StopAll();
                        animationController.RewindAll();
                        recorderUIController.SetTime(animationController.CurrentTime, animationController.EndTime, animationController.GetTakesTime());
                    }
                    break;
            }
        }

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
        }

        public void ObjectAttached(GameObject obj) {
            if (this.status == RecorderStatus.WAITING_OBJECT_TO_ATTACH) {
                this.gameObjectToRecord = obj;
                this.recorderTracker.source = obj.transform;
                this.animationController.StopAll();
                this.animationController.RewindAll();
                SetStatus(RecorderStatus.RECORDING);
            }
            recorderUIController.SetRecorderStatus(status, animationController.CurrentTake);
        }

        public void ObjectDetached(GameObject obj) {
            this.gameObjectToRecord = null;
            // Não deveria chegar na situação abaixo, coloco aqui somente por segurança
            if (status == RecorderStatus.RECORDING) {
                Debug.Log("ObjectDetached durante gravação????");
                StopRecording();
            }
        }
    }
}
