using UnityEngine;

public class WeaponOrbit : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 1.5f;
    public float orbitSpeed = 100f;

    private float angle = 0f;

    private void Update()
    {
        if (player == null) return;

        angle += orbitSpeed * Time.deltaTime;

        float radians = angle * Mathf.Deg2Rad;

        Vector2 offset = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * orbitRadius;
        transform.position = (Vector2)player.position + offset;

        transform.right = (transform.position - player.position).normalized;

    }
}
