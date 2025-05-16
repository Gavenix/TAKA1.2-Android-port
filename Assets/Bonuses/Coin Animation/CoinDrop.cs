using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    public float MoneyValue = 3;
    public float collectRadius = 1.5f;
    public float moveSpeed = 10f; // �������� ���������� ������

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // �������� �������� �������
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < collectRadius)
            {
                // ������� ������ � ������ � ���������� ���������
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.UpdateCoins(MoneyValue);
                Destroy(gameObject);
            }
        }
    }
}

