using UnityEngine;

public class EnemyShotMovement : MonoBehaviour
{
    private float _speed = 10f;
    private float _lifeTime = 1f;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime);
    }
}