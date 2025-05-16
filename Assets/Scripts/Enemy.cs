using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] float speed = 2f;

    public GameObject manaOrbPrefab;
    public int minDrop = 1;
    public int maxDrop = 3;

    public GameObject CoinPrefab;
    public int minCoins = 1;
    public int maxCoins = 3;

    [SerializeField] private GameObject targetIndicator; 

    private int currentHealth;
    private Transform target;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        currentHealth = maxHealth;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (targetIndicator != null)
            targetIndicator.SetActive(false); 
    }

    private void Update()
    {
        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;

        ChangeFacingDirection(direction);
    }

    private void ChangeFacingDirection(Vector3 moveDirection)
    {
        if (moveDirection.x > 0)
            spriteRenderer.flipX = false;
        else if (moveDirection.x < 0)
            spriteRenderer.flipX = true;
    }

    public void Hit(float damage)
    {
        currentHealth -= Mathf.RoundToInt(damage);

        if (currentHealth <= 0)
        {
            DropManaPoint();
            DropCoin();
            Destroy(gameObject);
        }
    }

    void DropManaPoint()
    {
        int dropCount = Random.Range(minDrop, maxDrop);
        for (int i = 0; i < dropCount; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            Instantiate(manaOrbPrefab, spawnPos, Quaternion.identity);
        }
    }

    void DropCoin()
    {
        int dropCount = Random.Range(minCoins, maxCoins);
        for (int i = 0; i < dropCount; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            Instantiate(CoinPrefab, spawnPos, Quaternion.identity);
        }
    }

    
    public void ShowTargetIndicator(bool state)
    {
        if (targetIndicator != null)
            targetIndicator.SetActive(state);
    }
}







