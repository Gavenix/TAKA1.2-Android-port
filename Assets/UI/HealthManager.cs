using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public TextMeshProUGUI healthText;
    public Image HPbar;

    public TextMeshProUGUI damageText;

    private bool isInvulnerable = false;

    public Animator cameraAnimator;
    public DoomguyFace doomguyFace;

    private void Start()
    {
        currentHealth = maxHealth;
        damageText.gameObject.SetActive(false);
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (cameraAnimator != null)
        {
            cameraAnimator.Play("CameraShake", -1, 0f);
        }
        if (doomguyFace != null)
        {
            doomguyFace.ShowHurtFace();
        }

        ShowDamagePopup(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    public void SetInvulnerable(bool state)
    {
        isInvulnerable = state;
    }

    public void ApplyHPBonus(float bonus)
    {
        int bonusInt = Mathf.RoundToInt(bonus);
        maxHealth += bonusInt;
        currentHealth += bonusInt;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"HP Bonus applied: +{bonusInt}, New Max HP: {maxHealth}");
        UpdateHealthUI();
    }

    private void ShowDamagePopup(int damage)
    {
        damageText.text = damage.ToString();
        damageText.color = new Color(1, 0, 0, 1);
        damageText.transform.position = transform.position + Vector3.up * 1.5f;
        damageText.gameObject.SetActive(true);

        StartCoroutine(FadeOutText());
    }

    private System.Collections.IEnumerator FadeOutText()
    {
        float duration = 1f;
        float elapsed = 0f;
        Color textColor = damageText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            textColor.a = Mathf.Lerp(1, 0, elapsed / duration);
            damageText.color = textColor;
            damageText.transform.position += Vector3.up * Time.deltaTime;
            yield return null;
        }

        damageText.gameObject.SetActive(false);
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth + "/" + maxHealth;
        }

        if (HPbar != null)
        {
            HPbar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Игрок умер!");
    }
}



