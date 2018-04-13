using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts {
    public interface AnimationControllerListener {

        void CurrentTakeChanged(int take);

    }
}
