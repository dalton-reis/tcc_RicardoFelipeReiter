using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class TakeSelector : Selector {

        public AnimationController AnimationController;
        public GameObject TakePrefab;

        private GameObject currentObject = null;

        void Start() {
            ChangeIndex(0);
        }

        public override void Next() {
            ChangeIndex(AnimationController.currentTake + 1);
        }

        public override void Prev() {
            ChangeIndex(AnimationController.currentTake - 1);
        }

        public override void Active() {
            if (currentObject) {
                currentObject.SetActive(true);
            }
        }

        public override void Desactive() {
            if (currentObject) {
                currentObject.SetActive(false);
            }
        }

        private void ChangeIndex(int index) {
            AnimationController.currentTake = Math.Abs(index % (AnimationController.takes.Count() + 1));
            if (currentObject) {
                Destroy(currentObject);
            }
            NewCurrentObject(AnimationController.currentTake);
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
        }

    }
}
