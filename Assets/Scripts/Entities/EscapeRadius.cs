using UnityEngine;

public class EscapeRadius : MonoBehaviour
{
    private Magician enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Magician>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        enemy.enemyStatus = Magician.State.Escape;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        enemy.enemyStatus = Magician.State.Roaming;
    }
}