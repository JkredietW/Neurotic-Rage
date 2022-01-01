using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponObject", order = 1)]
[Serializable]
public class Weapon : ScriptableObject
{
    public int weaponId;
    public int specialWeaponId = -1;
    public weaponType type;
    public GameObject objectprefab;
    public Sprite Ui_sprite;
    public List<Rigidbody> Bullet;
    public AudioClip shootSound, reloadSound;
    [Space]
    public float damage;
    public float attacksPerSecond;
    public float reloadTime = 1;
    public int pierceAmount;
    public int ammo, maxAmmo;
    public float bulletSpeed;
    public int projectileCount = 1;
    public float rotationOffset;
    public float shootAngle;
    public int burstShotAmount = 1;

    public float OnSwap(float _extraAttackSpeed)
    {
        float temp = Mathf.Clamp(temp = _extraAttackSpeed + attacksPerSecond, 0.1f, Mathf.Infinity);
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
