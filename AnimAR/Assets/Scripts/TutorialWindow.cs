using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class TutorialWindow : Window {

        [Serializable]
        public class Step {

            public Sprite image;
            public String text;

        }

        public Step[] steps;
        public Image stepImage;
        public Text stepText;
        public RectTransform stepExitButton;
        public Text stepNextButtonText;

        private int currentStep = 0;

        void Start() {
            WindowManager.Instance.RegisterWindow(this);
            gameObject.SetActive(false);
            AddLineBreaks();
            ChangeStep(0);
        }

        private void AddLineBreaks() {
            foreach (var step in steps) {
                step.text = step.text.Replace("\\n", "\n");
            }
        }

        public void NextStep() {
            if (currentStep == steps.Length - 1) {
                Close();
            } else {
                ChangeStep(currentStep + 1);
            }
        }

        public void Close() {
            WindowManager.Instance.CloseCurrent();
            ChangeStep(0);
        }

        private void ChangeStep(int step) {
            currentStep = step;
            stepImage.sprite = steps[step].image;
            stepText.text = steps[step].text;
            if (step < steps.Length - 1) {
                stepExitButton.gameObject.SetActive(true);
                stepNextButtonText.text = "Próximo";
            } else {
                stepExitButton.gameObject.SetActive(false);
                stepNextButtonText.text = "Sair";
            }
        }

    }
}
