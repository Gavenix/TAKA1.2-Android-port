using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public WeaponSlot slotData;              // ��������� ������ UI-�����
    public Image icon;                       // ������ �����������
    public PlayerInventory playerInventory;  // ������ �� ��������� ������
    public int inventoryIndex;               // ������ ���������������� ����� � PlayerInventory


    public Sprite placeholderSprite;
    public void Refresh()
    {
        if (slotData.currentWeapon != null)
        {
            icon.sprite = slotData.currentWeapon.icon;
            icon.enabled = true;
            icon.preserveAspect = true; // �������� Preserve Aspect
        }
        else
        {
            icon.sprite = placeholderSprite;
            icon.enabled = true; // ���������� �����������
            icon.preserveAspect = true; // �������� Preserve Aspect
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        ShopItemUI draggedItem = eventData.pointerDrag?.GetComponent<ShopItemUI>();

        if (draggedItem != null && draggedItem.weaponData != null)
        {
            // ��������� ������ � UI ����
            slotData.SetWeapon(draggedItem.weaponData);
            icon.preserveAspect = true; // �������� Preserve Aspect
            Refresh();
            icon.preserveAspect = true; // �������� Preserve Aspect
            // ����������� ��������� � �������� ��������� ������
            if (playerInventory != null && inventoryIndex >= 0 && inventoryIndex < playerInventory.weaponSlots.Length)
            {
                playerInventory.weaponSlots[inventoryIndex].SetWeapon(draggedItem.weaponData);
                icon.preserveAspect = true; // �������� Preserve Aspect
            }
        }
    }
}