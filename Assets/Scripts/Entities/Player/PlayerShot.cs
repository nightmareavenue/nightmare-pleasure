using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShot : MonoBehaviour
{
    [SerializeField] private float shotSpeed = 20f;
    [SerializeField] private float lifeTime = 2f;

    private Rigidbody2D rb;
    private Vector2 aimDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        aimDirection = (mouseWorld - (Vector2)transform.position).normalized;
    }

    private void Start()
    {

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }

        rb.MovePosition(rb.position + aimDirection * shotSpeed * Time.fixedDeltaTime);
    }
}
