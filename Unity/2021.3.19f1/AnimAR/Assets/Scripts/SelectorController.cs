using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace Assets.Scripts {
    public class SelectorController : CubeMarkerInteractorImpl//, ITrackableEventHandler
    , VirtualButtonListener {

        public Text SelectorLabel;
        public Selector[] Selectors;
        public CubeMarkerController CubeMarkerController;

        private int currentSelectorIndex = 0;
        private Selector currentSelector;

        void Start() {
            //GetComponent<ImageTargetBehaviour>().RegisterTrackableEventHandler(this);
            GetComponent<VirtualButtonHandler>().AddListener(this);

            foreach (Selector selector in Selectors) {
                selector.Desactive();
            }

            ChangeSelector(0);
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

        public override bool CanReceiveObject(MovableObject obj) {
            return currentSelector.CanReceiveObject(obj);
        }

        public override bool ObjectReceived(MovableObject obj) {
            return currentSelector.ObjectReceived(obj);
        }

        public override void ObjectRemoved(MovableObject obj) {
            currentSelector.ObjectRemoved(obj);
        }

        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
            if (currentSelector) {
                if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED) {
                    currentSelector.Active();
                } else {
                    currentSelector.Desactive();
                }
                PersistController.Instance.PersistEverything();
            }
        }

        public void ButtonPressed(VirtualButtonBehaviour vb) {
            switch (vb.VirtualButtonName) {
                case "Next":
                    currentSelector.Next();
                    CubeMarkerController.ResetAttached();
                    break;
                case "Prev":
                    currentSelector.Prev();
                    CubeMarkerController.ResetAttached();
                    break;
                case "ChangeSelector":
                    ChangeSelector(currentSelectorIndex + 1);
                    break;
            }
        }

    }
}
