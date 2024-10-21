using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class LookAround :  MonoBehaviour, IActivable, IFeatureSetup, IFeatureAction //Other channels
    {
        private Vector2 DEFAULT_SENSIBILITY = new Vector2(25f, 25f);
        private const float DEFULAT_X_ROTATION_UPPER_LIMIT = 90f;
        private const float DEFULAT_X_ROTATION_LOWER_LIMIT = -90f;

        //Configuration
        [Header("Settings")]
        public Settings settings;
        //Control
        [Header("Control")]
        [SerializeField] private bool active;
        //States
        [Header("States")]
        [SerializeField] private Vector2 rotation;
        //Properties
        [Header("Properties")]
        [SerializeField] private Vector2 sensibility;
        [SerializeField] private float xRotationUpperLimit;
        [SerializeField] private float xRotationLowerLimit;
        //References
        //Componentes
        [Header("Components")]
        [SerializeField] private Transform playerOrientation;
        [SerializeField] private Transform cameraHolder;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            //Setup Properties
            Vector2? tempSensibility = settings.Search("sensibility");
            if(tempSensibility.HasValue) sensibility = tempSensibility.Value;
            else sensibility = DEFAULT_SENSIBILITY;

            float? tempXRotationUpperLimit = settings.Search("xRotationUpperLimit");
            if(tempXRotationUpperLimit.HasValue) xRotationUpperLimit = tempXRotationUpperLimit.Value;
            else  xRotationUpperLimit = DEFULAT_X_ROTATION_UPPER_LIMIT;

            float? tempXRotationLowerLimit = settings.Search("xRotationLowerLimit");
            if(tempXRotationLowerLimit.HasValue) xRotationLowerLimit = tempXRotationLowerLimit.Value;
            else xRotationLowerLimit = DEFULAT_X_ROTATION_LOWER_LIMIT;

            ToggleActive(true);
        }

        public void FeatureAction(Controller controller, params Setting[] setting){
            if (!active) return;

            if (setting.Length < 1) return;

            Vector2 mouseDelta = setting[0].vector2Value;

            rotation.x = Mathf.Clamp(rotation.x - mouseDelta.y * sensibility.y * Time.deltaTime, xRotationLowerLimit, xRotationUpperLimit);
            rotation.y += mouseDelta.x * sensibility.x * Time.deltaTime;

            if(cameraHolder != null) cameraHolder.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
            if(playerOrientation != null) playerOrientation.rotation = Quaternion.Euler(0f, rotation.y, 0f);
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
