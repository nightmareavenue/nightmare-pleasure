using UnityEngine;

public class AttackRadius : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        enemy.enemyStatus = Enemy.State.Attack;
        if (enemy.enemyStatus == Enemy.State.Roaming)
            enemy.enemyStatus = Enemy.State.Attack;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (enemy.enemyStatus == Enemy.State.Attack)
            enemy.enemyStatus = Enemy.State.Roaming;
    }
}