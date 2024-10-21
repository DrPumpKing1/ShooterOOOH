using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Features
{
    public class GroundCheck : MonoBehaviour, IActivable, IFeatureSetup, IFeatureUpdate
    {
        private const float EXTRA_RAYCAST_DISTANCE = .2f;
        private LayerMask DEFAULT_WHAT_IS_GROUND;

        [Header("Settings")]
        [SerializeField] private Settings settings;

        [Header("Control")]
        [SerializeField] private bool active;

        [Header("Runtime")]
        [SerializeField] private bool onGround;

        [Header("Parameters")]
        [SerializeField] private LayerMask whatIsGround;

        [Header("Components")]
        [SerializeField] private Collider _collider;

        [Header("Debug")]
        [SerializeField] private bool debug;

        private void Awake()
        {
            DEFAULT_WHAT_IS_GROUND = LayerMask.NameToLayer("Solid");

            if(_collider == null) _collider = GetComponent<Collider>();
        }

        public void SetupFeature(Controller controller)
        {
            settings = controller.settings;

            LayerMask? tempWhatIsGround = settings.Search("whatIsGround");
            if(tempWhatIsGround.HasValue) whatIsGround = 1 << tempWhatIsGround.Value;
            else whatIsGround = 1 << DEFAULT_WHAT_IS_GROUND;

            ToggleActive(true);
        }

        public void UpdateFeature(Controller controller)
        {
            if(!active)
            {
                onGround = false;
                return;
            }

            TerrainEntity terrain = controller as TerrainEntity;

            Check(terrain);
        }

        private void Check(TerrainEntity terrain)
        {
            if(terrain == null || _collider == null) return;

            this.onGround = Physics.Raycast(transform.position, Vector3.down, _collider.bounds.extents.y + EXTRA_RAYCAST_DISTANCE, whatIsGround);

            terrain.onGround = this.onGround;
        }

        public bool GetActive()
        {
            return active;
        }

        public void ToggleActive(bool active)
        {
            this.active = active;
        }

        private void OnDrawGizmosSelected()
        {
            if(active == false || debug == false || _collider == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (_collider.bounds.extents.y + EXTRA_RAYCAST_DISTANCE));
        }
    }
}
