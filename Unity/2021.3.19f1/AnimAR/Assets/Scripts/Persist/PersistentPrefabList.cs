using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class PersistentPrefabList : MonoBehaviour {

        public static PersistentPrefabList Instance;
        public List<PersistentPrefab> list = new List<PersistentPrefab>();

        void Awake() {
            if (Instance == null) {
                Instance = this;
            } else if (Instance != this) {
                Destroy(gameObject);
            }
        }

    }
}
