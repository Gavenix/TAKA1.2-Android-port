using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponAutoAim : MonoBehaviour
{
    public float range = 5f;
    public string enemyTag = "Enemy";
    private Transform target;

    private void Update()
    {
        FindNearestEnemy();
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= range)
            {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        target = nearestEnemy;
    }
}

   