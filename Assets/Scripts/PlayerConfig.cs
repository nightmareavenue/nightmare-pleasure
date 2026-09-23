using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player Config")]
public class PlayerConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float jumpForce = 20f;
    public float fallSpeed = 20f;

    [Header("Health")]
    public int maxHealth = 100;
}
