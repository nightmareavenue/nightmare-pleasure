using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class StatusList : MonoBehaviour
{
    [System.Serializable]
    private class ActiveStatus
    {
        public StatusEffectConfig config;
        public float remainingTime;
        public int damage;
        public float tickTimer;
        public ActiveStatus(StatusEffectConfig config)
        {
            this.config = config;
            remainingTime = config.duration;
            damage = config.healthDamage * config.level;
            tickTimer = config.tickInterval;
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
        active.ConvertAll(a => new StatusInfo(a.config, a.remainingTime));
    public event Action OnStatusesChanged;
    private IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    public void AddEffect(StatusEffectConfig effect)
    {
        var existing = active.Find(a => a.config == effect);

        if (existing != null)
        {
            existing.remainingTime += effect.duration;
        }
        else
        {
            active.Add(new ActiveStatus(effect));
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
            status.tickTimer -= Time.deltaTime;

            if (status.tickTimer <= 0 && status.config.healthDamage > 0)
            {
                status.tickTimer = status.config.tickInterval;
                int damage = status.config.healthDamage * status.config.level;
                damageable?.TakeDamage(damage);
            }

            if (status.remainingTime <= 0)
            {
                active.RemoveAt(i);
                changed = true;
            }
        }

        if (changed) OnStatusesChanged?.Invoke();
    }
}