using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

namespace Assets.Scripts {
    public class ChangeVuforiaConfig : MonoBehaviour {

        void Start() {
            var vuforiaType = PlayerPrefs.GetString("vuforiaType");
            if (vuforiaType.Equals("vr")) {
                DigitalEyewearARController.Instance.SetEyewearType(DigitalEyewearARController.EyewearType.VideoSeeThrough);
                DigitalEyewearARController.Instance.SetStereoCameraConfiguration(DigitalEyewearARController.StereoFramework.Cardboard);
            } else {
                DigitalEyewearARController.Instance.SetEyewearType(DigitalEyewearARController.EyewearType.None);
            }
        }
    }
}
