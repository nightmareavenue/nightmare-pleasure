using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //private int _hp = 5;
    //private int _damage = 1;
    private float _shootingDelay = 1f;
    private float _cooldown = 0;

    [SerializeField] private GameObject _enemyShot;

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

    private void Update()
    {
        Shoot();
    }

}
