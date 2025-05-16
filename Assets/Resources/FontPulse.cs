using TMPro;
using UnityEngine;

public class FontPulse : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Color startColor = Color.black;
    public Color endColor = Color.red;
    public float speed = 2f;

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f; // �� 0 �� 1
        text.color = Color.Lerp(startColor, endColor, t);
    }
}
