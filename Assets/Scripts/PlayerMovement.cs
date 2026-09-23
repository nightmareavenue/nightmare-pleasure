using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStats playerStats;
    private Rigidbody2D rb;
    public bool isGrounded = true;
    private InputSystem_Actions controls;
    private int jumpsUsed = 0;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        controls = new();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Jump.performed += Jump;
        controls.Player.Fall.performed += Fall;
    }

    private void OnDisable()
    {
        controls.Player.Jump.performed -= Jump;
        controls.Player.Fall.performed -= Fall;
        controls.Player.Disable();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float movement = controls.Player.Move.ReadValue<float>();
        rb.linearVelocity = new Vector2(movement * playerStats.MoveSpeed, rb.linearVelocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsUsed >= playerStats.MaxJumps) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerStats.JumpForce);
        jumpsUsed++;
    }

    private void Fall(InputAction.CallbackContext context)
    {
        if (jumpsUsed > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -playerStats.FallSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.GetComponentInParent<Enemy>() != null)
        {
            jumpsUsed = 0;
        }
    }
}