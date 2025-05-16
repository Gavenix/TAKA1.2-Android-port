using UnityEngine;

public class WeaponOrbitAutoAim : MonoBehaviour
{
    public Transform player; // Игрок
    public float orbitRadius = 1.5f; // Радиус орбиты
    public float orbitSpeed = 100f; // Скорость вращения
    public float range = 5f; // Дистанция авто-наведения
    public string enemyTag = "Enemy"; // Тег врага
    private Transform target; // Текущая цель (враг)
    private float currentAngle = 0f; // Угол вращения

    void Update()
    {
        FindNearestEnemy(); // Ищем врага
        OrbitAroundPlayer(); // Оружие всегда летает вокруг игрока

        if (target != null)
        {
            AimAtEnemy(); // Если враг найден, поворачиваемся к нему
        }
    }

    void OrbitAroundPlayer()
    {
        if (player == null) return;

        // Увеличиваем угол вращения (в радианах)
        currentAngle += orbitSpeed * Time.deltaTime;
        if (currentAngle > 360f) currentAngle -= 360f;

        // Вычисляем позицию вокруг игрока
        float x = player.position.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius;
        float y = player.position.y + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius;

        // Перемещаем оружие
        transform.position = new Vector2(x, y);
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = range;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(player.position, enemy.transform.position); // Проверяем от игрока
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        target = nearestEnemy; // Обновляем цель
    }

    void AimAtEnemy()
    {
        if (target == null) return;

        // Оружие остаётся на орбите, но направляется на врага
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}