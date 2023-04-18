using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class TakeIcon : NumberIcon {

        private AnimationTake take;

        public AnimationTake Take {
            get {
                return take;
            }
            set {
                take = value;
            }
        }

    }
}
