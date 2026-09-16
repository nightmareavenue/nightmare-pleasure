using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData = new();
    private Rigidbody2D rb;
    private bool isGrounded = true;
    private InputSystem_Actions controls;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerData = SaveSystem.Load();
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
        rb.linearVelocity = new Vector2(movement * playerData._playerMoveSpeed, rb.linearVelocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerData._jumpForce);
            isGrounded = false;
        }
    }

    private void Fall(InputAction.CallbackContext context)
    {
        if (!isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -playerData._fallSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) isGrounded = true;
    }
}