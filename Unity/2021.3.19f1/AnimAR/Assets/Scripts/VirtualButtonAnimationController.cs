using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts {
    public class VirtualButtonAnimationController : MonoBehaviour//, IVirtualButtonEventHandler
     {

        public Sprite standyBySprite;
        public Sprite overSprite;
        public GameObject VirtualButton;

        void Start() {
            //VirtualButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
                
                //new
                VirtualButton.GetComponent<VirtualButtonBehaviour>().RegisterOnButtonPressed(OnButtonPressed);
                VirtualButton.GetComponent<VirtualButtonBehaviour>().RegisterOnButtonReleased(OnButtonReleased);



            GetComponent<SpriteRenderer>().sprite = standyBySprite;
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb) {
            GetComponent<SpriteRenderer>().sprite = overSprite;
        }

        public void OnButtonReleased(VirtualButtonBehaviour vb) {
            GetComponent<SpriteRenderer>().sprite = standyBySprite;
        }
    }
}
