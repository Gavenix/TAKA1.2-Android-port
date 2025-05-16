using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanelUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public Image background;

    private UpgradeData data;
    private UpgradeManager manager;

    private Button panelButton;  // ������� ������ ��� ��������� ������

    private void Awake()
    {
        panelButton = GetComponent<Button>();  // �������� ��������� Button
        if (panelButton != null)
        {
            panelButton.onClick.AddListener(OnClick);  // ��������� ��������� ������� �����
        }
    }

    public void Setup(UpgradeData upgrade, UpgradeManager upgradeManager)
    {
        data = upgrade;
        manager = upgradeManager;

        iconImage.sprite = data.icon;
        nameText.text = data.upgradeName;
        descriptionText.text = data.description;
        background.color = manager.GetColorForRarity(data.rarity);
    }

    public void OnClick()
    {
        manager.SelectUpgrade(this, data);  // �������� ����� ������ ���������
    }

    public void SetSelected(bool selected)
    {
        background.color = selected ? Color.yellow : manager.GetColorForRarity(data.rarity);
    }
}