using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class Movement : MonoBehaviour, IActivable, IFeatureSetup, IFeatureAction, IFeatureUpdate
    {
        private const float DEFAULT_BASE_MAX_SPEED = 30f;

        [Header("Settings")]
        [SerializeField] private Settings settings;

        [Header("Control")]
        [SerializeField] private bool active;

        [Header("Runtime")]
        [SerializeField] private Vector3 moveDirection;
        [SerializeField] private Vector3 currentSpeed;

        [Header("Parameters")]
        [SerializeField] private float baseMaxSpeed;

        [Header("Components")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform playerOrientation;

        private void Awake()
        {
            if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            float? tempBaseMaxSpeed = settings.Search("baseMaxSpeed");
            if(tempBaseMaxSpeed.HasValue) baseMaxSpeed = tempBaseMaxSpeed.Value;
            else baseMaxSpeed = DEFAULT_BASE_MAX_SPEED;

            ToggleActive(true);
        }

        public void FeatureAction(Controller controller, params Setting[] settings)
        {
            if(!active)
            {
                moveDirection = Vector3.zero;
                return;
            }

            if(settings.Length < 1) return;

            Vector2 moveInput = settings[0].vector2Value;

            KineticEntity kinetic = controller as KineticEntity;

            InputMove(moveInput, kinetic);
        }

        public void UpdateFeature(Controller controller)
        {
            if(!active)
            {
                currentSpeed = Vector3.zero;
                return;
            }

            KineticEntity kinetic = controller as KineticEntity;

            Move(kinetic);

            UpdateKineticInformation(kinetic);
        }

        private void InputMove(Vector2 moveInput, KineticEntity kinetic)
        {
            if(playerOrientation == null || kinetic == null)  return;

            moveDirection = playerOrientation.forward * moveInput.y + playerOrientation.right * moveInput.x;
        }

        private void Move(KineticEntity kinetic)
        {
            if(kinetic == null) return;

            if(_rigidbody != null)
            {
                _rigidbody.AddForce(moveDirection * baseMaxSpeed);
            }
        }

        private void UpdateKineticInformation(KineticEntity kinetic)
        {
            if(kinetic == null) return;

            if(_rigidbody != null) this.currentSpeed = _rigidbody.linearVelocity;

            kinetic.currentSpeed = this.currentSpeed;
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
