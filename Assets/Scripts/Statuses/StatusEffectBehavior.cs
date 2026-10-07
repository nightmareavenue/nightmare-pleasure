using UnityEngine;

public interface IStatusEffectRuntime
{
    void Apply();
    void Tick(float deltaTime);
    void Remove();
}

public abstract class StatusEffectBehavior : ScriptableObject
{
    public abstract IStatusEffectRuntime CreateRuntime(GameObject target);
}