using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class CubeMarkerController : MonoBehaviour {

        public CubeMarkerIndicatorController indicatorController;
        public Transform indicatorDot;
        private const float secondsToHold = 2;
        private const float movementTolerance = 0.2f;

        private CubeMarkerInteractor interactor = null;
        private GameObject objectMarkerOver = null;

        private GameObject attachedObject = null;
        private CubeMarkerInteractor attachedObjectInteractor = null;
        private Vector3 attachedObjectInitialPos = Vector3.zero;
        private Quaternion attachedObjectInitialRot = Quaternion.identity;

        private CubeMarkerStatus currentStatus = CubeMarkerStatus.NOP;
        private CubeMarkerAttachMode attachMode = CubeMarkerAttachMode.NORMAL;
        private LinkedList<CubeMarkerListener> listeners = new LinkedList<CubeMarkerListener>();

        private float currentTime = 0;
        private Vector3 lastPos = Vector3.zero;

        void Update() {
            if (attachedObject && attachMode == CubeMarkerAttachMode.RECORD_MODE) {
                attachedObject.transform.position = indicatorDot.position;
                attachedObject.transform.rotation = indicatorDot.rotation;
            }
        }

        void FixedUpdate() {
            if (currentStatus == CubeMarkerStatus.MARKER_OVER_OBJECT || currentStatus == CubeMarkerStatus.OBJECT_ATTACHED) {
                if (Math.Abs(Vector3.Distance(lastPos, transform.position)) > movementTolerance) {
                    currentTime = secondsToHold;
                } else {
                    currentTime -= Time.fixedDeltaTime;
                    if (currentTime <= 0) {
                        if (currentStatus == CubeMarkerStatus.MARKER_OVER_OBJECT) {
                            interactor.ObjectRemoved(objectMarkerOver);
                            attachedObject = objectMarkerOver;
                            attachedObjectInteractor = interactor;
                            attachedObjectInitialPos = attachedObject.transform.localPosition;
                            attachedObjectInitialRot = attachedObject.transform.localRotation;
                            if (attachMode == CubeMarkerAttachMode.RECORD_MODE) {
                                indicatorDot.position = attachedObject.transform.position;
                                indicatorDot.rotation = attachedObject.transform.rotation;
                            } else {
                                attachedObject.transform.parent = this.transform;
                            }
                            objectMarkerOver = null;
                            NotifyObjectAttached(attachedObject);
                        } else if (attachedObject && interactor != null && attachMode == CubeMarkerAttachMode.NORMAL) {
                            if (interactor.ObjectReceived(attachedObject)) {
                                attachedObject = null;
                                NotifyObjectDetached(attachedObject);
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

        public void AddListener(CubeMarkerListener listener) {
            listeners.AddLast(listener);
        }

        public void SetAttachMode(CubeMarkerAttachMode attachMode) {
            this.attachMode = attachMode;
        }

        public void ResetAttached() {
            if (attachedObject) {
                attachedObjectInteractor.ObjectReceived(attachedObject);
                attachedObject.transform.localPosition = attachedObjectInitialPos;
                attachedObject.transform.localRotation = attachedObjectInitialRot;
                NotifyObjectDetached(attachedObject);
                attachedObject = null;
                attachedObjectInteractor = null;
                UpdateStatus();
            }
        }

        private void NotifyObjectAttached(GameObject obj) {
            foreach (CubeMarkerListener listener in listeners) {
                listener.ObjectAttached(obj);
            }
        }

        private void NotifyObjectDetached(GameObject obj) {
            foreach (CubeMarkerListener listener in listeners) {
                listener.ObjectDetached(obj);
            }
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
