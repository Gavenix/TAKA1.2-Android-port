using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public WeaponSlot slotData;              // Локальные данные UI-слота
    public Image icon;                       // Иконка отображения
    public PlayerInventory playerInventory;  // Ссылка на инвентарь игрока
    public int inventoryIndex;               // Индекс соответствующего слота в PlayerInventory


    public Sprite placeholderSprite;
    public void Refresh()
    {
        if (slotData.currentWeapon != null)
        {
            icon.sprite = slotData.currentWeapon.icon;
            icon.enabled = true;
            icon.preserveAspect = true; // Включаем Preserve Aspect
        }
        else
        {
            icon.sprite = placeholderSprite;
            icon.enabled = true; // Показываем плейсхолдер
            icon.preserveAspect = true; // Включаем Preserve Aspect
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        ShopItemUI draggedItem = eventData.pointerDrag?.GetComponent<ShopItemUI>();

        if (draggedItem != null && draggedItem.weaponData != null)
        {
            // Назначаем оружие в UI слот
            slotData.SetWeapon(draggedItem.weaponData);
            icon.preserveAspect = true; // Включаем Preserve Aspect
            Refresh();
            icon.preserveAspect = true; // Включаем Preserve Aspect
            // Обязательно сохраняем в реальный инвентарь игрока
            if (playerInventory != null && inventoryIndex >= 0 && inventoryIndex < playerInventory.weaponSlots.Length)
            {
                playerInventory.weaponSlots[inventoryIndex].SetWeapon(draggedItem.weaponData);
                icon.preserveAspect = true; // Включаем Preserve Aspect
            }
        }
    }
}