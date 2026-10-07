using UnityEngine;

public class AttackRadius : MonoBehaviour
{
    private Magician enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Magician>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        enemy.enemyStatus = Magician.State.Attack;
        if (enemy.enemyStatus == Magician.State.Roaming)
            enemy.enemyStatus = Magician.State.Attack;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (enemy.enemyStatus == Magician.State.Attack)
            enemy.enemyStatus = Magician.State.Roaming;
    }
}