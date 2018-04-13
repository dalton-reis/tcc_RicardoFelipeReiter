using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public abstract class Selector : MonoBehaviour {

        public Transform showingObjectRoot;

        public abstract void Next();

        public abstract void Prev();

        public abstract void Active();

        public abstract void Desactive();

        public abstract bool ObjectReceived(GameObject obj);

        public abstract void ObjectRemoved(GameObject obj);

        public abstract string GetLabel();
    }
}
