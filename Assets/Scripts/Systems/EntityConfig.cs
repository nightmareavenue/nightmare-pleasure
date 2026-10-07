using UnityEngine;

[CreateAssetMenu(menuName = "Game/New Entity Config")]
public class EntityConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 0;
    public float jumpForce = 0;
    public float fallSpeed = 0;

    [Header("Health")]
    public int maxHealth = 0;
    public int maxJumps = 0;

    [Header("Attack")]
    public int attackDamage = 0;
    public int contactDamage = 0;
}
