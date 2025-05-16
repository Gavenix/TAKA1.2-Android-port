using UnityEngine;
using System.Collections;
public class KnockBack : MonoBehaviour
{
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackDuration = 0.2f; // ������������ ������������
    [SerializeField] private float knockbackForce = 10f; // ���� ������������
    private Rigidbody2D rb;
    private bool isKnockbackActive = false;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 direction)
    {
        if (isKnockbackActive) return;

        isKnockbackActive = true;

        // ������������� ������� �������� (���� ����)
        rb.velocity = Vector2.zero;

        // ��������� ���� ������������
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // ��������� ������ ��� ��������� ������������
        StartCoroutine(KnockbackTimer());
    }

    private IEnumerator KnockbackTimer()
    {
        yield return new WaitForSeconds(knockbackDuration);

        // ����� ��������� ������������, ���������� ��������
        rb.velocity = Vector2.zero;
        isKnockbackActive = false;
    }
}
