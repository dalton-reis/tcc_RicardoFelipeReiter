using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class SelectorController : MonoBehaviour, IVirtualButtonEventHandler, CubeMarkerInteractor {

        public GameObject NextButton;
        public GameObject PrevButton;
        public GameObject ChangeSelectorButton;
        public Selector[] Selectors;

        private int currentSelectorIndex = 0;
        private Selector currentSelector;

        void Start() {
            NextButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            PrevButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            ChangeSelectorButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);

            foreach (Selector selector in Selectors) {
                selector.Desactive();
            }

            ChangeSelector(0);
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb) {
            switch (vb.VirtualButtonName) {
                case "Next":
                    currentSelector.Next();
                    break;
                case "Prev":
                    currentSelector.Prev();
                    break;
                case "ChangeSelector":
                    ChangeSelector(currentSelectorIndex + 1);
                    break;
            }
        }

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
        }

        private void ChangeSelector(int index) {
            currentSelectorIndex = Math.Abs(index % Selectors.Length);
            if (currentSelector) {
                currentSelector.Desactive();
            }
            currentSelector = Selectors[currentSelectorIndex];
            currentSelector.Active();
        }

        public bool ObjectReceived(GameObject obj) {
            return currentSelector.ObjectReceived(obj);
        }

        public void ObjectRemoved(GameObject obj) {
            currentSelector.ObjectRemoved(obj);
        }

    }
}
