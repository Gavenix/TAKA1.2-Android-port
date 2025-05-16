using UnityEngine;

public class HeavyKnight : MonoBehaviour
{
    [SerializeField] int maxHealth = 200;
    [SerializeField] float speed = 2f;

    private int currentHealth;

    Transform target;

    private void Start()
    {
        currentHealth = maxHealth;
        target = GameObject.Find("Player").transform;

    }

    private void Update()
    {

        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();

            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void Hit(int damage)
    {
        currentHealth -= damage;


        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}
