using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private Image manaBar;
    [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField] private Image xpBarImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image healthBarImage;

    [SerializeField] private int playerLevel = 1;

    [SerializeField] private UpgradeManager upgradeManager;

    [Header("Weapon Hands")]
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;

    [Header("Shop")]
    public ShopManager shopManager;

    public Joystick joystick;

    private Animator anim;
    private Rigidbody2D rb;

    public float speed = 5f;
    private Vector2 movement;
    private bool dead = false;

    private int maxHealth = 100;
    private int currentHealth;

    private int facingDirection = 1;
    private Vector3 originalScale;

    public float maxMana = 100f;
    public float currentMana;

    public float currentCoin;

    public float hpBonus = 0;
    public float meleeDamage = 0f;
    public float bulletDamage = 0f;
    public GunMAnager gunManager;
    public DashSkill dashSkill;

    public float moveSpeed = 0f;
    public float manaRegenBonus = 0f;

    private PlayerStats stats;

    private float currentXP = 0f;
    private float xpToNextLevel = 100f;
    private int currentLevel = 1;
    [SerializeField] private float baseXPToLevelUp = 100f;
    [SerializeField] private float xpGrowthFactor = 1.2f;

    [SerializeField] private GameObject dustPrefab;
    [SerializeField] private Transform feetPosition;
    private float lastDustTime;
    private float dustInterval = 0.1f;
    [SerializeField] private float speedThreshold = 0.1f;

    public bool pendingLevelUp = false;
    private bool isPlayerInRange = false;

    private void Start()
    {
        if(dashSkill == null)
            dashSkill = GetComponent<DashSkill>();
  
        if (gunManager == null)
            gunManager = FindObjectOfType<GunMAnager>();

        stats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth + Mathf.RoundToInt(stats.HPbonus);
        currentMana = maxMana;
        currentCoin = 0;

        originalScale = transform.localScale;

        UpdateManaUI();
        UpdateXPUI();
        UpdateHealthUI();
    }

    void Update()
    {
        if (dead)
        {
            movement = Vector2.zero;
            anim.SetFloat("velocity", 0);
            return;
        }

        // Получаем ввод с клавиатуры
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Получаем ввод с джойстика (если он есть)
        Vector2 joystickInput = joystick != null ? new Vector2(joystick.Horizontal, joystick.Vertical) : Vector2.zero;

        // Если есть движение с джойстика — используем его, иначе клавиатуру
        movement = joystickInput.magnitude > 0.1f ? joystickInput : keyboardInput;

        movement = movement.normalized;
        anim.SetFloat("velocity", movement.magnitude);

        // Определение isMovingBackwards
        bool isBackwards = (movement.x < 0 && facingDirection == -1) || (movement.x > 0 && facingDirection == 1);
        anim.SetBool("isMovingBackwards", isBackwards);

        if (movement.magnitude > speedThreshold && Time.time - lastDustTime > dustInterval)
        {
            SpawnDust();
            lastDustTime = Time.time;
        }

        if (movement.x > 0 && facingDirection == 1)
        {
            Flip();
        }
        else if (movement.x < 0 && facingDirection == -1)
        {
            Flip();
        }
    }
    
    

    public void OnDashButtonDown()
    {
        if (dashSkill != null)
            dashSkill.TryDash();
    }

    
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * stats.moveSpeed * Time.fixedDeltaTime);
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(originalScale.x * facingDirection, originalScale.y, originalScale.z);
    }

    public void LookAtDirection(Vector2 dir)
    {
        if (dir.x > 0 && facingDirection == 1)
        {
            Flip();
        }
        else if (dir.x < 0 && facingDirection == -1)
        {
            Flip();
        }
    }

    private void SpawnDust()
    {
        if (dustPrefab != null && feetPosition != null)
        {
            GameObject dustInstance = Instantiate(dustPrefab, feetPosition.position, Quaternion.identity);
            ParticleSystem dustParticles = dustInstance.GetComponent<ParticleSystem>();

            if (dustParticles != null)
            {
                dustParticles.Play();
                Destroy(dustInstance, 1f);
            }
        }
    }

    public void UpdateCoins(float amount)
    {
        currentCoin += amount;
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = ":" + currentCoin;
        }
    }

    public void UpdateMana(float amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);

        AddXP(amount);
        UpdateManaUI();
    }

    private void UpdateManaUI()
    {
        if (manaText != null)
        {
            manaText.text = currentMana + "/" + maxMana;
        }

        if (manaBar != null)
        {
            float manaPercentage = currentMana / maxMana;
            manaBar.fillAmount = manaPercentage;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    public float GetCurrentMana()
    {
        return currentMana;
    }

    public void RecalculateHealth()
    {
        int newMaxHealth = maxHealth + Mathf.RoundToInt(stats.HPbonus);
        int missingHealth = maxHealth - currentHealth;
        maxHealth = newMaxHealth;
        currentHealth = maxHealth - missingHealth;

        Debug.Log($"[HP] New MaxHealth: {maxHealth}, Current: {currentHealth}, Bonus: {stats.HPbonus}");
        UpdateHealthUI();
    }

    public Vector2 GetMovementDirection()
    {
        return movement;
    }

    void Hit(int damage)
    {
        anim.SetTrigger("Hit");
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthUI();
    }

    void Die()
    {
        dead = true;
        anim.SetTrigger("Die");
    }

    public void AddXP(float amount)
    {
        if (amount <= 0f) return;

        currentXP += amount;

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        UpdateXPUI();
    }

    private void LevelUp()
    {
        currentLevel++;
        xpToNextLevel = Mathf.Round(baseXPToLevelUp * Mathf.Pow(xpGrowthFactor, currentLevel - 1));
        Debug.Log("Level Up! Now Level: " + currentLevel);

        pendingLevelUp = true;
    }

    private void UpdateXPUI()
    {
        if (xpBarImage != null)
        {
            xpBarImage.fillAmount = currentXP / xpToNextLevel;
        }

        if (levelText != null)
        {
            levelText.text = "Lv." + currentLevel;
        }
    }
    public int GetFacingDirection()
    {
        return facingDirection;
    }
    public int GetMaxHealth() => maxHealth;
    public int GetCurrentHealth() => currentHealth;

}