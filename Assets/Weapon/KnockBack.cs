using UnityEngine;
using System.Collections;
public class KnockBack : MonoBehaviour
{
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackDuration = 0.2f; // Длительность отталкивания
    [SerializeField] private float knockbackForce = 10f; // Сила отталкивания
    private Rigidbody2D rb;
    private bool isKnockbackActive = false;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 direction)
    {
        if (isKnockbackActive) return;

        isKnockbackActive = true;

        // Останавливаем текущую скорость (если есть)
        rb.velocity = Vector2.zero;

        // Применяем силу отталкивания
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // Запускаем таймер для окончания отталкивания
        StartCoroutine(KnockbackTimer());
    }

    private IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(knockbackDuration);

        // После окончания отталкивания, прекращаем движение
        rb.velocity = Vector2.zero;
        isKnockbackActive = false;
    }
}
