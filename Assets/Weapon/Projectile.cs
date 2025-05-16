using UnityEngine;


public class Projectile : MonoBehaviour

{
    [Header("Параметры снаряда")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float bulletDamage = 50f;
    [SerializeField] private ParticleSystem bulletTrail;
    private ParticleSystem bulletTrailInstance;

    private void Start()
    {
        // Эффект следа
        if (bulletTrail != null)
        {
            bulletTrailInstance = Instantiate(bulletTrail, transform.position, Quaternion.identity);
            bulletTrailInstance.transform.SetParent(transform);
            bulletTrail.Play();
        }

        // Игнорируем столкновения с игроком
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
        }

        // Игнорируем столкновения с другими снарядами
        IgnoreProjectilesWithTag("Projectile");
        IgnoreProjectilesWithTag("EnemyProjectile");
    }

    private void Update()
    {
        // Движение
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Обновление позиции следа
        if (bulletTrailInstance != null)
        {
            bulletTrailInstance.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Наносим урон
            enemy.Hit(bulletDamage);

            // Применяем отталкивание
            KnockBack knockback = enemy.GetComponent<KnockBack>();
            if (knockback != null)
            {
                Vector2 direction = (collision.transform.position - transform.position).normalized;
                knockback.ApplyKnockback(direction);
            }
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (bulletTrailInstance != null)
        {
            bulletTrailInstance.transform.SetParent(null);
            Destroy(bulletTrailInstance, 1f);
        }
    }

    // Установить урон снаряду извне (например, через апгрейд)
    public void SetDamage(float value)
    {
        bulletDamage = value;
    }

    private void IgnoreProjectilesWithTag(string tag)
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject proj in projectiles)
        {
            if (proj != gameObject && proj.TryGetComponent(out Collider2D otherCollider))
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), otherCollider);
            }
        }
    }
}