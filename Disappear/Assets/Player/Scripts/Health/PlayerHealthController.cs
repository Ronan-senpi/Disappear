using UnityEngine;
using WAG.Health;

namespace WAG.Player.Health
{
    public class PlayerHealthController : HealthStatusController
    {
        private PlayerController pc;

        [SerializeField] private float healthySpeedModifier;
        [SerializeField] private float woundedSpeedModifier = -0.2f;
        [SerializeField] private float dyingSpeedModifier = -0.8f;

        public float HealthSpeedModifier { get; private set; } = 0;

        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent<PlayerController>(out pc))
            {
                Debug.LogError("Need PlayerController", this);
                Debug.Break();
            }
            pc.Sync.SyncHealth(startHeathStatus);
            OnHealthChanged += status => pc.Sync.SyncHealth(status);
        }

        protected override void OnHealthy()
        {
            HealthSpeedModifier = healthySpeedModifier;
        }

        protected override void OnWounded()
        {
            HealthSpeedModifier = woundedSpeedModifier;
        }

        protected override void OnDying()
        {
            HealthSpeedModifier = dyingSpeedModifier;
        }

        protected override void OnDead()
        {
            HealthSpeedModifier = -1;
        }
    }
}