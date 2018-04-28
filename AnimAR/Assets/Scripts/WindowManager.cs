using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class WindowManager : MonoBehaviour {

        public static WindowManager Instance;
        public Window DefaultWindow;

        private List<Window> windows = new List<Window>();
        private Window currentOpenWindow = null;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }
        }

        void Start() {
            InternalOpenWindow(DefaultWindow);
        }

        public void RegisterWindow(Window window) {
            windows.Add(window);
        }

        public void OpenWindow(string name) {
            Window window = windows.Find(w => w.Name.Equals(name));
            if (!window) {
                throw new UnityException("Could not find a window with name: " + name);
            }
            InternalOpenWindow(window);
        }

        public void CloseCurrent() {
            InternalOpenWindow(DefaultWindow);
        }

        private void InternalOpenWindow(Window window) {
            if (currentOpenWindow && !window.isDialog) {
                currentOpenWindow.gameObject.SetActive(false);
            }
            window.gameObject.SetActive(true);
            currentOpenWindow = window;
        }

    }
}
