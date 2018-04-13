using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

namespace Assets.Scripts {
    public class RecorderUIController : MonoBehaviour {

        public TimelineUI Timeline;
        public Text TimeText;
        public Text InformationText;

        public void SetRecorderStatus(RecorderStatus status, int currentTake) {
            switch (status) {
                case RecorderStatus.IDLE:
                    InformationText.text = "Take " + currentTake;
                    break;
                case RecorderStatus.PLAYING:
                    InformationText.text = "Executando Animação";
                    break;
                case RecorderStatus.RECORDING:
                    InformationText.text = "Gravando Take " + currentTake;
                    break;
                case RecorderStatus.WAITING_OBJECT_TO_ATTACH:
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
