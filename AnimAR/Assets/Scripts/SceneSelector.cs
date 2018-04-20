using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class SceneSelector : Selector {

        public SceneController SceneController;
        public GameObject SceneNumberPrefab;

        private GameObject currentObject = null;
        private bool isActive = false;

        void Start() {
            UpdateCurrentScene(0);
        }

        public override void Next() {
            UpdateCurrentScene(SceneController.CurrentScene + 1);
        }

        public override void Prev() {
            UpdateCurrentScene(SceneController.CurrentScene - 1);
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

        private void UpdateCurrentScene(int takeIndex) {
            if (takeIndex >= SceneController.scenes.Count()) {
                SceneController.AddNewScene();
            } else {
                SceneController.CurrentScene = Math.Abs(takeIndex % SceneController.scenes.Count());
            }
            ChangeSceneIcon(SceneController.CurrentScene);
        }

        private void ChangeSceneIcon(int index) {
            if (currentObject) {
                Destroy(currentObject);
            }
            NewCurrentObject(SceneController.CurrentScene);
        }


        public override bool CanReceiveObject(MovableObject obj) {
            return false;
        }

        public override void ObjectReceived(MovableObject obj) {
        }

        public override void ObjectRemoved(MovableObject obj) {
        }

        public override string GetLabel() {
            return "Selecionar Cena Atual";
        }

        public void NewCurrentObject(int index) {
            currentObject = GameObject.Instantiate(SceneNumberPrefab, showingObjectRoot);
            currentObject.transform.localPosition = new Vector3(0, 0, 0);
            currentObject.GetComponent<NumberIcon>().SetLabel(index.ToString());
            currentObject.SetActive(isActive);
        }

    }
}
