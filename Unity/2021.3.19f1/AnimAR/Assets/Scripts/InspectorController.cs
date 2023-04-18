using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

namespace Assets.Scripts {
    public class InspectorController : MonoBehaviour//, ITrackableEventHandler 
    {

        public Text title;
        public Text description;
        public GameObject indicator;

        void Start() {
            //GetComponent<ImageTargetBehaviour>().RegisterTrackableEventHandler(this);//erro
        }

        void OnTriggerEnter(Collider other) {
            var movable = other.GetComponent<MovableObject>();
            var titleString = "Sem informação";
            var descriptionString = "Nenhuma informação encontrada para o objeto selecionado";

            if (movable != null) {
                switch (movable.type) {
                    case MovableObject.TYPE.SCENE_OBJECT:
                        titleString = movable.gameObject.name;
                        var takes = SceneController.Instance.GetCurrentScene().Takes;
                        var index = takes.FindIndex(t => t.GameObject == movable.gameObject);
                        if (index >= 0) {
                            descriptionString = "Animação: " + (index + 1);
                            descriptionString += "\nDuração: " + takes[index].Clip.length;
                        } else {
                            descriptionString = "Sem animação";
                        }
                        break;
                    case MovableObject.TYPE.SCENE_INFO_OBJECT:
                        var sceneNumber = other.GetComponent<NumberIcon>().Number;
                        titleString = "Cena " + sceneNumber;
                        var scene = SceneController.Instance.scenes[sceneNumber];
                        var objectsQuantity = scene.Map.GetComponentsInChildren<MovableObject>().Length;
                        var takesQuantity = scene.Takes.Count;
                        descriptionString = "Objetos: " + objectsQuantity;
                        descriptionString += "\nAnimações: " + takesQuantity;
                        break;
                    case MovableObject.TYPE.TAKE_OBJECT:
                        var takeNumber = other.GetComponent<NumberIcon>().Number;
                        titleString = "Animação " + takeNumber;
                        var take = SceneController.Instance.GetCurrentScene().Takes[takeNumber];
                        descriptionString = "Duração: " + take.Clip.length;
                        break;
                }
                title.text = titleString;
                description.text = descriptionString;
            }
        }

        void OnTriggerExit(Collider other) {
        }

        public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus) {
            if (newStatus != TrackableBehaviour.Status.DETECTED && newStatus != TrackableBehaviour.Status.TRACKED) {
                title.text = "Sem informações";
                description.text = "Selecione um objeto de interesse para inspecionar";
            }
        }
    }
}
