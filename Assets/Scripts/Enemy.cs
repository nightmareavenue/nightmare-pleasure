using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private int maxHp = 5;
    [SerializeField] private int contactDamage = 1;
    private int currentHp;

    private float _shootingDelay = 1f;
    private float _cooldown = 0;

    private Rigidbody2D rb;
    private float speed = 3f;
    private float escapeSpeed = 5f;

    private int currentDirection = -1;
    private float startPosition;
    private float roamingRadius = 3f;
    private GameObject player;

    public enum State { Roaming, Escape, Attack }
    public State enemyStatus = State.Roaming;

    [SerializeField] private GameObject _enemyShot;
    [SerializeField] private Transform shotSpawnPoint;

    [Header("Debug")]
    public bool drawBorder = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position.x;
        currentHp = maxHp;
    }

    private void Update()
    {
        switch (enemyStatus)
        {
            case State.Roaming: DoRoaming(); break;
            case State.Escape: DoEscape(); break;
            case State.Attack: DoAttacking(); break;
        }

        if (_cooldown > 0) _cooldown -= Time.deltaTime;
    }

    public void DoRoaming()
    {
        if (startPosition - transform.position.x >= roamingRadius) currentDirection = 1;
        else if (startPosition - transform.position.x <= -roamingRadius) currentDirection = -1;

        rb.linearVelocity = new Vector2(currentDirection * speed, rb.linearVelocity.y);
        FaceDirection(currentDirection);
    }

    private void DoEscape()
    {
        float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(-dir * escapeSpeed, rb.linearVelocity.y);
        FaceDirection((int)dir);
    }

    private void DoAttacking()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        FaceDirection((int)dir);
        Shoot(dir);
    }

    private void Shoot(float direction)
    {
        if (_enemyShot == null || _cooldown > 0) return;

        Vector2 spawnPos = shotSpawnPoint != null ? (Vector2)shotSpawnPoint.position : (Vector2)transform.position;
        GameObject shot = Instantiate(_enemyShot, spawnPos, Quaternion.identity);

        var shotMovement = shot.GetComponent<EnemyShotMovement>();


        _cooldown = _shootingDelay;
    }

    private void FaceDirection(int dir)
    {
        if (dir == 0) return;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0) Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var playerStats = collision.collider.GetComponent<PlayerStats>();
            playerStats?.TakeDamage(contactDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawBorder) return;
        Gizmos.color = Color.yellow;
        float left = startPosition - roamingRadius;
        float right = startPosition + roamingRadius;
        Gizmos.DrawLine(new Vector3(right, transform.position.y, 0), new Vector3(left, transform.position.y, 0));
        Gizmos.DrawCube(new Vector3(left, transform.position.y, 0), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(new Vector3(right, transform.position.y, 0), new Vector3(0.1f, 0.1f, 0.1f));
    }
}