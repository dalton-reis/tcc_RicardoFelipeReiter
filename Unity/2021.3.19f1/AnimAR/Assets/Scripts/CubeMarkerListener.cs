using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts {
    public interface CubeMarkerListener {

        void ObjectAttached(MovableObject obj);

        void ObjectDetached(MovableObject obj);

        void MarkerLost();
    }
}
