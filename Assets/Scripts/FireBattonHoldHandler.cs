using UnityEngine;
using UnityEngine.EventSystems;

public class FireButtonHoldHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GunMAnager gunManager; // —сылка на GunMAnager

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gunManager != null)
            gunManager.SetActiveGunsFiring(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (gunManager != null)
            gunManager.SetActiveGunsFiring(false);
    }
}