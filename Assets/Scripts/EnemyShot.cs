using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public float enemyShotSpeed = 5f;
    public float enemyShotLifeTime = 2f;
    [SerializeField] private float turnSpeed = 200f;

    private Rigidbody2D rb;
    private Transform player;
    private Vector2 currentDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        currentDirection = player != null
            ? ((Vector2)player.position - rb.position).normalized
            : Vector2.left;

        Destroy(gameObject, enemyShotLifeTime);
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 targetDirection = ((Vector2)player.position - rb.position).normalized;
            currentDirection = Vector3.RotateTowards(
                currentDirection,
                targetDirection,
                turnSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime,
                0f
            ).normalized;
        }

        rb.MovePosition(rb.position + currentDirection * enemyShotSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerStats>()?.TakeDamage(5);
            Destroy(gameObject);
        }
    }
}