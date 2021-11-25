using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopHealth", menuName = "ScriptableObjects/ShopHealth", order = 1)]
[System.Serializable]
public class ShopHealth : ShopItem
{
    public int healthAmount;
}
