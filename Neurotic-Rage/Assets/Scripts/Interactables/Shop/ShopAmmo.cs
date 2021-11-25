using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopAmmo", menuName = "ScriptableObjects/ShopAmmo", order = 1)]
[System.Serializable]
public class ShopAmmo : ShopItem
{
    public int normalAmmoAmount;
    public int specialAmmoAmount;
}
