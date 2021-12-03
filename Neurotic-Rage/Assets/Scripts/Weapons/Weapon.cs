using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponObject", order = 1)]
[Serializable]
public class Weapon : ScriptableObject
{
    public int weaponId;
    public int specialWeaponId;
    public weaponType type;
    public Mesh weaponMesh;
    public Mesh weaponMesh2;
    [Space]
    public float damage;
    public float attacksPerSecond;
    public float reloadTime;
    public int pierceAmount;
    public int ammo, maxAmmo;
    public float bulletSpeed;
    public int projectileCount = 1;
    public float rotationOffset;
    public float shootAngle;

    public float OnSwap(float _extraAttackSpeed)
    {
        float temp = _extraAttackSpeed + attacksPerSecond;
        float attackSpeed = temp / (temp * temp);
        return attackSpeed;
    }
    private void Awake()
    {
        ammo = maxAmmo;
    }
}
[SerializeField]
public enum weaponType
{
    light,
    heavy,
    special,
}
