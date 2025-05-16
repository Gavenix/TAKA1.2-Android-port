
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 5f;
    public float destroyTime = 5f;
    public float slowDownFactor = 0.98f; // Коэффициент замедления

    private Rigidbody2D rb;
    private Vector2 direction;// Сохраняем направление до инициализации Rigidbody
    private Collider2D myCollider;

    

    void Start()
    {


        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
            Debug.Log("Пуля запущена! Направление: " + direction + ", Скорость: " + rb.linearVelocity);
        }

        

        Destroy(gameObject, destroyTime);

        Collider2D myCollider = GetComponent<Collider2D>();
        Collider2D[] allProjectiles = FindObjectsOfType<Collider2D>();

        foreach (Collider2D col in allProjectiles)
        {
            if (col.CompareTag("Projectile") || col.CompareTag("EnemyProjectile") || col.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(myCollider, col);
            }
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Сохраняем нормализованное направление
    }

    void FixedUpdate()
    {

        if (rb != null && rb.linearVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity *= slowDownFactor;
        }
        else if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            other.GetComponent<HealthManager>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Solid"))
        {
            Destroy(gameObject);
        }
    }
    
}




