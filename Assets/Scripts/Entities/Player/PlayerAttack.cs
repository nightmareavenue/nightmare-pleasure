using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(EntityStats))]
public class PlayerAttack : MonoBehaviour
{
    private InputSystem_Actions controls;
    private CircleCollider2D attackCollider;
    private float nextAttackTime;
    private float attackDuration = 0.2f;
    private EntityStats entityStats;
    private Coroutine disableAttackCoroutine;
    private readonly HashSet<IDamageable> damagedThisAttack = new();

    private void Awake()
    {
        controls = new();
        attackCollider = GetComponentInChildren<CircleCollider2D>();
        entityStats = GetComponent<EntityStats>();
        attackCollider.enabled = false;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Attack.performed += Attack;
    }

    private void OnDisable()
    {
        controls.Player.Attack.performed -= Attack;
        controls.Player.Disable();
        if (disableAttackCoroutine != null)
        {
            StopCoroutine(disableAttackCoroutine);
            disableAttackCoroutine = null;
        }
        attackCollider.enabled = false;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (Time.time < nextAttackTime) {
            
            return;
        }
        

        Vector2 direction = context.control.name switch
        {
            "leftArrow" => Vector2.left,
            "rightArrow" => Vector2.right,
            "upArrow" => Vector2.up,
            "downArrow" => Vector2.down,
            _ => Vector2.zero
        };

        if (direction == Vector2.zero) return;

        damagedThisAttack.Clear();
        attackCollider.offset = direction;
        attackCollider.enabled = true;
        if (disableAttackCoroutine != null)
        {
            StopCoroutine(disableAttackCoroutine);
        }
        disableAttackCoroutine = StartCoroutine(DisableAttackCollider());
        nextAttackTime = Time.time + 0.5f;
    }

    private System.Collections.IEnumerator DisableAttackCollider()
    {
        yield return new WaitForSeconds(attackDuration);
        attackCollider.enabled = false;
        disableAttackCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!attackCollider.enabled) return;

        IDamageable damageable = collision.GetComponentInParent<IDamageable>();
        if (damageable == null || !damagedThisAttack.Add(damageable)) return;
        damageable.TakeDamage(entityStats.AttackDamage);
    }
}
