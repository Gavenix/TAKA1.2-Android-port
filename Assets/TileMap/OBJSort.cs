using UnityEngine;

public class OBJSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform player; // Автоматически находим игрока

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ищем объект с тегом "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Игрок не найден! Убедитесь, что персонажу задан тег 'Player'.");
        }
    }

    void Update()
    {
        if (player == null) return; // Если игрок не найден, просто выходим из Update

        // Если ящик выше персонажа по Y, он позади (меньше Sorting Order)
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
