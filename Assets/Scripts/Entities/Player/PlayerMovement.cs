using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EntityStats))]
public class PlayerMovement : MonoBehaviour
{
    private EntityStats playerStats;
    private StatusList statusList;
    private Rigidbody2D rb;
    private bool isGrounded;
    private readonly HashSet<Collider2D> groundContacts = new();
    private InputSystem_Actions controls;
    private int jumpsUsed = 0;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<EntityStats>();
        statusList = GetComponent<StatusList>();
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

    private void Jump(InputAction.CallbackContext context)
    {
        if (statusList != null && statusList.HasEffect<LevitationStatusEffect>()) return;
        if (!isGrounded && jumpsUsed == 0) return;
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
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateGroundContact(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundContacts.Remove(collision.collider);
        isGrounded = groundContacts.Count > 0;
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
}