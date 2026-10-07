using UnityEngine;

[RequireComponent(typeof(EntityStats))]
public class Barrel : MonoBehaviour, IDamageable
{
    [Header("Combat")]
    private int currentHp;
    private EntityStats entityStats;

    private void Awake()
    {
        entityStats = GetComponent<EntityStats>();
        currentHp = entityStats.MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

}
