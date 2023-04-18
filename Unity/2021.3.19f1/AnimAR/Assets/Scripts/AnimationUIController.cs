using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Globalization;

namespace Assets.Scripts {
    public class AnimationUIController : MonoBehaviour, VirtualButtonListener, AnimationControllerListener {
   
        public TimelineUI Timeline;
        public Text TimeText;
        public Text InformationText;
        public AnimationController animationController;

        void Start() {
            GetComponent<VirtualButtonHandler>().AddListener(this);
            animationController.AddListener(this);
        }

        private void SetRecorderStatus(AnimationController.STATUS status) {
            switch (status) {
                case AnimationController.STATUS.IDLE:
                    InformationText.text = "IDLE";
                    break;
                case AnimationController.STATUS.PLAYING:
                    InformationText.text = "Executando Animação";
                    break;
                case AnimationController.STATUS.RECORDING:
                    InformationText.text = "Gravando Animação ";
                    break;
                case AnimationController.STATUS.WAITING_OBJECT_TO_ATTACH:
                    InformationText.text = "Selecione um objeto que a gravação irá começar";
                    break;
            }
        }

        private void SetTime(float currentTime, float endTime, float[] takesTime) {
            var endString = endTime.ToString("F", CultureInfo.InvariantCulture);
            if (endTime <= 0 || currentTime > endTime) {
                endString = "-:--";
                endTime = currentTime;
            }

            var currentString = currentTime.ToString("F", CultureInfo.InvariantCulture);
            TimeText.text = currentString + "/" + endString;
            Timeline.SetTime(currentTime, endTime, takesTime);
        }


        public void TakeAdded(int take) {
        }

        public void TakeDeleted(int take) {
        }

        public void StatusChanged(AnimationController.STATUS status) {
            SetRecorderStatus(status);
        }

        public void AnimationTimesChanged(float currentTime, float endTime, float[] takesTime) {
            SetTime(currentTime, endTime, takesTime);
        }

        public void ButtonPressed(VirtualButtonBehaviour vb) {
            switch (vb.VirtualButtonName) {
                case "Record":
                    switch (animationController.Status) {
                        case AnimationController.STATUS.RECORDING:
                            animationController.StopRecording();
                            break;
                        case AnimationController.STATUS.WAITING_OBJECT_TO_ATTACH:
                            animationController.PrepareForRecording(false);
                            break;
                        case AnimationController.STATUS.IDLE:
                            animationController.PrepareForRecording(true);
                            break;
                    }
                    break;
                case "Play":
                    switch (animationController.Status) {
                        case AnimationController.STATUS.PLAYING:
                            animationController.StopAll();
                            break;
                        case AnimationController.STATUS.IDLE:
                            animationController.PlayAll();
                            break;
                    }
                    break;
                case "Rewind":
                    if (animationController.Status == AnimationController.STATUS.IDLE || animationController.Status == AnimationController.STATUS.PLAYING) {
                        animationController.RewindAll();
                    }
                    break;
            }
        }
    }
}
