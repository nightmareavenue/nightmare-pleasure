using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerConfig config;
    public float MoveSpeed { get; private set; }
    public float JumpForce { get; private set; }
    public float FallSpeed { get; private set; }
    public int MaxHealth { get; private set; }
    public int modifiedHealth { get; private set; }
    public int currentHealth { get; private set; }

    private void Update()
    {
        MoveSpeed = config.moveSpeed;
        JumpForce = config.jumpForce;
        FallSpeed = config.fallSpeed;
        MaxHealth = config.maxHp + modifiedHealth;
        currentHealth = MaxHealth;
    }

    public void IncreaseMaxHp(int count)
    {
        modifiedHealth += count;
    }
}
