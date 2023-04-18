using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts {
    [Serializable]
    public struct SceneObjectPersistData {

        public float LocalPosX;
        public float LocalPosY;
        public float LocalPosZ;

        public float LocalRotX;
        public float LocalRotY;
        public float LocalRotZ;
        public float LocalRotW;

        public float LocalScaX;
        public float LocalScaY;
        public float LocalScaZ;

        public string PrefabName;

        public int AnimationTakeId;

    }
}
