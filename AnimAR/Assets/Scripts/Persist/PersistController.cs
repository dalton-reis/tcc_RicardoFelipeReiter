using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class PersistController : MonoBehaviour {

        public static PersistController Instance;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }
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
            if (jsonData != null) {
                UserPersistData userData = JsonUtility.FromJson<UserPersistData>(jsonData);
                SceneController.Instance.scenes.Clear();
                foreach (var scene in userData.Scenes) {
                    var newScene = new GameObject().AddComponent<Scene>();
                    newScene.transform.parent = SceneController.Instance.DesactivatedScenes.transform;
                    newScene.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                    newScene.LoadPersistData(scene);
                    SceneController.Instance.scenes.Add(newScene);
                    SceneController.Instance.CurrentScene = 0;
                }
            }
        }

    }
}
