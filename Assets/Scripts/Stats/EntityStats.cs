using UnityEngine;

public class EntityStats : MonoBehaviour, IDamageable
{
    [SerializeField] private EntityConfig config;
    public float MoveSpeed { get; private set; }
    public float JumpForce { get; private set; }
    public float FallSpeed { get; private set; }
    public int MaxHealth { get; private set; }
    public int ModifiedHealth { get; private set; }
    public int AttackDamage { get; private set; }
    public int ContactDamage {get; private set;}
    public int CurrentHealth { get; private set; }
    public int MaxJumps { get; private set; }
    
    private int bonusJumps = 0;

    private void Awake()
    {
        MoveSpeed = config.moveSpeed;
        JumpForce = config.jumpForce;
        FallSpeed = config.fallSpeed;
        AttackDamage = config.attackDamage;
        ContactDamage = config.contactDamage;
        MaxHealth = config.maxHealth + ModifiedHealth;
        MaxJumps = config.maxJumps + bonusJumps;

        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        MoveSpeed = config.moveSpeed;
        JumpForce = config.jumpForce;
        FallSpeed = config.fallSpeed;
        MaxHealth = config.maxHealth + ModifiedHealth;
        MaxJumps = config.maxJumps + bonusJumps;
    }
    public void UnlockDoubleJump() => bonusJumps = 1;
    public void IncreaseMaxHp(int count) => ModifiedHealth += count;
    public void TakeDamage(int count)
    {
        CurrentHealth -= count;
    } 
}