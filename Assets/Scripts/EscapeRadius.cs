using UnityEngine;

public class EscapeRadius : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        enemy.enemyStatus = Enemy.State.Escape;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        enemy.enemyStatus = Enemy.State.Roaming;
    }
}