using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class SceneController : CubeMarkerInteractorImpl {

        public static SceneController Instance;

        public List<Scene> scenes = new List<Scene>();
        public Scene EmptyScenePrefab;
        public GameObject DesactivatedScenes;

        private List<SceneControllerListener> listeners = new List<SceneControllerListener>();
        private int currentScene = -1;

        public int CurrentScene {
            get {
                return currentScene;
            }
            set {
                if (currentScene > -1) {
                    NotifyCurrentSceneIsGoingToChange();
                    GetCurrentScene().Map.transform.parent = DesactivatedScenes.transform;
                }
                currentScene = value;
                GetCurrentScene().Map.transform.parent = this.transform;

                NotifyCurrentSceneChanged();
            }
        }

        void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }

        public void AddNewScene() {
            Scene newScene = GameObject.Instantiate(EmptyScenePrefab, this.transform);
            scenes.Add(newScene);
            CurrentScene = scenes.Count() - 1;
        }

        public Scene GetCurrentScene() {
            return scenes[currentScene];
        }

        public override bool CanReceiveObject(MovableObject obj) {
            return obj.type == MovableObject.TYPE.SCENE_OBJECT;
        }

        public override bool ObjectReceived(MovableObject obj) {
            obj.transform.parent = GetCurrentScene().Map.transform;
            VuforiaUtils.EnableTargetObject(obj.gameObject);
            PersistController.Instance.PersistEverything();
            return false;
        }

        public override void ObjectRemoved(MovableObject obj) {
        }

        public void AddListener(SceneControllerListener listener) {
            listeners.Add(listener);
        }

        private void NotifyCurrentSceneIsGoingToChange() {
            foreach (var listener in listeners) {
                listener.CurrentSceneIsGoingToChange();
            }
        }

        private void NotifyCurrentSceneChanged() {
            foreach (var listener in listeners) {
                listener.CurrentSceneChanged(GetCurrentScene());
            }
        }

        private void NotifyCurrentSceneIsGoingToBeDeleted() {
            foreach (var listener in listeners) {
                listener.CurrentSceneIsGoingToBeDeleted();
            }
        }

        public void RemoveScene(int sceneNumber) {
            var newSceneNumber = sceneNumber;
            if (newSceneNumber >= (scenes.Count() - 1)) {
                newSceneNumber -= 1;
            }

            if (newSceneNumber >= 0) {
                NotifyCurrentSceneIsGoingToBeDeleted();

                Destroy(GetCurrentScene().Map);
                scenes.RemoveAt(sceneNumber);

                currentScene = newSceneNumber;
                GetCurrentScene().Map.transform.parent = this.transform;

                NotifyCurrentSceneChanged();
            } else {
                CurrentScene = 0;
            }
        }
    }
}
