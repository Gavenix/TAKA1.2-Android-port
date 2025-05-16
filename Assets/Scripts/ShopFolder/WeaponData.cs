using UnityEngine;

public enum WeaponType
{
    OneHanded,
    TwoHanded,
    Melee
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Основные данные")]
    public string weaponName;
    [TextArea]
    public string description;

    public Sprite icon;

    public WeaponType weaponType;
    public int price;
    public GameObject weaponPrefab;

    [Header("Статы оружия")]
    public float damage;
    public float fireRate;
    public float range;
}