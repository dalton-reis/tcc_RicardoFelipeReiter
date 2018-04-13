using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class TakeSelector : Selector, AnimationControllerListener {

        public AnimationController AnimationController;
        public GameObject TakePrefab;

        private GameObject currentObject = null;
        private bool isActive = false;

        void Start() {
            AnimationController.AddListener(this);
            UpdateCurrentTake(0);
        }

        public override void Next() {
            UpdateCurrentTake(AnimationController.CurrentTake + 1);
        }

        public override void Prev() {
            UpdateCurrentTake(AnimationController.CurrentTake - 1);
        }

        public override void Active() {
            if (currentObject) {
                currentObject.SetActive(true);
            }
            isActive = true;
        }

        public override void Desactive() {
            if (currentObject) {
                currentObject.SetActive(false);
            }
            isActive = false;
        }

        private void UpdateCurrentTake(int takeIndex) {
            AnimationController.CurrentTake = Math.Abs(takeIndex % (AnimationController.takes.Count() + 1));
        }

        private void ChangeTakeIcon(int index) {
            if (currentObject) {
                Destroy(currentObject);
            }
            NewCurrentObject(AnimationController.CurrentTake);
        }

        public override bool ObjectReceived(GameObject obj) {
            return false;
        }

        public override void ObjectRemoved(GameObject obj) {

        }

        public override string GetLabel() {
            return "Selecionar Take Atual";
        }

        public void NewCurrentObject(int index) {
            var label = index == AnimationController.takes.Count() ? index.ToString() + '+' : index.ToString();

            currentObject = GameObject.Instantiate(TakePrefab, showingObjectRoot);
            currentObject.transform.localPosition = new Vector3(0, 0, 0);
            currentObject.GetComponent<TakeIcon>().SetLabel(label);
            currentObject.SetActive(isActive);
        }

        public void CurrentTakeChanged(int take) {
            ChangeTakeIcon(take);
        }

    }
}
