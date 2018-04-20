using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts {
    public interface CubeMarkerInteractor {

        bool CanReceiveObject(MovableObject obj);

        void ObjectReceived(MovableObject obj);

        void ObjectRemoved(MovableObject obj);

    }
}
