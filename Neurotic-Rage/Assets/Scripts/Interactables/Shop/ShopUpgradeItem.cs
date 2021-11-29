using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopUpgradeItem", menuName = "ScriptableObjects/ShopUpgradeItem", order = 1)]
[System.Serializable]
public class ShopUpgradeItem : ShopItem
{
    public UpgradeItemStats stats;
}
[System.Serializable]
public class UpgradeItemStats
{
    public int pierces;
    public float damage;
    public float attackSpeed;
    public int ammo;
    public int health;
}
