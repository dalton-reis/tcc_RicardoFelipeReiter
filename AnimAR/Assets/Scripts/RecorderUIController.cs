using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

namespace Assets.Scripts {
    public class RecorderUIController : MonoBehaviour {

        public Slider TimeLineSlider;
        public Text TimeText;
        public Text InformationText;

        public void SetRecorderStatus(RecorderStatus status) {
            switch (status) {
                case RecorderStatus.IDLE:
                    InformationText.text = "";
                    break;
                case RecorderStatus.PLAYING:
                    InformationText.text = "Executando Animação";
                    break;
                case RecorderStatus.RECORDING:
                    InformationText.text = "Gravando...";
                    break;
                case RecorderStatus.WAITING_OBJECT_TO_ATTACH:
                    InformationText.text = "Selecione um objeto que a gravação irá começar";
                    break;
            }
        }

        public void SetTime(float currentTime, float endTime) {
            var endString = endTime.ToString("F", CultureInfo.InvariantCulture);
            if (endTime <= 0 || currentTime > endTime) {
                endString = "-:--";
                endTime = currentTime;
            }

            var currentString = currentTime.ToString("F", CultureInfo.InvariantCulture);
            TimeText.text = currentString + "/" + endString;
            TimeLineSlider.maxValue = endTime;
            TimeLineSlider.value = currentTime;
        }

    }
}
