using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;
using UnityEditor.Experimental.Animations;

namespace Assets.Scripts {
    public class RecorderController : MonoBehaviour, IVirtualButtonEventHandler, CubeMarkerListener {

        public AnimationClip clip;
        public GameObject RecButton;
        public CubeMarkerController cubeMarkerController;

        private GameObjectRecorder recorder = new GameObjectRecorder();
        private GameObject gameObjectToRecord;
        private bool record = false;

        void Start() {
            RecButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            cubeMarkerController.AddListener(this);
        }

        void LateUpdate() {
            if (clip == null)
                return;

            if (record && gameObjectToRecord) {
                recorder.TakeSnapshot(Time.deltaTime);
            } else if (recorder.isRecording) {
                recorder.SaveToClip(clip);
                recorder.ResetRecording();
            }
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb) {
            record = true;
        }

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
            record = false;
        }

        public void ObjectAttached(GameObject obj) {
            this.gameObjectToRecord = obj;
            this.recorder.root = obj;
            this.recorder.BindComponent<Transform>(obj, true);
        }

        public void ObjectDetached(GameObject obj) {
            this.gameObjectToRecord = null;
        }
    }
}
