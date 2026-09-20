using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //private int _hp = 5;
    //private int _damage = 1;
    private float _shootingDelay = 1f;
    private float _cooldown = 0;
    private Rigidbody2D rb;
    private float speed = 3f;
    private int currentDirection = -1;
    private float startPosition;
    private float roamingRadius = 3f;
    private GameObject player;

    public enum State
    {
        Roaming,
        Chase,
        Attack
    }

    public State enemyStatus = State.Roaming;

    [SerializeField] private GameObject _enemyShot;
    [Header("Debug")]
    public bool drawBorder = false; //Gizmos отрисовка границ патрулирования

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position.x;
    }
    private void Shoot()
    {
        if (_enemyShot != null)
        {
            if (_cooldown <= 0)
            {
                Instantiate(_enemyShot, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
                _cooldown = _shootingDelay;
            }
            else
            {
                _cooldown -= Time.deltaTime;
            }
        }
    }

    private void DoChasing()
    {
        transform.position = Vector2.MoveTowards(transform.position,player.transform.position,speed * Time.deltaTime);
    }

    public void DoRoaming()
    {
        if (startPosition - transform.position.x >= roamingRadius)
        {
            currentDirection = 1;
        }
        else if (startPosition - transform.position.x <= -roamingRadius)
        {
            currentDirection = -1;
        }
        rb.linearVelocity = new Vector2(currentDirection * speed, rb.linearVelocity.y);
    }

    private void Update()
    {
        switch (enemyStatus)
        {
            case State.Roaming: DoRoaming();
                break;
            case State.Chase: DoChasing();
                break;
            case State.Attack: Shoot();
                break;
            default:
                break;
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (!drawBorder) return;
        Gizmos.color = Color.yellow;

        float left = startPosition - roamingRadius;
        float right = startPosition + roamingRadius;

        Gizmos.DrawLine(new Vector3(right,transform.position.y,0), new Vector3(left, transform.position.y, 0));
        Gizmos.DrawCube(new Vector3(left, transform.position.y, 0), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(new Vector3(right, transform.position.y, 0), new Vector3(0.1f, 0.1f, 0.1f));
    }
}