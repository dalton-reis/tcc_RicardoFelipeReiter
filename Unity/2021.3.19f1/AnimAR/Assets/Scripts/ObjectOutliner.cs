using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class ObjectOutliner : MonoBehaviour {

        private List<Outline> outlines = new List<Outline>();

        void Awake() {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            outlines.Clear();
            foreach (var render in renderers) {
                var outline = render.GetComponent<Outline>();
                if (!outline) {
                    outline = render.gameObject.AddComponent<Outline>();
                    outline.OutlineMode = Outline.Mode.OutlineAll;
                    outline.OutlineWidth = 2;
                    outline.OutlineColor = Color.red;
                    outline.enabled = false;
                }
                outlines.Add(outline);
            }
        }

        public void SetColor(Color color) {
            foreach (var outline in outlines) {
                outline.OutlineColor = color;
            }
        }

        public void SetEnabled(bool enabled) {
            foreach (var outline in outlines) {
                outline.enabled = enabled;
            }
        }

    }
}
