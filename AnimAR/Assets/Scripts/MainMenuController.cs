using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class MainMenuController : MonoBehaviour {

        public Toggle CardboardToggle;

        void Start() {
            var vuforiaType = PlayerPrefs.GetString("vuforiaType");
            CardboardToggle.isOn = vuforiaType.Equals("vr");
        }

        public void LoadVuforiaScene() {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public void CardboardToggleChanged(Boolean value) {
            if (CardboardToggle.isOn) {
                PlayerPrefs.SetString("vuforiaType", "vr");
            } else {
                PlayerPrefs.SetString("vuforiaType", "non-vr");
            }
        }
        public void StartWithoutVR() {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public void ShowTutorial() {

        }

        public void ShowAbout() {

        }

        public void CleanSavedData() {

        }

        public void Exit() {
            Application.Quit();
        }

    }
}
