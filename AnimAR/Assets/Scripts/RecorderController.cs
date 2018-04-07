using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using Vuforia;
using UnityEditor.Experimental.Animations;

namespace Assets.Scripts {
    public class RecorderController : MonoBehaviour, IVirtualButtonEventHandler, CubeMarkerListener {

        public GameObject RecButton;
        public GameObject PlayButton;
        public GameObject RewindButton;
        public CubeMarkerController cubeMarkerController;
        public RecorderTracker recorderTracker;
        public AnimationController animationController;

        private GameObjectRecorder recorder;
        private GameObject gameObjectToRecord;
        private bool record = false;
        private bool waitingAttachedObject = false;

        void Start() {
            recorder = new GameObjectRecorder();
            recorder.root = recorderTracker.gameObject;
            recorder.BindComponent<Transform>(recorderTracker.gameObject, true);

            RecButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            PlayButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            RewindButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            cubeMarkerController.AddListener(this);
        }

        void LateUpdate() {
            if (record && gameObjectToRecord) {
                Debug.Log("Snapshot");
                recorder.TakeSnapshot(Time.deltaTime);
            }
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb) {
            switch (vb.VirtualButtonName) {
                case "Record":
                    if (record) {
                        record = false;
                        cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.NORMAL);
                        animationController.StopRecording(recorder);
                        recorder.ResetRecording();
                        recorder = new GameObjectRecorder();
                        recorder.root = recorderTracker.gameObject;
                        recorder.BindComponent<Transform>(recorderTracker.gameObject, true);
                        cubeMarkerController.ResetAttached();
                    } else {
                        if (waitingAttachedObject) {
                            waitingAttachedObject = false;
                            cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.NORMAL);
                            cubeMarkerController.ResetAttached();
                        } else {
                            cubeMarkerController.SetAttachMode(CubeMarkerAttachMode.RECORD_MODE);
                            cubeMarkerController.ResetAttached();
                            waitingAttachedObject = true;
                        }
                    }
                    break;
                case "Play":
                    cubeMarkerController.ResetAttached();
                    if (animationController.isPlaying) {
                        animationController.StopAll();
                    } else {
                        animationController.PlayAll();
                    }
                    break;
                case "Rewind":
                    cubeMarkerController.ResetAttached();
                    animationController.RewindAll();
                    break;
            }
        }

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
        }

        public void ObjectAttached(GameObject obj) {
            if (this.waitingAttachedObject) {
                this.gameObjectToRecord = obj;
                this.recorderTracker.source = obj.transform;
                this.animationController.StartRecording(obj);
                this.waitingAttachedObject = false;
                this.record = true;
            }
        }

        public void ObjectDetached(GameObject obj) {
            this.gameObjectToRecord = null;
        }
    }
}
