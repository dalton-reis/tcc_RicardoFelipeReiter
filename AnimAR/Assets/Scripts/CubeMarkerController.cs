using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class CubeMarkerController : MonoBehaviour {

        public CubeMarkerIndicatorController indicatorController;
        private const float secondsToHold = 2;
        private const float movementTolerance = 0.2f;

        private CubeMarkerInteractor interactor = null;
        private GameObject objectMarkerOver = null;
        private GameObject attachedObject = null;
        private CubeMarkerStatus currentStatus = CubeMarkerStatus.NOP;

        private float currentTime = 0;
        private Vector3 lastPos = Vector3.zero;

        void FixedUpdate() {
            if (currentStatus == CubeMarkerStatus.MARKER_OVER_OBJECT || currentStatus == CubeMarkerStatus.OBJECT_ATTACHED) {
                if (Math.Abs(Vector3.Distance(lastPos, transform.position)) > movementTolerance) {
                    currentTime = secondsToHold;
                } else {
                    currentTime -= Time.fixedDeltaTime;
                    if (currentTime <= 0) {
                        if (currentStatus == CubeMarkerStatus.MARKER_OVER_OBJECT) {
                            interactor.ObjectRemoved(objectMarkerOver);
                            objectMarkerOver.transform.parent = this.transform;
                            attachedObject = objectMarkerOver;
                            objectMarkerOver = null;
                        } else if (attachedObject && interactor != null) {
                            if (interactor.ObjectReceived(attachedObject)) {
                                attachedObject = null;
                            }
                        }
                        UpdateStatus();
                        currentTime = secondsToHold;
                    }
                }
            }
                lastPos = transform.position;
        }

        void OnTriggerEnter(Collider other) {
            switch (other.tag) {
                case "Movable":
                    if (currentStatus == CubeMarkerStatus.NOP) {
                        objectMarkerOver = other.gameObject;
                        currentTime = secondsToHold;
                    }
                    break;
                default:
                    var newInteractor = other.gameObject.GetComponent<CubeMarkerInteractor>();
                    if (newInteractor != null) {
                        interactor = newInteractor;
                    }
                    break;
            }
            UpdateStatus();
        }

        void OnTriggerExit(Collider other) {
            switch (other.tag) {
                case "Movable":
                    if (other.gameObject == objectMarkerOver) {
                        objectMarkerOver = null;
                    }
                    break;
                default:
                    if (interactor == other.gameObject.GetComponent<CubeMarkerInteractor>()) {
                        interactor = null;
                    }
                    break;
            }
            UpdateStatus();
        }

        private void UpdateStatus() {
            if (attachedObject) {
                currentStatus = CubeMarkerStatus.OBJECT_ATTACHED;
            } else if (objectMarkerOver) {
                currentStatus = CubeMarkerStatus.MARKER_OVER_OBJECT;
            } else {
                currentStatus = CubeMarkerStatus.NOP;
            }
            indicatorController.SetStatus(currentStatus);
        }

    }
}
