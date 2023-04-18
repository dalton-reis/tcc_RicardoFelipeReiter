using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class SceneSelector : Selector, SceneControllerListener {

        public SceneController SceneController;
        public GameObject SceneNumberPrefab;

        private GameObject currentObject = null;
        private bool isActive = false;

        void Start() {
            SceneController.AddListener(this);
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

        private void UpdateCurrentScene(int sceneIndex) {
            if (sceneIndex >= SceneController.scenes.Count()) {
                SceneController.AddNewScene();
            } else {
                sceneIndex = sceneIndex < 0 ? SceneController.scenes.Count() + sceneIndex : sceneIndex;
                SceneController.CurrentScene = Math.Abs(sceneIndex % SceneController.scenes.Count());
            }
        }

        private void UpdateSceneIcon() {
            if (currentObject) {
                Destroy(currentObject);
            }
            NewCurrentObject(SceneController.CurrentScene);
        }


        public override bool CanReceiveObject(MovableObject obj) {
            return false;
        }

        public override bool ObjectReceived(MovableObject obj) {
            Destroy(obj.gameObject);
            return true;
        }

        public override void ObjectRemoved(MovableObject obj) {
        }

        public override string GetLabel() {
            return "Selecionar Cena Atual";
        }

        public void NewCurrentObject(int index) {
            currentObject = GameObject.Instantiate(SceneNumberPrefab, showingObjectRoot);
            currentObject.transform.localPosition = new Vector3(0, 0, 0);
            currentObject.GetComponent<MovableObject>().type = MovableObject.TYPE.SCENE_INFO_OBJECT;
            currentObject.GetComponent<NumberIcon>().SetLabel(index.ToString());
            currentObject.GetComponent<NumberIcon>().Number = index;
            currentObject.SetActive(isActive);
        }

        public void CurrentSceneIsGoingToBeDeleted() {
        }

        public void CurrentSceneIsGoingToChange() {
        }

        public void CurrentSceneChanged(Scene currentScene) {
            UpdateSceneIcon();
        }
    }
}
