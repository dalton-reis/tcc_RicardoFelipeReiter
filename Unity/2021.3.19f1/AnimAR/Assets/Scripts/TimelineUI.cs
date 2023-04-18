using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

namespace Assets.Scripts {
    public class TimelineUI : MonoBehaviour {

        public Slider Slider;
        public RectTransform FillArea;
        public GameObject TakeMarkPrefab;

        private List<GameObject> TakeMarkList = new List<GameObject>();

        public void SetTime(float currentTime, float endTime, float[] takesTime) {
            Slider.maxValue = endTime;
            Slider.value = currentTime;

            var fillAreaWidth = FillArea.rect.width;
            
            for (var i = 0; i < takesTime.Length; i++) {
                GameObject mark = null;

                if (i < TakeMarkList.Count) {
                    mark = TakeMarkList[i];
                } else {
                    mark = GameObject.Instantiate(TakeMarkPrefab, FillArea);
                    TakeMarkList.Add(mark);
                }

                mark.SetActive(FillArea.gameObject.activeInHierarchy);
                var markRectTransform = mark.GetComponent<RectTransform>();
                var markText = mark.GetComponentInChildren<Text>();

                markRectTransform.anchoredPosition = new Vector2((takesTime[i] * fillAreaWidth) / endTime, 0.0f);
                markText.text = "Take " + (i + 1);
            }
            for (var i = TakeMarkList.Count() - 1; i >= takesTime.Length; i--) {
                Destroy(TakeMarkList[i]);
                TakeMarkList.RemoveAt(i);
            }
        }

    }
}
