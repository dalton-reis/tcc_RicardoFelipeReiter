using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class Scene : MonoBehaviour, Persistent<ScenePersistData> {

        public GameObject Map;
        public List<AnimationTake> Takes = new List<AnimationTake>();

        public ScenePersistData GetPersistData() {
            ScenePersistData data = new ScenePersistData();
            var prefabList = Map.GetComponentsInChildren<PersistentPrefab>();

            data.ObjectList = new SceneObjectPersistData[prefabList.Length];
            for (var i = 0; i < prefabList.Length; i++) {
                SceneObjectPersistData objectData = new SceneObjectPersistData();
                objectData.LocalPosX = prefabList[i].transform.localPosition.x;
                objectData.LocalPosY = prefabList[i].transform.localPosition.y;
                objectData.LocalPosZ = prefabList[i].transform.localPosition.z;

                objectData.LocalRotX = prefabList[i].transform.localRotation.x;
                objectData.LocalRotY = prefabList[i].transform.localRotation.y;
                objectData.LocalRotZ = prefabList[i].transform.localRotation.z;
                objectData.LocalRotW = prefabList[i].transform.localRotation.w;

                objectData.LocalScaX = prefabList[i].transform.localScale.x;
                objectData.LocalScaY = prefabList[i].transform.localScale.y;
                objectData.LocalScaZ = prefabList[i].transform.localScale.z;

                objectData.PrefabName = prefabList[i].UniqueName;

                objectData.AnimationTakeId = Takes.FindIndex(take => take.GameObject == prefabList[i].gameObject);

                data.ObjectList[i] = objectData;
            }

            data.AnimationTakeList = new AnimationTakePersistData[Takes.Count()];
            for (var i = 0; i < Takes.Count(); i++) {
                data.AnimationTakeList[i] = Takes[i].GetPersistData();
            }

            return data;
        }

        public void LoadPersistData(ScenePersistData data) {
            Map = this.gameObject;

            Dictionary<int, GameObject> AnimationMap = new Dictionary<int, GameObject>();

            foreach (var obj in data.ObjectList) {
                var prefab = PersistentPrefabList.Instance.list.Find(p => p.UniqueName.Equals(obj.PrefabName));
                if (!prefab) {
                    throw new UnityException("Couldn't find prefab with name " + prefab);
                }

                var newSceneObject = GameObject.Instantiate(prefab, Map.transform);
                newSceneObject.transform.localPosition = new Vector3(obj.LocalPosX, obj.LocalPosY, obj.LocalPosZ);
                newSceneObject.transform.localRotation = new Quaternion(obj.LocalRotX, obj.LocalRotY, obj.LocalRotZ, obj.LocalRotW);
                newSceneObject.transform.localScale = new Vector3(obj.LocalScaX, obj.LocalScaY, obj.LocalScaZ);

                if (obj.AnimationTakeId > -1) {
                    AnimationMap.Add(obj.AnimationTakeId, newSceneObject.gameObject);
                }
            }

            Takes.Clear();
            for (var i = 0; i < data.AnimationTakeList.Count(); i++) {
                GameObject animationGameObject = null;
                AnimationMap.TryGetValue(i, out animationGameObject);

                if (!animationGameObject) {
                    Debug.LogError("Could not find a gameobject for animation take with ID " + i);
                    continue;
                }

                AnimationTake newTake = new AnimationTake(null, null, animationGameObject, null);
                newTake.LoadPersistData(data.AnimationTakeList[i]);

                Takes.Add(newTake);
            }
        }
    }
}
