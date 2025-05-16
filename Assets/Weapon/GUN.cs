using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject muzzle;
    [SerializeField] Transform muzzlePosition;
    [SerializeField] GameObject projectile;
    [SerializeField] private Animator animator;

    [Header("Config")]
    [SerializeField] float fireDistance = 10;
    [SerializeField] float fireRate = 0.05f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float spreadAngle = 10f;

    private Transform player;
    private Vector2 offset;
    private float timeSinceLastShot = 0.0f;
    private Transform closestEnemy;
    private Transform previouslyHighlightedEnemy;
    private Vector2 lastMoveDirection = Vector2.right;
    private PlayerController playerMovement;
    private PlayerStats playerStats;

    private void Start()
    {
        timeSinceLastShot = fireRate;

        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerController>();
        playerStats = player.GetComponent<PlayerStats>();
    }

    private bool isFiring = false;

    public void SetFiring(bool firing)
    {
        isFiring = firing;
    }

    private void Update()
    {
        transform.position = (Vector2)player.position + offset;

        Vector2 moveDirection = playerMovement.GetMovementDirection();
        if (moveDirection.magnitude > 0.1f)
        {
            lastMoveDirection = moveDirection.normalized;
        }

        // "ИЛИ-ИЛИ": если зажата хотя бы одна из кнопок
        bool shouldFire = isFiring || Input.GetKey(KeyCode.Space);

        timeSinceLastShot += Time.deltaTime;
        float finalFireRate = Mathf.Max(0.01f, fireRate - playerStats.fireRateBonus);

        if (shouldFire && timeSinceLastShot >= finalFireRate)
        {
            Shoot();
            timeSinceLastShot = 0.0f;
        }

        FindClosestEnemy();
        AimWeapon();

    }

    void FindClosestEnemy()
    {
        closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance < fireDistance)
            {
                if (!IsEnemyBlocked(enemy.transform))
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }

        HighlightEnemy(closestEnemy);
    }

    bool IsEnemyBlocked(Transform enemy)
    {
        Vector2 direction = (enemy.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, enemy.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleLayer);
        return hit.collider != null;
    }

    void AimWeapon()
    {
        Vector3 direction;

        if (closestEnemy != null)
            direction = closestEnemy.position - transform.position;
        else
            direction = lastMoveDirection;

        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
        if (playerMovement != null)
        {
            // Если враг справа, dir.x > 0, если слева, dir.x < 0
            playerMovement.LookAtDirection(direction);
        }

        var sr = GetComponent<SpriteRenderer>();
        bool flip = Mathf.Abs(angle) > 90f;
        if (sr != null)
            sr.flipY = flip;

        // Корректируем muzzlePosition по flipY
        if (muzzlePosition != null)
        {
            Vector3 local = muzzlePosition.localPosition;
            local.y = Mathf.Abs(local.y) * (flip ? -1 : 1);
            muzzlePosition.localPosition = local;
        }
    }

    public void TryAutoFire()
    {
        timeSinceLastShot += Time.deltaTime;
        float finalFireRate = Mathf.Max(0.01f, fireRate - playerStats.fireRateBonus);

        if (timeSinceLastShot >= finalFireRate)
        {
            Shoot();
            timeSinceLastShot = 0.0f;
        }
    }
    public void Shooting()
    {
        timeSinceLastShot += Time.deltaTime;
        float finalFireRate = Mathf.Max(0.01f, fireRate - playerStats.fireRateBonus);

        if (Input.GetKey(KeyCode.Space) && timeSinceLastShot >= finalFireRate)
        {
            Shoot();
            timeSinceLastShot = 0.0f;
        }
    }

    public void Shoot()
    {
        float randomOffset = Random.Range(-spreadAngle, spreadAngle);
        Quaternion bulletRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + randomOffset);

        var muzzleGo = Instantiate(muzzle, muzzlePosition.position, bulletRotation);
        Destroy(muzzleGo, 0.05f);

        var projectileGo = Instantiate(projectile, muzzlePosition.position, bulletRotation);
        Rigidbody2D rb = projectileGo.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            float baseDamage = 50f;
            float bonusDamage = playerStats.bulletDamage;
            float totalDamage = baseDamage + bonusDamage;

            Projectile projectileScript = projectileGo.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDamage(totalDamage);
            }
        }

        Destroy(projectileGo, 3);

        if (animator != null)
            animator.SetTrigger("Recoil");
    }

    void HighlightEnemy(Transform enemy)
    {
        if (previouslyHighlightedEnemy != null && previouslyHighlightedEnemy != enemy)
        {
            var prevHighlighter = previouslyHighlightedEnemy.GetComponent<TargetHighlighter>();
            if (prevHighlighter != null)
                prevHighlighter.SetHighlighted(false);
        }

        if (enemy != null)
        {
            var highlighter = enemy.GetComponent<TargetHighlighter>();
            if (highlighter != null)
                highlighter.SetHighlighted(true);
        }

        previouslyHighlightedEnemy = enemy;
    }

    public void SetOffset(Vector2 o)
    {
        offset = o;
    }
    public void OnFireButtonDown()
    {
        Shoot();
    }
}