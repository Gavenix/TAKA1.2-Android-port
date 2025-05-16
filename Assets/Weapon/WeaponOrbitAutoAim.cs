using UnityEngine;

public class WeaponOrbitAutoAim : MonoBehaviour
{
    public Transform player; // �����
    public float orbitRadius = 1.5f; // ������ ������
    public float orbitSpeed = 100f; // �������� ��������
    public float range = 5f; // ��������� ����-���������
    public string enemyTag = "Enemy"; // ��� �����
    private Transform target; // ������� ���� (����)
    private float currentAngle = 0f; // ���� ��������

    void Update()
    {
        FindNearestEnemy(); // ���� �����
        OrbitAroundPlayer(); // ������ ������ ������ ������ ������

        if (target != null)
        {
            AimAtEnemy(); // ���� ���� ������, �������������� � ����
        }
    }

    void OrbitAroundPlayer()
    {
        if (player == null) return;

        // ����������� ���� �������� (� ��������)
        currentAngle += orbitSpeed * Time.deltaTime;
        if (currentAngle > 360f) currentAngle -= 360f;

        // ��������� ������� ������ ������
        float x = player.position.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius;
        float y = player.position.y + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius;

        // ���������� ������
        transform.position = new Vector2(x, y);
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = range;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(player.position, enemy.transform.position); // ��������� �� ������
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        target = nearestEnemy; // ��������� ����
    }

    void AimAtEnemy()
    {
        if (target == null) return;

        // ������ ������� �� ������, �� ������������ �� �����
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}