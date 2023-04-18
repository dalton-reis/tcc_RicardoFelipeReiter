using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class TakeSelector : Selector, SceneControllerListener, AnimationControllerListener {

        public AnimationController AnimationController;
        public SceneController SceneController;
        public TakeIcon TakePrefab;

        private TakeIcon currentObject = null;
        private bool isActive = false;
        private int currentTake = -1;

        private int CurrentTake {
            get {
                return currentTake;
            }
            set {
                currentTake = value;
                ChangeTakeIcon();
            }
        }

        void Start() {
            SceneController.AddListener(this);
            AnimationController.AddListener(this);
            CurrentSceneChanged(SceneController.GetCurrentScene());
        }

        public override void Next() {
            UpdateCurrentTake(CurrentTake + 1);
        }

        public override void Prev() {
            UpdateCurrentTake(CurrentTake - 1);
        }

        public override void Active() {
            isActive = true;
            ChangeTakeIcon();
        }

        public override void Desactive() {
            DestroyCurrentObject();
            isActive = false;
        }

        private void UpdateCurrentTake(int takeIndex) {
            var takesCount = SceneController.GetCurrentScene().Takes.Count();
            if (takesCount > 0) {
                takeIndex = takeIndex < 0 ? takesCount + takeIndex : takeIndex;
                CurrentTake = Math.Abs(takeIndex % takesCount);
            }
        }

        private void ChangeTakeIcon() {
            DestroyCurrentObject();
            NewCurrentObject(CurrentTake);
        }

        private void DestroyCurrentObject() {
            if (currentObject) {
                var takeObject = currentObject.Take;
                if (takeObject != null) {
                    var outliner = takeObject.GameObject.GetComponent<ObjectOutliner>();
                    if (outliner) {
                        outliner.SetEnabled(false);
                    }
                }
                Destroy(currentObject.gameObject);
            }
        }

        public override bool CanReceiveObject(MovableObject obj) {
            return obj.type == MovableObject.TYPE.TAKE_OBJECT;
        }

        public override bool ObjectReceived(MovableObject obj) {
            Destroy(obj.gameObject);
            ChangeTakeIcon();
            return true;
        }

        public override void ObjectRemoved(MovableObject obj) {
            if (currentObject) {
                var takeObject = currentObject.Take;
                if (takeObject != null) {
                    var outliner = takeObject.GameObject.GetComponent<ObjectOutliner>();
                    if (outliner) {
                        outliner.SetEnabled(false);
                    }
                }
            }
            currentObject = null;
        }

        public override string GetLabel() {
            return "Selecionar Animação";
        }

        public void NewCurrentObject(int index) {
            currentObject = GameObject.Instantiate(TakePrefab, showingObjectRoot);
            currentObject.transform.localPosition = new Vector3(0, 0, 0);
            currentObject.SetLabel(index < 0 ? "X" : index.ToString());
            currentObject.Number = index;
            currentObject.GetComponent<Collider>().enabled = index > -1;
            currentObject.gameObject.SetActive(isActive);

            if (isActive && index > -1) {
                currentObject.Take = SceneController.GetCurrentScene().Takes[index];
                var outliner = currentObject.Take.GameObject.GetComponent<ObjectOutliner>();
                if (outliner) {
                    outliner.SetColor(Color.cyan);
                    outliner.SetEnabled(true);
                }
            }
        }

        public void CurrentSceneIsGoingToBeDeleted() {
        }

        public void CurrentSceneIsGoingToChange() {
        }

        public void CurrentSceneChanged(Scene currentScene) {
            if (currentScene.Takes.Count() > 0) {
                CurrentTake = 0;
            } else {
                CurrentTake = -1;
            }
        }

        public void TakeDeleted(int take) {
            CurrentSceneChanged(SceneController.GetCurrentScene());
        }

        public void TakeAdded(int take) {
            CurrentSceneChanged(SceneController.GetCurrentScene());
        }


        public void StatusChanged(AnimationController.STATUS status) {
        }

        public void AnimationTimesChanged(float currentTime, float endTime, float[] takesTime) {
        }
    }
}
