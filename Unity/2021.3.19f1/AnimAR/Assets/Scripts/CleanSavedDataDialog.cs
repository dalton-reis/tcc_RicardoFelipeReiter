using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
    public class CleanSavedDataDialog : Window {

        public void RemoveDataAndClose() {
            PlayerPrefs.DeleteAll();
            WindowManager.Instance.CloseCurrent();
        }

    }
}
