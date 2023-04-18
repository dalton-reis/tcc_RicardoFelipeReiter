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

        public abstract bool CanReceiveObject(MovableObject obj);

        public abstract bool ObjectReceived(MovableObject obj);

        public abstract void ObjectRemoved(MovableObject obj);

        public abstract string GetLabel();
    }
}
