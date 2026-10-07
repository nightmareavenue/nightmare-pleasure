using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusList : MonoBehaviour
{
    private class ActiveStatus
    {
        public StatusEffectConfig config;
        public float remainingTime;
        public IStatusEffectRuntime runtime;

        public ActiveStatus(StatusEffectConfig config, GameObject target)
        {
            this.config = config;
            remainingTime = config.duration;
            runtime = config.Behavior != null ? config.Behavior.CreateRuntime(target) : null;
            runtime?.Apply();
        }
    }

    public readonly struct StatusInfo
    {
        public readonly StatusEffectConfig Config;
        public readonly float RemainingTime;

        public StatusInfo(StatusEffectConfig config, float remainingTime)
        {
            Config = config;
            RemainingTime = remainingTime;
        }
    }

    private readonly List<ActiveStatus> active = new();

    public IReadOnlyList<StatusInfo> ActiveStatuses =>
        active.ConvertAll(status => new StatusInfo(status.config, status.remainingTime));
    public event Action OnStatusesChanged;

    public bool HasEffect<TBehavior>() where TBehavior : StatusEffectBehavior =>
        active.Exists(status => status.config.Behavior is TBehavior);

    public void AddEffect(StatusEffectConfig effect)
    {
        if (effect == null) return;

        var existing = active.Find(status => status.config == effect);
        if (existing != null)
        {
            existing.remainingTime += effect.duration;
        }
        else
        {
            active.Add(new ActiveStatus(effect, gameObject));
        }

        OnStatusesChanged?.Invoke();
    }

    private void Update()
    {
        bool changed = false;

        for (int i = active.Count - 1; i >= 0; i--)
        {
            var status = active[i];
            status.remainingTime -= Time.deltaTime;
            status.runtime?.Tick(Time.deltaTime);

            if (status.remainingTime <= 0)
            {
                status.runtime?.Remove();
                active.RemoveAt(i);
                changed = true;
            }
        }

        if (changed) OnStatusesChanged?.Invoke();
    }

    private void OnDestroy()
    {
        foreach (var status in active)
        {
            status.runtime?.Remove();
        }
    }
}