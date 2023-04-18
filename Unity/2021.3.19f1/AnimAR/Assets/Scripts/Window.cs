using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class Window : MonoBehaviour {

        public string Name;
        public bool isDialog;

        void Start() {
            WindowManager.Instance.RegisterWindow(this);
            gameObject.SetActive(false);
        }

    }
}
