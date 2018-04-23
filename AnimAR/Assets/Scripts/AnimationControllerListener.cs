using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts {
    public interface AnimationControllerListener {

        void TakeAdded(int take);

        void TakeDeleted(int take);

        void StatusChanged(AnimationController.STATUS status);

        void AnimationTimesChanged(float currentTime, float endTime, float[] takesTime);

    }
}
