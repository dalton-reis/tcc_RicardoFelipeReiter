using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace Assets.Scripts {
    public class SelectorController : MonoBehaviour, IVirtualButtonEventHandler, CubeMarkerInteractor, ITrackableEventHandler {

        public GameObject NextButton;
        public GameObject PrevButton;
        public GameObject ChangeSelectorButton;
        public Text SelectorLabel;
        public Selector[] Selectors;
        public CubeMarkerController CubeMarkerController;

        private int currentSelectorIndex = 0;
        private Selector currentSelector;

        void Start() {
            NextButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            PrevButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            ChangeSelectorButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
            GetComponent<ImageTargetBehaviour>().RegisterTrackableEventHandler(this);

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
            CubeMarkerController.ResetAttached();
            if (currentSelector) {
                currentSelector.Desactive();
            }
            currentSelector = Selectors[currentSelectorIndex];
            currentSelector.Active();
            SelectorLabel.text = currentSelector.GetLabel();
        }

        public bool CanReceiveObject(MovableObject obj) {
            return currentSelector.CanReceiveObject(obj);
        }

        public void ObjectReceived(MovableObject obj) {
            currentSelector.ObjectReceived(obj);
        }

        public void ObjectRemoved(MovableObject obj) {
            currentSelector.ObjectRemoved(obj);
        }

        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
            if (currentSelector) {
                if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED) {
                    currentSelector.Active();
                } else {
                    currentSelector.Desactive();
                }
            }
        }
    }
}
