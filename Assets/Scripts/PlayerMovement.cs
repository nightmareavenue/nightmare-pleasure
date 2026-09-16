using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData = new PlayerData();
    private Rigidbody2D rb;
    private bool isGrounded = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerData = SaveSystem.Load();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        Jump();
        Fall();
    }

    private void Move()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;
        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;

        rb.linearVelocity = new Vector2(horizontalInput * playerData._playerMoveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, playerData._jumpForce);
            isGrounded = false;
        }
    }

    private void Fall()
    {
        if (Input.GetKeyDown(KeyCode.S) && !isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -playerData._fallSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = true;
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            }
        }
    }

}