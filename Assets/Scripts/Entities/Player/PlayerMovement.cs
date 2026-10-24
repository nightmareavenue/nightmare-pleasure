using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStats))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Wall jump")]
    private float wallJumpForce = 8f;
    private float wallJumpLockTime = 0.1f;
    private float wallClingPressure = 1f;
    private float wallClingMaxTime = 0.1f;
    private float wallSlideSpeed = 1.5f;

    private EntityStats playerStats;
    private StatusList statusList;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingWall;
    private readonly HashSet<Collider2D> groundContacts = new();
    private readonly HashSet<Collider2D> wallContacts = new();
    private InputSystem_Actions controls;
    private int jumpsUsed = 0;
    private float wallNormalX;
    private float wallJumpLockTimer;
    private float wallClingTimer;
    private float defaultGravityScale;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<EntityStats>();
        statusList = GetComponent<StatusList>();
        defaultGravityScale = rb.gravityScale;
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
        if (wallJumpLockTimer > 0)
        {
            wallJumpLockTimer -= Time.fixedDeltaTime;
            return;
        }

        bool clinging = IsWallClinging();
        rb.gravityScale = clinging ? 0f : defaultGravityScale;

        if (clinging)
        {
            wallClingTimer += Time.fixedDeltaTime;
            float verticalSpeed = wallClingTimer >= wallClingMaxTime ? -wallSlideSpeed : 0f;
            rb.linearVelocity = new Vector2(-wallNormalX * wallClingPressure, verticalSpeed);
            return;
        }

        wallClingTimer = 0f;
        Move();
    }

    private bool IsWallClinging()
    {
        var kb = Keyboard.current;
        if (kb == null || !isTouchingWall || isGrounded) return false;
        if (rb.linearVelocity.y > 0.1f) return false;

        return wallNormalX > 0f ? kb.aKey.isPressed : kb.dKey.isPressed;
    }

    private void Move()
    {
        float movement = controls.Player.Move.ReadValue<float>();
        rb.linearVelocity = new Vector2(movement * playerStats.MoveSpeed, rb.linearVelocity.y);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (statusList != null && statusList.HasEffect<LevitationStatusEffect>()) return;

        if (isTouchingWall && !isGrounded)
        {
            rb.gravityScale = defaultGravityScale;
            rb.linearVelocity = new Vector2(wallNormalX * wallJumpForce, playerStats.JumpForce);
            wallJumpLockTimer = wallJumpLockTime;
            wallClingTimer = 0f;
            jumpsUsed = 1;
            return;
        }

        bool canJumpFromWall = isTouchingWall || isGrounded;
        if (!canJumpFromWall && jumpsUsed == 0) return;
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateGroundContact(collision);
        UpdateWallContact(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateGroundContact(collision);
        UpdateWallContact(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundContacts.Remove(collision.collider);
        wallContacts.Remove(collision.collider);
        isGrounded = groundContacts.Count > 0;
        isTouchingWall = wallContacts.Count > 0;
    }

    private void UpdateGroundContact(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Ground")) return;

        bool touchingTop = false;
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y >= 0.7f)
            {
                touchingTop = true;
                break;
            }
        }

        if (touchingTop && groundContacts.Add(collision.collider))
        {
            jumpsUsed = 0;
        }
        else if (!touchingTop)
        {
            groundContacts.Remove(collision.collider);
        }

        isGrounded = groundContacts.Count > 0;
    }

    private void UpdateWallContact(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground")) return;

        bool touchingWall = false;
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            if (Mathf.Abs(normal.x) >= 0.7f)
            {
                touchingWall = true;
                wallNormalX = Mathf.Sign(normal.x);
                break;
            }
        }

        if (touchingWall)
        {
            if (wallContacts.Add(collision.collider))
            {
                jumpsUsed = 0;
            }
        }
        else
        {
            wallContacts.Remove(collision.collider);
        }

        isTouchingWall = wallContacts.Count > 0;
    }
}