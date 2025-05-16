using UnityEngine;

public class WeaponAimFixed : MonoBehaviour
{
    public Transform player; // �����
    public float distanceFromPlayer = 1.5f; // ���������� �� ������
    public float aimSpeed = 10f; // �������� �������� � �����
    public float range = 5f; // ������ ��� ����������� ������
    public string enemyTag = "Enemy"; // ��� ������
    private Transform target; // ������� ����
    private Vector2 previousPlayerPosition; // ���������� ��������� ������

    void Start()
    {
        previousPlayerPosition = player.position; // �������������� ��������� ��������� ������
    }

    void Update()
    {
        FollowPlayer(); // ������ ����������� �� ���������� �� ������
        FindNearestEnemy(); // ���� ���������� �����
        AimAtEnemy(); // �������������� � �����, ���� �� ����
    }

    void FollowPlayer()
    {
        if (player == null) return;

        // ������ ����������� �� ���������� �� ������, ������������ � ���
        Vector2 offset = new Vector2(distanceFromPlayer, 0); // ������� ������ ����� ��������� � �������, ��������� ����� ���

        // ��������� ����������� �������� ������
        Vector2 playerMovementDirection = (Vector2)player.position - previousPlayerPosition;
        if (playerMovementDirection.x < 0) // ���� ����� �������� �����
        {
            offset.x = -distanceFromPlayer; // ����������� ������ �� ��������������� �������
            FlipWeapon(true); // �������� ������ �� �����������
        }
        else // ���� ����� �������� ������
        {
            offset.x = distanceFromPlayer; // ������ ����� � �������� �������
            FlipWeapon(false); // ������ �� ����������
        }

        // ������������� ������ �� ������ ���� ��������
        transform.position = (Vector2)player.position + offset;

        // ���� ������ �� ��������� �� �����, �� ��� ����� �������� � ������� �������� ������
        if (playerMovementDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(playerMovementDirection.y, playerMovementDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime);
        }

        // ��������� ������� ������ ��� ���������� �����
        previousPlayerPosition = player.position;
    }

    void FlipWeapon(bool flip)
    {
        // ����������� ������� ������ �� ��� X, ����� �������� ��� �� �����������
        Vector3 scale = transform.localScale;
        scale.x = flip ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x); // ����������� ������ �� ��� X
        transform.localScale = scale;
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); // �������� ���� ������
        float shortestDistance = range; // ���������� ���� ����� � �������� �������
        Transform nearestEnemy = null;

        // ���� ���������� ����� � �������
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position); // ���������� �� �����
            if (distance < shortestDistance) // ���� ���� � �������
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        // ��������� ����
        target = nearestEnemy;
    }

    void AimAtEnemy()
    {
        if (target == null) return; // ���� ����� ���, ������ �� ������

        // ������������ ����������� � �����
        Vector2 direction = (target.position - transform.position).normalized; // ����������� � �����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // ������� ���� ������������ ������

        // ������ ������������ ������ � �����, ���������� Slerp ��� �������� ��������
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle); // �������� �� ��� Z
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, aimSpeed * Time.deltaTime); // ������� �������
    }
}