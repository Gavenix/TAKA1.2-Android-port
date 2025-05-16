using UnityEngine;

public class SlimeShoot : MonoBehaviour
{
    public GameObject projectilePrefab; // Префаб пули
    public Transform firePoint; // Точка выстрела
    public float minFireRate = 3f; // Минимальная задержка перед выстрелом
    public float maxFireRate = 5f; // Максимальная задержка перед выстрелом
    public float detectionRange = 10f; // Радиус обнаружения игрока
    public float spreadAngle = 10f; // Разброс угла стрельбы

    private Transform player;
    private float nextFireTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SetNextFireTime();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && Time.time >= nextFireTime)
        {
            Shoot();
            SetNextFireTime();
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null || player == null) return;

        // Создаем пулю
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        SlimeProjectile slimeProjectile = projectile.GetComponent<SlimeProjectile>();

        if (slimeProjectile != null)
        {
            Vector2 direction = (player.position - firePoint.position).normalized;

            // Добавляем случайный угол разброса
            float randomAngle = Random.Range(-spreadAngle, spreadAngle);
            direction = Quaternion.Euler(0, 0, randomAngle) * direction;

            // Передаем направление в пулю
            slimeProjectile.SetDirection(direction);
        }
    }

    void SetNextFireTime()
    {
        nextFireTime = Time.time + Random.Range(minFireRate, maxFireRate);
    }
}

