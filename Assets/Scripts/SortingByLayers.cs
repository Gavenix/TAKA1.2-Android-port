using UnityEngine;

public class SortingByLayers : MonoBehaviour
{
    private GameObject player; // �����
    private SpriteRenderer playerRenderer; // ������ �������� ������
    private SpriteRenderer objectRenderer; // ������ �������� �������

    void Start()
    {
        // ������� ������ � ����� "Player" � �������� ��� ��������� SpriteRenderer
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRenderer = player.GetComponent<SpriteRenderer>();
        }

        // �������� ��������� SpriteRenderer �������, � �������� ���������� ���� ������
        objectRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ���������, ��� ��� ���������� �������
        if (playerRenderer != null && objectRenderer != null)
        {
            // �������� ������� ������ � �������
            float playerY = player.transform.position.y;
            float objectY = objectRenderer.transform.position.y;

            // ���� ����� ��������� ����� �������� (�� ��� Y)
            if (playerY > objectY)
            {
                // ����� ����� ��������� ����� ��������
                objectRenderer.sortingOrder = 0;
                playerRenderer.sortingOrder = 4;
            }
            else
            {
                // ����� ����� ��������� �� ��������
                objectRenderer.sortingOrder = 1;
                playerRenderer.sortingOrder = 0;
            }
        }
    }
}