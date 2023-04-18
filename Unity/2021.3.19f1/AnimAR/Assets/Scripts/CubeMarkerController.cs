using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class CubeMarkerController : MonoBehaviour, SceneControllerListener//,DefaultTrackableEventHandler //ITrackableEventHandler 
    {

        public CubeMarkerIndicatorController indicatorController;
        public Transform indicatorDot;
        private const float secondsToHold = 2;
        private const float movementTolerance = 0.3f;

        private CubeMarkerInteractor interactor = null;
        private MovableObject objectMarkerOver = null;

        private MovableObject attachedObject = null;
        private Vector3 attachedObjectInitialPos = Vector3.zero;
        private Quaternion attachedObjectInitialRot = Quaternion.identity;

        private CubeMarkerStatus currentStatus = CubeMarkerStatus.NOP;
        private CubeMarkerAttachMode attachMode = CubeMarkerAttachMode.NORMAL;
        private LinkedList<CubeMarkerListener> listeners = new LinkedList<CubeMarkerListener>();

        private float currentTime = 0;
        private Vector3 lastPos = Vector3.zero;

        void Start() {
            GameObject.FindObjectOfType<SceneController>().AddListener(this);
            //GetComponent<MultiTargetBehaviour>().RegisterTrackableEventHandler(this);//erro
        }

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
                            objectMarkerOver.currentInteractor.ObjectRemoved(objectMarkerOver);
                            attachedObject = objectMarkerOver;
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
                            if (interactor.CanReceiveObject(attachedObject)) {
                                attachedObject.currentInteractor = interactor;
                                interactor.ObjectReceived(attachedObject);
                                NotifyObjectDetached(attachedObject);
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
            var movable = other.GetComponent<MovableObject>();
            var newInteractor = other.GetComponent<CubeMarkerInteractor>();

            if (movable != null) {
                if (currentStatus == CubeMarkerStatus.NOP) {
                    newInteractor = movable.currentInteractor;
                    objectMarkerOver = movable;
                    if (objectMarkerOver.outliner) {
                        objectMarkerOver.outliner.SetColor(Color.red);
                        objectMarkerOver.outliner.SetEnabled(true);
                    }
                    currentTime = secondsToHold;
                }
            }

            if (newInteractor != null && currentStatus != CubeMarkerStatus.MARKER_OVER_OBJECT) {
                if (interactor != null) interactor.OnCubeMarkerExit();
                interactor = newInteractor;
                interactor.OnCubeMarkerEnter();

            }
            UpdateStatus();
        }

        void OnTriggerExit(Collider other) {
            var movable = other.GetComponent<MovableObject>();
            var newInteractor = other.GetComponent<CubeMarkerInteractor>();

            if (movable != null) {
                if (movable == objectMarkerOver) {
                    if (objectMarkerOver.outliner) {
                        objectMarkerOver.outliner.SetEnabled(false);
                    }
                    objectMarkerOver = null;
                }
            } else if (newInteractor != null) {
                if (newInteractor == interactor) {
                    interactor.OnCubeMarkerExit();
                    interactor = null;
                }
            }
            UpdateStatus();
        }

        public void AddListener(CubeMarkerListener listener) {
            listeners.AddLast(listener);
        }

        public void SetAttachMode(CubeMarkerAttachMode attachMode) {
            this.attachMode = attachMode;
            ResetAttached();
        }

        public void ResetAttached() {
            // TODO: Normalmente chamado no SelectorController e no RecorderController, talvez criar um listener para ambos e esse daqui ser um observer
            if (attachedObject) {
                VuforiaUtils.EnableTargetObject(attachedObject.gameObject);
                if (!attachedObject.currentInteractor.ObjectReceived(attachedObject)) {
                    attachedObject.transform.localPosition = attachedObjectInitialPos;
                    attachedObject.transform.localRotation = attachedObjectInitialRot;
                    if (attachedObject.outliner) {
                        attachedObject.outliner.SetEnabled(false);
                    }
                }
                NotifyObjectDetached(attachedObject);
                attachedObject = null;
                UpdateStatus();
            }
        }

        private void NotifyObjectAttached(MovableObject obj) {
            foreach (CubeMarkerListener listener in listeners) {
                listener.ObjectAttached(obj);
            }
        }

        private void NotifyObjectDetached(MovableObject obj) {
            foreach (CubeMarkerListener listener in listeners) {
                listener.ObjectDetached(obj);
            }
        }

        private void NotifyCubeMarkerLost() {
            foreach (CubeMarkerListener listener in listeners) {
                listener.MarkerLost();
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


        public void CurrentSceneIsGoingToBeDeleted() {

        }

        public void CurrentSceneIsGoingToChange() {
            ResetAttached();
        }

        public void CurrentSceneChanged(Scene currentScene) {
        }

        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
            if (newStatus != TrackableBehaviour.Status.DETECTED && newStatus != TrackableBehaviour.Status.TRACKED) {
                if (interactor != null) {
                    interactor.OnCubeMarkerExit();
                    interactor = null;
                }
                NotifyCubeMarkerLost();
            }
        }
    }
}
