using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class ShopManager : MonoBehaviour
{
    [Header("Shop UI")]
    public GameObject shopPanel;
    public List<Transform> cardSlots;           // 3 ����� ��� ��������
    public ShopItemUI shopItemPrefab;           // ������ ����� ��������
    public TextMeshProUGUI errorMessageText;
    public Animator shopAnimator;
    public WaveSpawner waveSpawner;

    [Header("Inventory UI")]
    public Canvas canvas;
    public Transform dragLayer;

    [Header("Purchase UI")]
    public Button confirmPurchaseButton;
    public TextMeshProUGUI totalPriceText;

    [Header("Data")]
    public List<WeaponData> allItems;           // ��� ��������� ��������
    public List<InventorySlot> inventorySlots;  // UI-����� ���������

    private PlayerInventory playerInventory;
    private PlayerController playerController;
    private List<(WeaponData, InventorySlot)> selectedItems = new();
    private GunMAnager gunManager;

    private void Start()
    {
        gunManager = FindObjectOfType<GunMAnager>();
        shopPanel.SetActive(false);
        errorMessageText.gameObject.SetActive(false);
        confirmPurchaseButton.onClick.AddListener(ConfirmPurchase);


        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerInventory = playerObj.GetComponent<PlayerInventory>();
            playerController = playerObj.GetComponent<PlayerController>();
        }

        if (playerInventory == null)
            Debug.LogError("ShopManager: PlayerInventory �� ������ �� ������� � ����� Player");
        if (playerController == null)
            Debug.LogError("ShopManager: PlayerController �� ������ �� ������� � ����� Player");

        UpdateTotalPriceUI();
    }

    private void RefreshInventorySlotsUI()
    {
        if (playerInventory == null) return;

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < playerInventory.weaponSlots.Length)
            {
                inventorySlots[i].slotData = playerInventory.weaponSlots[i];
                inventorySlots[i].Refresh();
            }
            else
            {
                inventorySlots[i].slotData = null;
                inventorySlots[i].Refresh();
            }
        }
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        ClearShopSelection();


        RefreshInventorySlotsUI(); // <-- �������� ��� ������

        // ������� ������ ��������, ���� ����
        foreach (var slot in cardSlots)
        {
            foreach (Transform child in slot)
            {
                Destroy(child.gameObject);
            }
        }

        // �������� ��������� 3 ������
        var chosen = allItems.OrderBy(x => Random.value).Take(3).ToList();

        for (int i = 0; i < chosen.Count; i++)
        {
            var item = Instantiate(shopItemPrefab, cardSlots[i]);
            item.Setup(chosen[i], this);
        }
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        ClearShopSelection();
    }

    public void AddItemToPurchase(WeaponData weapon, InventorySlot slot)
    {
        if (!slot.slotData.IsEmpty)
        {
            ShowError("���� �����");
            return;
        }

        if (selectedItems.Any(s => s.Item2 == slot))
        {
            ShowError("���� ��� ������");
            return;
        }

        // ��������� ������ � ����
        slot.slotData.SetWeapon(weapon);
        slot.Refresh();

        selectedItems.Add((weapon, slot));
        UpdateTotalPriceUI();
    }

    public void ConfirmPurchase()
    {
        int totalPrice = selectedItems.Sum(item => item.Item1.price);

        if (playerController.currentCoin < totalPrice)
        {
            ShowError("������������ �����");
            return;
        }

        foreach (var (weapon, slot) in selectedItems)
        {
            slot.slotData.SetWeapon(weapon);
            slot.Refresh();
        }

        playerController.currentCoin -= totalPrice;
        playerController.UpdateCoinUI();
        selectedItems.Clear();
        UpdateTotalPriceUI();

        if (gunManager != null)
            gunManager.EquipCurrentSet();

        if (shopAnimator != null)
        {
            shopAnimator.ResetTrigger("Show");
            shopAnimator.SetTrigger("Hide");
        }
        
        CloseShop();

        // ������ ��������� ����� � ��������� 5 ������
        if (waveSpawner != null)
            StartCoroutine(StartNextWaveWithDelay(5f));
    }

    private System.Collections.IEnumerator StartNextWaveWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        waveSpawner.StartWave();
    }

    public void ClearShopSelection()
    {
        selectedItems.Clear();
        UpdateTotalPriceUI();
    }

    private void UpdateTotalPriceUI()
    {
        int total = selectedItems.Sum(item => item.Item1.price);
        totalPriceText.text = $"�����: {total}$";
    }

    private void ShowError(string message)
    {
        errorMessageText.text = message;
        errorMessageText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideError));
        Invoke(nameof(HideError), 2f);
    }

    private void HideError()
    {
        errorMessageText.gameObject.SetActive(false);
    }
}