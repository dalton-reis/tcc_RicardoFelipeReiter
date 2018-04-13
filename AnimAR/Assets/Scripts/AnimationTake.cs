using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class AnimationTake {

        private Animation animation;
        private AnimationClip clip;
        private GameObject gameObject;

        public Animation Animation {
            get {
                return animation;
            }
            set {
                animation = value;
            }
        }

        public AnimationClip Clip {
            get {
                return clip;
            }
            set {
                clip = value;
            }
        }

        public GameObject GameObject {
            get {
                return gameObject;
            }
            set {
                gameObject = value;
            }
        }

        public AnimationTake(Animation animation, AnimationClip clip, GameObject gameObject) {
            this.Animation = animation;
            this.Clip = clip;
            this.GameObject = gameObject;
        }

    }
}
