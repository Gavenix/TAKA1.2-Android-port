using UnityEngine;

public class PlayerDust : MonoBehaviour
{
    public GameObject dustPrefab;  // Префаб пыли
    private GameObject dustInstance;
    private ParticleSystem dustParticles;
    private Rigidbody2D rb; // Rigidbody2D персонажа

    [SerializeField] private float speedThreshold = 0.1f; // Минимальная скорость для появления пыли

    void Start()
    {
        // Создаём копию партиклов и привязываем её к персонажу
        dustInstance = Instantiate(dustPrefab, transform.position, Quaternion.identity, transform);
        dustParticles = dustInstance.GetComponent<ParticleSystem>();
        dustParticles.Stop();

        rb = GetComponent<Rigidbody2D>(); // Получаем Rigidbody2D
    }

    void Update()
    {
        // Проверяем, движется ли персонаж по горизонтали (Y нам не нужен)
        bool isMoving = Mathf.Abs(rb.velocity.x) > speedThreshold;

        if (isMoving && !dustParticles.isPlaying)
        {
            dustParticles.Play();
        }
        else if (!isMoving && dustParticles.isPlaying)
        {
            dustParticles.Stop();
        }
    }
}