using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class ObjectFactorySelector : Selector {

        public GameObject[] objList;

        private int currentObjIndex = 0;
        private GameObject currentObject = null;

        void Start() {
            ChangeIndex(0);
        }

        public override void Next() {
            ChangeIndex(currentObjIndex + 1);
        }

        public override void Prev() {
            ChangeIndex(currentObjIndex - 1);
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
            index = index < 0 ? objList.Length + index : index;
            currentObjIndex = Math.Abs(index % objList.Length);
            if (currentObject) {
                Destroy(currentObject);
            }
            NewCurrentObject(objList[currentObjIndex]);
        }

        public override bool CanReceiveObject(MovableObject obj) {
            return false;
        }

        public override void ObjectReceived(MovableObject obj) {

        }

        public override void ObjectRemoved(MovableObject obj) {
            if (currentObject == obj.gameObject) {
                NewCurrentObject(obj.gameObject);
            }
        }

        public override string GetLabel() {
            return "Fábrica de Objetos";
        }

        public void NewCurrentObject(GameObject objToCopy) {
            currentObject = GameObject.Instantiate(objToCopy, showingObjectRoot);
            currentObject.transform.localPosition = new Vector3(0, 0, 0);
        }

    }
}
