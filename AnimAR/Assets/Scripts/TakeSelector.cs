using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class TakeSelector : Selector, AnimationControllerListener {

        public AnimationController AnimationController;
        public SceneController SceneController;
        public GameObject TakePrefab;

        private GameObject currentObject = null;
        private bool isActive = false;

        void Start() {
            AnimationController.AddListener(this);
        }

        public override void Next() {
            UpdateCurrentTake(AnimationController.CurrentTake + 1);
        }

        public override void Prev() {
            UpdateCurrentTake(AnimationController.CurrentTake - 1);
        }

        public override void Active() {
            //if (currentObject) {
            //    currentObject.SetActive(true);
            //}
            isActive = true;
            ChangeTakeIcon(AnimationController.CurrentTake);
        }

        public override void Desactive() {
            if (currentObject) {
                currentObject.SetActive(false);
            }
            isActive = false;
        }

        private void UpdateCurrentTake(int takeIndex) {
            if (SceneController.GetCurrentScene().Takes.Count() > 0) {
                AnimationController.CurrentTake = Math.Abs(takeIndex % SceneController.GetCurrentScene().Takes.Count());
            }
        }

        private void ChangeTakeIcon(int index) {
            if (currentObject) {
                Destroy(currentObject);
            }
            NewCurrentObject(AnimationController.CurrentTake);
        }

        public override bool CanReceiveObject(MovableObject obj) {
            return obj.type == MovableObject.TYPE.TAKE_OBJECT;
        }

        public override void ObjectReceived(MovableObject obj) {
            Destroy(obj);
            ChangeTakeIcon(AnimationController.CurrentTake);
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
            Debug.Log("Lol1");
            Debug.Log(isActive);
        }

        public void CurrentTakeChanged(int take) {
            ChangeTakeIcon(take);
        }

    }
}
