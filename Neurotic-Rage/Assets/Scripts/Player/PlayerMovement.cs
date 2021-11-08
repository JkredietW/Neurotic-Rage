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
    public int currentAmmo, maxAmmo;

    Vector3 moveDir;

    [Header("CameraStats")]
    public LayerMask aimLayer;

    [Header("Weapons")]
    public Weapon currentWeapon;
    public List<Weapon> weaponSlots;
    float nextAttack, attackCooldown;
    int currentWeaponSlot;

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
        if (Input.GetButton("Fire1") && Time.time >= nextAttack)
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
        Rigidbody spawnedBullet = Instantiate(bulletPrefab, bulletOrigin.position, playerAim.transform.rotation);
        spawnedBullet.velocity = spawnedBullet.transform.forward * currentWeapon.bulletSpeed;
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
