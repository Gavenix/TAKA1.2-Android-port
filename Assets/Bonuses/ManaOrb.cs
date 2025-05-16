using UnityEngine;

public class ManaOrb : MonoBehaviour
{
    public float ManaValue = 3;
    public float collecetRadius = 1.5f;
    public float moveSpeed = 10f;

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player && Vector2.Distance(transform.position, player.transform.position) < collecetRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController)
            {
                playerController.UpdateMana(ManaValue);
                Destroy(gameObject);
            }
        }
    }
}
