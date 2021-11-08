using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Camera playerCamera;
    PlayerAim playerAim;

    [Header("PlayerStats")]
    public float movementSpeed = 1;
        public float reloadTime;
    public int currentAmmo, maxAmmo;

    Vector3 moveDir;

    [Header("CameraStats")]
    public LayerMask aimLayer;

    [Header("Weapons")]
    public Weapon currentWeapon;
    public List<Weapon> weaponSlots;
    float nextAttack, attackCooldown;
    int currentWeaponSlot;
    bool isReloading;

    [Header("Bullets")]
    public Transform bulletOrigin;
    public Rigidbody bulletPrefab;

    private void Awake()
    {
        //get components
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        playerAim = GetComponentInChildren<PlayerAim>();

        //set variables
        playerAim.GetVariables();
        attackCooldown = currentWeapon.OnSwap();
    }

    private void Update()
    {
        Movement();
        if (Input.GetButton("Fire1") && Time.time >= nextAttack && !isReloading)
        {
            nextAttack = Time.time + attackCooldown;
            FireWeapon();
        }
        SwapWeapon();
    }
    private void FixedUpdate()
    {
        controller.Move(movementSpeed * Time.deltaTime * moveDir.normalized);
    }

    public void SwapWeapon()
    {
        if(Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0)
        {
            currentWeaponSlot += (int)Input.mouseScrollDelta.y;
            if(currentWeaponSlot > weaponSlots.Count)
            {
                currentWeaponSlot = 0;
            }
            if(currentWeaponSlot < 0)
            {
                currentWeaponSlot = weaponSlots.Count - 1;
            }
            currentWeapon = weaponSlots[currentWeaponSlot];
            attackCooldown = currentWeapon.OnSwap();
        }
    }
    public void FireWeapon()
    {
        if(currentWeapon.ammo > 0)
        {
            currentWeapon.ammo -= 1;
            Rigidbody spawnedBullet = Instantiate(bulletPrefab, bulletOrigin.position, playerAim.transform.rotation);
            spawnedBullet.velocity = spawnedBullet.transform.forward * currentWeapon.bulletSpeed;
        }
        else
        {
            ReloadWeapon();
        }
    }
    public void ReloadWeapon()
    {
        isReloading = true;
        new WaitForSeconds(reloadTime);
        if(currentAmmo == 0)
        {
            print("no more ammo");
            return;
        }
        if(currentAmmo - currentWeapon.maxAmmo > 0)
        {
            currentWeapon.ammo = currentWeapon.maxAmmo;
            currentAmmo -= currentWeapon.maxAmmo;
        }
        else
        {
            currentWeapon.ammo = currentAmmo;
            currentAmmo = 0;
        }
        isReloading = false;
    }
    public void Movement()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDir = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * moveDir;
    }
    #region return references
    public CharacterController GiveController()
    {
        return controller;
    }
    public Camera GiveCamera()
    {
        return playerCamera;
    }
    public LayerMask GiveLayerMask()
    {
        return aimLayer;
    }
    #endregion
}
