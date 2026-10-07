using UnityEngine;

[CreateAssetMenu(menuName = "Game/Status Effects/Levitation")]
public class LevitationStatusEffect : StatusEffectBehavior
{
    public override IStatusEffectRuntime CreateRuntime(GameObject target)
    {
        return new LevitationRuntime(target.GetComponent<Rigidbody2D>());
    }

    private sealed class LevitationRuntime : IStatusEffectRuntime
    {
        private readonly Rigidbody2D body;
        private float originalGravityScale;
        private bool applied;

        public LevitationRuntime(Rigidbody2D body)
        {
            this.body = body;
        }

        public void Apply()
        {
            if (body == null) return;

            originalGravityScale = body.gravityScale;
            body.gravityScale = 0f;
            body.linearVelocityY = 2f;
            applied = true;
        }

        public void Tick(float deltaTime) { }

        public void Remove()
        {
            if (!applied || body == null) return;

            body.gravityScale = originalGravityScale;
            applied = false;
        }
    }
}