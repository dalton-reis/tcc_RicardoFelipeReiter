using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts {
    public interface SceneControllerListener {

        void CurrentSceneIsGoingToChange();

        void CurrentSceneChanged(Scene currentScene);

    }
}
