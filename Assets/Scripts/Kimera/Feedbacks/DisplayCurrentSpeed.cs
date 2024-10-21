using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Features
{
    public class DisplayCurrentSpeed : MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate
    {
        [Header("Settings")]
        [SerializeField] private Settings settings;

        [Header("Control")]
        [SerializeField] private bool active;

        //[Header("Runtime")]

        //[Header("Parameters")]

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI speedHolder;

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            if(!active)
            {
                if(speedHolder != null) speedHolder.text = string.Empty;
                return;
            }

            KineticEntity kinetic = controller as KineticEntity;
            if(kinetic == null)
            {
                if(speedHolder != null) speedHolder.text = string.Empty;
                return;
            }

            speedHolder.text = $"Speed: {kinetic.currentSpeed.magnitude.ToString()}";
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }
    }
}
