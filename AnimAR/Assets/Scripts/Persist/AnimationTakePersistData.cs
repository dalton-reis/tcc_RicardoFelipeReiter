using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    [Serializable]
    public struct AnimationTakePersistData {

        public KeyframePersistData[] PosX;
        public KeyframePersistData[] PosY;
        public KeyframePersistData[] PosZ;

        public KeyframePersistData[] RotX;
        public KeyframePersistData[] RotY;
        public KeyframePersistData[] RotZ;
        public KeyframePersistData[] RotW;

    }
}
