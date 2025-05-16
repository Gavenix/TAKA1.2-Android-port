using UnityEngine;
using System.Collections;



public class HeavyKnightAI : MonoBehaviour
{
    private Transform player;
    public float moveSpeed = 3f;
    public float stopDistance = 3f;
    public float attackDelay = 1f;
    public float dashSpeed = 8f;
    public float attackDamage = 35f;
    public float dashDuration = 0.3f;
    public float attackCooldown = 3f;
    public float maxHealth = 250f;

    private Vector2 attackDirection;
    private bool isPreparingAttack = false;
    private bool isDashing = false;
    private bool isOnCooldown = false;
    private Rigidbody2D rb;
    private float currentHealth;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Игрок с тегом 'Player' не найден!");
        }
    }

    void Update()
    {
        if (player == null || currentHealth <= 0) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!isPreparingAttack && !isDashing)
        {
            if (distanceToPlayer > stopDistance)
            {
                // Преследуем игрока во время кулдауна тоже
                Vector2 moveDirection = (player.position - transform.position).normalized;
                rb.velocity = moveDirection * moveSpeed;

                // Разворот врага
                Flip(moveDirection.x);
            }
            else if (!isOnCooldown)
            {
                // Останавливаемся и подготавливаем атаку
                rb.velocity = Vector2.zero;
                StartCoroutine(PrepareAttack());
            }
        }
    }

    IEnumerator PrepareAttack()
    {
        isPreparingAttack = true;

        float elapsedTime = 0f;

        while (elapsedTime < attackDelay)
        {
            if (player == null) yield break;

            attackDirection = (player.position - transform.position).normalized;
            Flip(attackDirection.x);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(PerformDash());
    }

    IEnumerator PerformDash()
    {
        isPreparingAttack = false;
        isDashing = true;
        rb.velocity = attackDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;

        // Включаем задержку перед следующей атакой
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    void Flip(float directionX)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = directionX < 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDashing && other.CompareTag("Player"))
        {
            other.GetComponent<HealthManager>()?.TakeDamage((int)attackDamage);
        }
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}