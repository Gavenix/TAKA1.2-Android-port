using UnityEngine;

public class OBJSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform player; // ������������� ������� ������

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ���� ������ � ����� "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("����� �� ������! ���������, ��� ��������� ����� ��� 'Player'.");
        }
    }

    void Update()
    {
        if (player == null) return; // ���� ����� �� ������, ������ ������� �� Update

        // ���� ���� ���� ��������� �� Y, �� ������ (������ Sorting Order)
        if (transform.position.y > player.position.y)
        {
            spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 4;
        }
        else
        {
            spriteRenderer.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 4;
        }
    }
}
