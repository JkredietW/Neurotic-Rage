using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Camera playerCamera;
    PlayerAim playerAim;

    [Header("PlayerStats")]
    public float movementSpeed = 1;
    public float gravity;
    public int currentAmmo, maxAmmo;

    Vector3 moveDir;
    public VisualEffect moveDust;
    bool dustIsInEffect;

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
    float total, min, max;
    public VisualEffect muzzleFlashObject;

    [Header("MiniMap")]
    public GameObject miniMapObject;
    public GameObject miniMapCameraObject;
    public GameObject bigMapObject;
    public GameObject bigMapCameraObject;

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
        ToggleMap();
    }
    private void FixedUpdate()
    {
        controller.Move(movementSpeed * Time.deltaTime * moveDir.normalized);
    }

    public void SwapWeapon()
    {
        if(Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0)
        {
            currentWeaponSlot -= (int)Input.mouseScrollDelta.y;
            if(currentWeaponSlot > weaponSlots.Count - 1)
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
            muzzleFlashObject.Play();
            for (int i = 0; i < currentWeapon.projectileCount; i++)
            {
                //get max angle to shoot all projectiles
                min = playerAim.transform.rotation.y - currentWeapon.shootAngle;
                max = playerAim.transform.rotation.y + currentWeapon.shootAngle; //(max - min)

                //how much each projectile is away from eachother
                total = (max - min) / currentWeapon.projectileCount;

                //get max rotation in radius
                float value = (float)(Mathf.Atan2(playerAim.transform.rotation.y, playerAim.transform.rotation.w) / Mathf.PI) * 180;
                if (value > 180)
                {
                    value -= 360;
                }
                //set random bullet offset
                float roll = Random.Range(-currentWeapon.rotationOffset, currentWeapon.rotationOffset);

                //spawn bullet
                Rigidbody spawnedBullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.Euler(new Vector3(0, value - (total * (currentWeapon.projectileCount / 2)) + (total * i) + roll, 0)));
                spawnedBullet.GetComponent<BulletBehavior>().SetUp(currentWeapon.damage, currentWeapon.pierceAmount);
                spawnedBullet.velocity = spawnedBullet.transform.TransformDirection(spawnedBullet.transform.forward) * (currentWeapon.bulletSpeed * Random.Range(0.8f, 1.2f));
            }
        }
        else
        {
            ReloadWeapon();
        }
    }
    public void ReloadWeapon()
    {
        isReloading = true;
        new WaitForSeconds(currentWeapon.reloadTime);
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
    void ToggleMap()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(bigMapObject.activeSelf)
            {
                bigMapObject.SetActive(false);
                bigMapCameraObject.SetActive(false);
                miniMapCameraObject.SetActive(true);
                miniMapObject.SetActive(true);
            }
            else
            {
                bigMapObject.SetActive(true);
                bigMapCameraObject.SetActive(true);
                miniMapCameraObject.SetActive(false);
                miniMapObject.SetActive(false);
            }
        }
    }
    void Movement()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDir = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * moveDir;

        //gravity
        if(!controller.isGrounded)
        {
            moveDir.y = gravity;
        }
        else
        {
            moveDir.y = -0.01f;
        }

        //particle effect
        if(moveDir.magnitude != 0)
        {
            if (!dustIsInEffect)
            {
                dustIsInEffect = true;
                moveDust.Play();
            }
        }
        else
        {
            dustIsInEffect = false;
            moveDust.Stop();
        }
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
