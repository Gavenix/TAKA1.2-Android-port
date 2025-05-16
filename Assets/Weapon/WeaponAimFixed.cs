using UnityEngine;

public class WeaponAimFixed : MonoBehaviour
{
    public Transform player; // Игрок
    public float distanceFromPlayer = 1.5f; // Расстояние от игрока
    public float aimSpeed = 10f; // Скорость поворота к врагу
    public float range = 5f; // Радиус для обнаружения врагов
    public string enemyTag = "Enemy"; // Тег врагов
    private Transform target; // Текущий враг
    private Vector2 previousPlayerPosition; // Предыдущее положение игрока

    void Start()
    {
        previousPlayerPosition = player.position; // Инициализируем начальное положение игрока
    }

    void Update()
    {
        FollowPlayer(); // Оружие фиксируется на расстоянии от игрока
        FindNearestEnemy(); // Ищем ближайшего врага
        AimAtEnemy(); // Поворачиваемся к врагу, если он есть
    }

    void FollowPlayer()
    {
        if (player == null) return;

        // Оружие фиксируется на расстоянии от игрока, передвигаясь с ним
        Vector2 offset = new Vector2(distanceFromPlayer, 0); // Сначала оружие будет двигаться с игроком, оставаясь перед ним

        // Проверяем направление движения игрока
        Vector2 playerMovementDirection = (Vector2)player.position - previousPlayerPosition;
        if (playerMovementDirection.x < 0) // Если игрок движется влево
        {
            offset.x = -distanceFromPlayer; // Инвертируем оружие на противоположную сторону
            FlipWeapon(true); // Отражаем оружие по горизонтали
        }
        else // Если игрок движется вправо
        {
            offset.x = distanceFromPlayer; // Оружие будет с передней стороны
            FlipWeapon(false); // Оружие не отражается
        }

        // Позиционируем оружие на основе этой инверсии
        transform.position = (Vector2)player.position + offset;

        // Если оружие не наводится на врага, то оно будет смотреть в сторону движения игрока
        if (playerMovementDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(playerMovementDirection.y, playerMovementDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime);
        }

        // Обновляем позицию игрока для следующего кадра
        previousPlayerPosition = player.position;
    }

    void FlipWeapon(bool flip)
    {
        // Инвертируем масштаб оружия по оси X, чтобы отразить его по горизонтали
        Vector3 scale = transform.localScale;
        scale.x = flip ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x); // Инвертируем только по оси X
        transform.localScale = scale;
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); // Получаем всех врагов
        float shortestDistance = range; // Изначально враг будет в пределах радиуса
        Transform nearestEnemy = null;

        // Ищем ближайшего врага в радиусе
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position); // Расстояние до врага
            if (distance < shortestDistance) // Если враг в радиусе
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        // Обновляем цель
        target = nearestEnemy;
    }

    void AimAtEnemy()
    {
        if (target == null) return; // Если врага нет, ничего не делаем

        // Рассчитываем направление к врагу
        Vector2 direction = (target.position - transform.position).normalized; // Направление к врагу
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Находим угол относительно оружия

        // Плавно поворачиваем оружие к врагу, используем Slerp для плавного поворота
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle); // Вращение по оси Z
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime); // Плавный поворот
    }
}