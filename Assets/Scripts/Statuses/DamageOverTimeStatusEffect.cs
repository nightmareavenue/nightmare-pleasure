using UnityEngine;

[CreateAssetMenu(menuName = "Game/Status Effects/Damage Over Time")]
public class DamageOverTimeStatusEffect : StatusEffectBehavior
{
    [SerializeField] private int damage = 1;
    [SerializeField] private int level = 1;
    [SerializeField] private float tickInterval = 1f;

    public override IStatusEffectRuntime CreateRuntime(GameObject target)
    {
        return new DamageOverTimeRuntime(
            target.GetComponent<IDamageable>(),
            damage * level,
            Mathf.Max(0.01f, tickInterval));
    }

    private sealed class DamageOverTimeRuntime : IStatusEffectRuntime
    {
        private readonly IDamageable damageable;
        private readonly int damage;
        private readonly float tickInterval;
        private float tickTimer;

        public DamageOverTimeRuntime(IDamageable damageable, int damage, float tickInterval)
        {
            this.damageable = damageable;
            this.damage = damage;
            this.tickInterval = tickInterval;
            tickTimer = tickInterval;
        }

        public void Apply() { }

        public void Tick(float deltaTime)
        {
            tickTimer -= deltaTime;
            if (tickTimer > 0) return;

            tickTimer = tickInterval;
            damageable?.TakeDamage(damage);
        }

        public void Remove() { }
    }
}