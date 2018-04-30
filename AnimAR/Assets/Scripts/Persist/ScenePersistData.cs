using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts {
    [Serializable]
    public struct ScenePersistData {

        public SceneObjectPersistData[] ObjectList;
        public AnimationTakePersistData[] AnimationTakeList;

    }
}
