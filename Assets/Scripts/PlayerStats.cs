using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerConfig config;

    public float MoveSpeed { get; private set; }
    public float JumpForce { get; private set; }
    public float FallSpeed { get; private set; }
    public int MaxHp { get; private set; }

    private void Update()
    {
        MoveSpeed = config.moveSpeed;
        JumpForce = config.jumpForce;
        FallSpeed = config.fallSpeed;
        MaxHp = config.maxHp;
    }
}
