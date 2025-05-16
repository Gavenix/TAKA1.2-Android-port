using UnityEngine;

public class SortingByLayers : MonoBehaviour
{
    private GameObject player; // Игрок
    private SpriteRenderer playerRenderer; // Спрайт рендерер игрока
    private SpriteRenderer objectRenderer; // Спрайт рендерер объекта

    void Start()
    {
        // Находим объект с тегом "Player" и получаем его компонент SpriteRenderer
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerRenderer = player.GetComponent<SpriteRenderer>();
        }

        // Получаем компонент SpriteRenderer объекта, к которому прикреплен этот скрипт
        objectRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Проверяем, что оба компонента найдены
        if (playerRenderer != null && objectRenderer != null)
        {
            // Получаем позиции игрока и объекта
            float playerY = player.transform.position.y;
            float objectY = objectRenderer.transform.position.y;

            // Если игрок находится перед объектом (по оси Y)
            if (playerY > objectY)
            {
                // Игрок будет нарисован перед объектом
                objectRenderer.sortingOrder = 0;
                playerRenderer.sortingOrder = 4;
            }
            else
            {
                // Игрок будет нарисован за объектом
                objectRenderer.sortingOrder = 1;
                playerRenderer.sortingOrder = 0;
            }
        }
    }
}