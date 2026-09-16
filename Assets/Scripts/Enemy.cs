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
    private float speed = 1f;
    private int currentDirection = -1;
    private float startPosition;

    [SerializeField] private GameObject _enemyShot;
    [Header("Debug")]
    public bool drawBorder = false; //Gizmos отрисовка границ патрулирования

    private void Awake()
    {
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

    private void FixedUpdate()
    {
        Roaming();
    }

    public void Roaming()
    {
        if (startPosition - transform.position.x >= 1)
        {
            currentDirection = 1;
        }
        else if (startPosition - transform.position.x <= -1)
        {
            currentDirection = -1;
        }
        rb.linearVelocity = new Vector2(currentDirection * speed, rb.linearVelocity.y);
    }

    private void Update()
    {
        Shoot();
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawBorder) return;
        Gizmos.color = Color.yellow;

        float left = startPosition - 1f;
        float right = startPosition + 1f;

        Gizmos.DrawLine(new Vector3(right,transform.position.y,0), new Vector3(left, transform.position.y, 0));
        Gizmos.DrawCube(new Vector3(left, transform.position.y, 0), new Vector3(0.1f, 0.1f, 0.1f));
        Gizmos.DrawCube(new Vector3(right, transform.position.y, 0), new Vector3(0.1f, 0.1f, 0.1f));
    }
}