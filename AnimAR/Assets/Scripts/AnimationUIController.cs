using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Globalization;

namespace Assets.Scripts {
    public class AnimationUIController : MonoBehaviour, IVirtualButtonEventHandler {

        public GameObject RecButton;
        public GameObject PlayButton;
        public GameObject RewindButton;
        
        public TimelineUI Timeline;
        public Text TimeText;
        public Text InformationText;
        public AnimationController animationController;

        void Start() {
            RecButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            PlayButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            RewindButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        }


        public void OnButtonPressed(VirtualButtonBehaviour vb) {
            switch (vb.VirtualButtonName) {
                case "Record":
                    switch (animationController.Status) {
                        case AnimationController.STATUS.RECORDING:
                            animationController.StopRecording();
                            break;
                        case AnimationController.STATUS.WAITING_OBJECT_TO_ATTACH:
                            animationController.PrepareForRecording(true);
                            break;
                        case AnimationController.STATUS.IDLE:
                            animationController.PrepareForRecording(false);
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

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
        }

        public void SetRecorderStatus(AnimationController.STATUS status) {
            switch (status) {
                case AnimationController.STATUS.IDLE:
                    InformationText.text = "IDLE";
                    break;
                case AnimationController.STATUS.PLAYING:
                    InformationText.text = "Executando Animação";
                    break;
                case AnimationController.STATUS.RECORDING:
                    InformationText.text = "Gravando Take ";
                    break;
                case AnimationController.STATUS.WAITING_OBJECT_TO_ATTACH:
                    InformationText.text = "Selecione um objeto que a gravação irá começar";
                    break;
            }
        }

        public void SetTime(float currentTime, float endTime, float[] takesTime) {
            var endString = endTime.ToString("F", CultureInfo.InvariantCulture);
            if (endTime <= 0 || currentTime > endTime) {
                endString = "-:--";
                endTime = currentTime;
            }

            var currentString = currentTime.ToString("F", CultureInfo.InvariantCulture);
            TimeText.text = currentString + "/" + endString;
            Timeline.SetTime(currentTime, endTime, takesTime);
        }

    }
}
