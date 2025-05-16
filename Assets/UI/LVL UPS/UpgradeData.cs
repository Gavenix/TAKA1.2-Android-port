using UnityEngine;

public enum Rarity { Common, Rare, Legendary }
public enum UpgradeType { MeleeDamage, MoveSpeed, BulletDamage, FireRate, HPbonus }

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public Sprite icon;
    public Rarity rarity;
    public UpgradeType type;
    public float value; // Например, +1, +2, +3 к урону и т.д.
}
