using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI Elements")]
    public Image icon;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statsText;
    public TextMeshProUGUI priceText;

    [Header("Card Root")]
    public RectTransform cardRoot;

    [HideInInspector] public WeaponData weaponData;
    [HideInInspector] public ShopManager shopManager;

    private RectTransform draggedIcon;
    private Canvas canvas;
    private bool isDragging = false;

    private void Awake()
    {
        // Проверяем, что ссылка на shopManager есть, и если её нет, пытаемся найти её
        if (shopManager == null)
        {
            shopManager = FindObjectOfType<ShopManager>();
        }

        // Проверяем, что shopManager и canvas правильно установлены
        if (shopManager != null)
        {
            canvas = shopManager.canvas;
        }
        else
        {
            Debug.LogError("ShopManager не найден. Убедитесь, что он существует в сцене.");
        }
    }

    public void Setup(WeaponData data, ShopManager manager)
    {
        weaponData = data;
        shopManager = manager;
        canvas = manager.canvas;

        icon.sprite = data.icon;
        icon.preserveAspect = true; // Включаем Preserve Aspect

        typeText.text = data.weaponType.ToString(); // Преобразуем enum в строку
        nameText.text = data.weaponName;
        statsText.text = $"DMG:{data.damage}  FireRate:{data.fireRate}";
        priceText.text = $"{data.price}$";

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        weaponData = null;
        gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (weaponData == null || weaponData.icon == null || canvas == null) return;
        if (draggedIcon != null) return; // Защита от дублирования

        isDragging = true;

        GameObject iconObj = new GameObject("DraggedIcon");
        iconObj.transform.SetParent(canvas.transform, false);
        draggedIcon = iconObj.AddComponent<RectTransform>();
        icon.preserveAspect = true; // Включаем Preserve Aspect
        draggedIcon.sizeDelta = new Vector2(80, 80);

        Image img = iconObj.AddComponent<Image>();
        img.sprite = weaponData.icon;
        img.raycastTarget = false;

        UpdateDraggedIconPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedIcon != null)
        {
            UpdateDraggedIconPosition(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (draggedIcon != null)
        {
            Destroy(draggedIcon.gameObject);
            draggedIcon = null;
        }

        bool droppedOnSlot = false;

        if (eventData.pointerEnter != null)
        {
            InventorySlot slot = eventData.pointerEnter.GetComponentInParent<InventorySlot>();
            if (slot != null)
            {
                shopManager.AddItemToPurchase(weaponData, slot);
                droppedOnSlot = true;
            }
        }

        if (droppedOnSlot)
        {
            gameObject.SetActive(false); // Прячем карточку только если успешно помещена
        }
    }

    private void UpdateDraggedIconPosition(PointerEventData eventData)
    {
        if (canvas != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out Vector2 localPoint
            );
            draggedIcon.localPosition = localPoint;
            icon.preserveAspect = true; // Включаем Preserve Aspect
        }
    }
}