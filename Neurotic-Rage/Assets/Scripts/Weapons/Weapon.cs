using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponObject", order = 1)]
[Serializable]
public class Weapon : ScriptableObject
{
    public int weaponId;
    public weaponType type;
    [Space]
    public float damage;
    public float attacksPerSecond;
    public int pierceAmount;
    public int ammo, maxAmmo;
    public float bulletSpeed;
    public int projectileCount = 1;
    public float rotationOffset;
    public float shootAngle;

    [SerializeField]
    public enum weaponType
    {
        light,
        heavy,
    }
    public float OnSwap()
    {
        float attackSpeed = attacksPerSecond / (attacksPerSecond * attacksPerSecond);
        return attackSpeed;
    }
}
