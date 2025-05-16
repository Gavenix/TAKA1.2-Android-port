using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public WeaponSlot[] weaponSlots;
    public WeaponData startWeapon; // Ќазначь через инспектор

    private void Awake()
    {
        // ѕрисваиваем стартовое оружие в слот 1 (RightHand1)
        if (startWeapon != null && weaponSlots.Length > 1)
        {
            weaponSlots[1].SetWeapon(startWeapon);
        }
        if (weaponSlots == null || weaponSlots.Length == 0)
        {
            weaponSlots = new WeaponSlot[5];
            for (int i = 0; i < weaponSlots.Length; i++)
            {
                weaponSlots[i].Clear();
            }
        }
    }

    public bool TryAddWeapon(WeaponData weapon)
    {
        if (weapon.weaponType == WeaponType.OneHanded)
        {
            for (int i = 0; i < 4; i++)
            {
                if (weaponSlots[i].IsEmpty)
                {
                    weaponSlots[i].SetWeapon(weapon);
                    return true;
                }
            }
        }
        else if (weapon.weaponType == WeaponType.TwoHanded)
        {
            for (int i = 0;i < 3; i += 2)
            {
                if (weaponSlots[i].IsEmpty && weaponSlots[i + 1].IsEmpty)
                {
                    weaponSlots[i].SetWeapon(weapon);
                    weaponSlots[i + 1] .SetWeapon(weapon);
                    return true;
                }
            }
        }
        else if (weapon.weaponType == WeaponType.Melee)
        {
            for (int i = 4; i < 6; i++) // последние 2 слота Ч ближний бой
            {
                if (weaponSlots[i].IsEmpty)
                {
                    weaponSlots[i].SetWeapon(weapon);
                    return true;
                }
            }
        }

        return false; // нет места
    }
    public void RemoveWeaponFromSlot(int index)
    {
        if (index >= 0 && index < weaponSlots.Length)
        {
            weaponSlots[index].Clear();
        }
    }
}

