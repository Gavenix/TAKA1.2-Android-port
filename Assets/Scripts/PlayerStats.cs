using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float meleeDamage = 1f;
    public float bulletDamage = 1f;
    public float moveSpeed = 5f;
    public float HPbonus = 0f;
    public float fireRateBonus = 0f;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void ApplyUpgrade(UpgradeData upgrade)
    {
        switch (upgrade.type)
        {
            case UpgradeType.FireRate:
                fireRateBonus += upgrade.value;
                break;
            case UpgradeType.MeleeDamage:
                meleeDamage += upgrade.value;
                break;
            case UpgradeType.MoveSpeed:
                moveSpeed += upgrade.value;
                break;
            case UpgradeType.BulletDamage:
                bulletDamage += upgrade.value;
                break;
            case UpgradeType.HPbonus:
                HPbonus += upgrade.value;
                HealthManager healthManager = FindObjectOfType<HealthManager>();
                if (healthManager != null)
                {
                    healthManager.ApplyHPBonus(upgrade.value);
                }
                break;
                // добавь свои апгрейды
        }
    }
}
