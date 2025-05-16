using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;
    private Color textColor;

    public void Setup(int damage)
    {
        textMesh.text = damage.ToString();
        textColor = textMesh.color;
    }

    private void Update()
    {
        transform.position += new Vector3(Random.Range(-0.2f, 0.2f), moveSpeed * Time.deltaTime, 0);
        textColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = textColor;

        if (textColor.a <= 0) Destroy(gameObject);
    }
}
