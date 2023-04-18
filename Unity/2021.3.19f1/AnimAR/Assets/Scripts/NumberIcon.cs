using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class NumberIcon : MonoBehaviour {

        public Text text1;
        public Text text2;

        private int number;

        public int Number {
            get {
                return number;
            }
            set {
                number = value;
            }
        }

        public void SetLabel(string label) {
            text1.text = label;
            text2.text = label;
        }

    }
}
