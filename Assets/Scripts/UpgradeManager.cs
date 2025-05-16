using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    [Header("Настройки")]
    public List<UpgradeData> allUpgrades;
    public GameObject upgradePanelPrefab;

    [Header("UI")]
    public List<Transform> panelSlots;
    public Button applyButton;

    [Header("Анимация")]
    public Animator animator;

    [Header("Цвета по редкости")]
    public Color commonColor;
    public Color rareColor;
    public Color legendaryColor;

    private UpgradePanelUI selectedPanel;
    private UpgradeData selectedUpgrade;

    private bool isPanelVisible = false; // Флаг для управления видимостью панели улучшений

    private void Start()
    {
        applyButton.onClick.AddListener(ApplySelectedUpgrade);
        if (animator == null)
            animator = GetComponent<Animator>();

        applyButton.interactable = false;
    }

    // Метод для показа окна улучшений
    public void ShowUpgradeOptions()
    {
        // Если окно уже показано, не показываем его снова
        if (isPanelVisible) return;
        isPanelVisible = true;

        Debug.Log(">>> Показать улучшения вызван!");

        // Отключаем кнопку "Применить" до того, как игрок выберет улучшение
        applyButton.interactable = false;
        selectedUpgrade = null;
        selectedPanel = null;

        // Очистка слотов улучшений
        foreach (Transform slot in panelSlots)
        {
            foreach (Transform child in slot)
            {
                Destroy(child.gameObject);
            }
        }

        // Выбираем 3 случайных улучшения
        List<UpgradeData> pool = new List<UpgradeData>(allUpgrades);
        for (int i = 0; i < 3 && pool.Count > 0; i++)
        {
            int index = Random.Range(0, pool.Count);
            UpgradeData upgrade = pool[index];
            pool.RemoveAt(index);

            GameObject panelGO = Instantiate(upgradePanelPrefab, panelSlots[i]);
            UpgradePanelUI panel = panelGO.GetComponent<UpgradePanelUI>();
            panel.Setup(upgrade, this);

            // Получаем компонент Button и назначаем обработчик
            Button panelButton = panelGO.GetComponent<Button>();
            if (panelButton != null)
            {
                panelButton.onClick.AddListener(panel.OnClick);
            }
        }

        // Триггерим анимацию для показа окна
        animator.ResetTrigger("Hide");
        animator.SetTrigger("Show");

         isPanelVisible = false;
    }

    // Метод для выбора улучшения
    public void SelectUpgrade(UpgradePanelUI panel, UpgradeData upgrade)
    {
        selectedPanel = panel;
        selectedUpgrade = upgrade;

        // Подсветка выбранной панели
        foreach (Transform slot in panelSlots)
        {
            UpgradePanelUI p = slot.GetComponentInChildren<UpgradePanelUI>();
            if (p != null)
                p.SetSelected(p == panel);
        }

        applyButton.interactable = true;
    }

    // Метод для применения выбранного улучшения
    public void ApplySelectedUpgrade()
    {
        if (selectedUpgrade != null)
        {
            // Применяем улучшение к игроку
            FindObjectOfType<PlayerStats>().ApplyUpgrade(selectedUpgrade);

            // Скрываем панель улучшений с анимацией
            animator.ResetTrigger("Show");
            animator.SetTrigger("Hide");

            // Сбрасываем флаг видимости
            isPanelVisible = false;
        }
    }

    // Метод для получения цвета по редкости улучшения
    public Color GetColorForRarity(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => commonColor,
            Rarity.Rare => rareColor,
            Rarity.Legendary => legendaryColor,
            _ => Color.white
        };
    }

    // Метод для обработки уровня (вызывается при получении нового уровня)
    public void OnLevelUp()
    {
        // Проверяем, чтобы окно улучшений не показывалось повторно, если оно уже открыто
        if (!isPanelVisible)
        {
            ShowUpgradeOptions();
        }
    }
    public void OnUpgradePanelHidden()
    {
        isPanelVisible = false;
    }
}