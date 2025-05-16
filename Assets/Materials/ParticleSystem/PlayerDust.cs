using UnityEngine;

public class PlayerDust : MonoBehaviour
{
    public GameObject dustPrefab;  // ������ ����
    private GameObject dustInstance;
    private ParticleSystem dustParticles;
    private Rigidbody2D rb; // Rigidbody2D ���������

    [SerializeField] private float speedThreshold = 0.1f; // ����������� �������� ��� ��������� ����

    void Start()
    {
        // ������ ����� ��������� � ����������� � � ���������
        dustInstance = Instantiate(dustPrefab, transform.position, Quaternion.identity, transform);
        dustParticles = dustInstance.GetComponent<ParticleSystem>();
        dustParticles.Stop();

        rb = GetComponent<Rigidbody2D>(); // �������� Rigidbody2D
    }

    void Update()
    {
        // ���������, �������� �� �������� �� ����������� (Y ��� �� �����)
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