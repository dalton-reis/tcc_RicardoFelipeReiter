using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class PersistController : MonoBehaviour, AnimationControllerListener {

        public static PersistController Instance;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }
        }

        void Start() {
            LoadFromPersistedData();
            FindObjectOfType<AnimationController>().AddListener(this);
        }

        public void PersistEverything() {
            UserPersistData userData = new UserPersistData();
            userData.Scenes = new ScenePersistData[SceneController.Instance.scenes.Count()];
            for (var i = 0; i < SceneController.Instance.scenes.Count(); i++) {
                userData.Scenes[i] = SceneController.Instance.scenes[i].GetPersistData();
            }
            //Debug.Log(JsonUtility.ToJson(userData, true));
            PlayerPrefs.SetString("data", JsonUtility.ToJson(userData, false));
        }

        public void LoadFromPersistedData() {
            var jsonData = PlayerPrefs.GetString("data");
            if (!String.IsNullOrEmpty(jsonData)) {
                UserPersistData userData = JsonUtility.FromJson<UserPersistData>(jsonData);
                SceneController.Instance.scenes.Clear();
                foreach (var scene in userData.Scenes) {
                    var newScene = new GameObject("Map").AddComponent<Scene>();
                    newScene.transform.parent = SceneController.Instance.DesactivatedScenes.transform;
                    newScene.transform.localPosition = Vector3.zero;
                    newScene.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
                    newScene.LoadPersistData(scene);
                    SceneController.Instance.scenes.Add(newScene);
                    SceneController.Instance.CurrentScene = 0;
                }
            }
        }


        public void TakeAdded(int take) {
            PersistEverything();
        }

        public void TakeDeleted(int take) {
            PersistEverything();
        }

        public void StatusChanged(AnimationController.STATUS status) {
        }

        public void AnimationTimesChanged(float currentTime, float endTime, float[] takesTime) {
        }
    }
}
