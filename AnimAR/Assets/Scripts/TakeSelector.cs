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
        public GameObject TakePrefab;

        private GameObject currentObject = null;
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
            if (currentObject) {
                currentObject.SetActive(false);
            }
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
            if (currentObject) {
                Destroy(currentObject);
            }
            NewCurrentObject(CurrentTake);
        }

        public override bool CanReceiveObject(MovableObject obj) {
            return obj.type == MovableObject.TYPE.TAKE_OBJECT;
        }

        public override void ObjectReceived(MovableObject obj) {
            Destroy(obj.gameObject);
            ChangeTakeIcon();
        }

        public override void ObjectRemoved(MovableObject obj) {
            currentObject = null;
        }

        public override string GetLabel() {
            return "Selecionar Take Atual";
        }

        public void NewCurrentObject(int index) {
            currentObject = GameObject.Instantiate(TakePrefab, showingObjectRoot);
            currentObject.transform.localPosition = new Vector3(0, 0, 0);
            currentObject.GetComponent<MovableObject>().type = MovableObject.TYPE.TAKE_OBJECT;
            currentObject.GetComponent<NumberIcon>().SetLabel(index < 0 ? "X" : index.ToString());
            currentObject.GetComponent<NumberIcon>().Number = index;
            currentObject.GetComponent<Collider>().enabled = index > -1;
            currentObject.SetActive(isActive);
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
