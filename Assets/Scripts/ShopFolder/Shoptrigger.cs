using UnityEngine;
using TMPro;

public class Shoptrigger : MonoBehaviour
{
    public ShopManager shopManager;
    public GameObject interactionHint; // UI-текст "Нажмите E"

    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            shopManager.OpenShop();
        }
    }

    public void OpenShopByButton()
    {
        if (isPlayerInRange && shopManager != null)
        {
            shopManager.OpenShop();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionHint != null)
                interactionHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactionHint != null)
                interactionHint.SetActive(false);
        }
    }
}