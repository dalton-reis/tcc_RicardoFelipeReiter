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

        public AnimationClip clip;
        public GameObject RecButton;
        public CubeMarkerController cubeMarkerController;
        public RecorderTracker recorderTracker;

        private GameObjectRecorder recorder;
        private GameObject gameObjectToRecord;
        private bool record = false;

        void Start() {
            recorder = new GameObjectRecorder();
            recorder.root = recorderTracker.gameObject;
            recorder.BindComponent<Transform>(recorderTracker.gameObject, true);

            clip = new AnimationClip();
            RecButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            cubeMarkerController.AddListener(this);
        }

        void LateUpdate() {
            if (clip == null)
                return;

            if (record && gameObjectToRecord) {
                Debug.Log("Snapshot?");
                recorder.TakeSnapshot(Time.deltaTime);
            } else if (recorder.isRecording) {
                recorder.SaveToClip(clip);
                recorder.ResetRecording();
            }
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb) {
            if (record) {
                record = false;
                AssetDatabase.CreateAsset(clip, "Assets/Test.anim");
            } else {
                record = true;
            }
        }

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
        }

        public void ObjectAttached(GameObject obj) {
            this.gameObjectToRecord = obj;
            recorderTracker.source = obj.transform;
        }

        public void ObjectDetached(GameObject obj) {
            this.gameObjectToRecord = null;
        }
    }
}
