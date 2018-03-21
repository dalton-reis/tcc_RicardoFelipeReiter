using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts {
    public interface CubeMarkerInteractor {

        bool ObjectReceived(GameObject obj);

        void ObjectRemoved(GameObject obj);

    }
}
