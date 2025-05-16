using UnityEngine;

[System.Serializable]
public class WeaponSlot
{
    public enum SlotType
    {
        LeftHand1,
        RightHand1,
        LeftHand2,
        RightHand2,
        Melee1,
    }

    public SlotType slotType;
    public WeaponData currentWeapon;

    public bool IsEmpty => currentWeapon != null;

    public void SetWeapon(WeaponData weapon)
    {
        currentWeapon = weapon;
    }


    public void Clear()
    {
        currentWeapon = null;
    }
}