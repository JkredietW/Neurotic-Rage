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
    public float meleeAttackCooldown;
    public float movementSpeed = 1;
    public float gravity;
    public int currentAmmo, maxAmmo, currentSpecialAmmo, maxSpecialAmmo;
    [HideInInspector] public bool lastInputWasController;
    [HideInInspector] public bool isRunning;

    [HideInInspector] public Vector3 moveDir;
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
    bool hasMeleeAttacked;

    [Header("Bullets")]
    public Transform bulletOrigin;
    public Rigidbody bulletPrefab;
    float total, min, max;
    public VisualEffect muzzleFlashObject;
    public Transform playerRotation;

    [Header("MiniMap")]
    public GameObject miniMapObject;
    public GameObject miniMapCameraObject;
    public GameObject bigMapObject;
    public GameObject bigMapCameraObject;

    [Header("animations")]
    public Animator animator;

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
        DefineDirection();
        SwapWeapon();
        ToggleMap();

        //inputs
        //attacks
        if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0.5f)
        {
            FireWeapon();
        }
        //melee
        if (Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0.5f)
        {
            MeleeAttack();
        }
        if(Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("ReloadButton"))
        {
            ReloadWeapon();
        }
        if(Input.GetButtonDown("Sprint"))
        {
            isRunning = true;
            animator.SetBool("Isrunning", isRunning);
        }
    }
    private void FixedUpdate()
    {
        float extraSprintSpeed = 0;
        if(isRunning)
        {
            extraSprintSpeed = movementSpeed * 0.7f;
        }
        controller.Move((movementSpeed + extraSprintSpeed) * Time.deltaTime * moveDir.normalized);
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
        if(Input.GetButtonDown("RightBumber"))
        {
            currentWeaponSlot -= 1;
            if (currentWeaponSlot > weaponSlots.Count - 1)
            {
                currentWeaponSlot = 0;
            }
            if (currentWeaponSlot < 0)
            {
                currentWeaponSlot = weaponSlots.Count - 1;
            }
            currentWeapon = weaponSlots[currentWeaponSlot];
            attackCooldown = currentWeapon.OnSwap();
        }
    }
    public void MeleeAttack()
    {
        if (Time.time >= nextAttack)
        {
            isRunning = false;
            animator.SetBool("Isrunning", isRunning);
            hasMeleeAttacked = true;
            nextAttack = Time.time + meleeAttackCooldown;
            //hier attack doen

            new WaitForSeconds(meleeAttackCooldown);
            hasMeleeAttacked = false;
        }
    }
    public void FireWeapon()
    {
        if (Time.time >= nextAttack && !isReloading)
        {
            if(isRunning)
            {
                isRunning = false;
                playerAim.RotateToAim();
            }
            nextAttack = Time.time + attackCooldown;
            if (currentWeapon.ammo > 0)
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
                    spawnedBullet.GetComponent<BulletBehavior>().SetUp(currentWeapon.damage, currentWeapon.pierceAmount, playerRotation.rotation);
                    spawnedBullet.velocity = spawnedBullet.transform.TransformDirection(spawnedBullet.transform.forward) * (currentWeapon.bulletSpeed * Random.Range(0.8f, 1.2f));
                }
            }
            else
            {
                ReloadWeapon();
            }
        }
    }
    public void ReloadWeapon()
    {
        isReloading = true;
        new WaitForSeconds(currentWeapon.reloadTime);
        if (!hasMeleeAttacked)
        {
            if (currentWeapon.type == Weapon.weaponType.light)
            {
                if (currentAmmo == 0)
                {
                    print("no more ammo");
                    return;
                }
                if (currentAmmo - currentWeapon.maxAmmo > 0)
                {
                    currentWeapon.ammo = currentWeapon.maxAmmo;
                    currentAmmo -= currentWeapon.maxAmmo;
                }
                else
                {
                    currentWeapon.ammo = currentAmmo;
                    currentAmmo = 0;
                }
            }
            else if (currentWeapon.type == Weapon.weaponType.heavy)
            {
                if (currentSpecialAmmo == 0)
                {
                    print("no more ammo");
                    return;
                }
                if (currentSpecialAmmo - currentWeapon.maxAmmo > 0)
                {
                    currentWeapon.ammo = currentWeapon.maxAmmo;
                    currentSpecialAmmo -= currentWeapon.maxAmmo;
                }
                else
                {
                    currentWeapon.ammo = currentSpecialAmmo;
                    currentSpecialAmmo = 0;
                }
            }
            isReloading = false;
        }
        else
        {
            isReloading = false;
        }
    }
    void ToggleMap()
    {
        if(Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("YGamePadButton"))
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
    //for walk animation
    void DefineDirection()
    {
        Transform forwardDirection = animator.transform;
        float velocityZ = Vector3.Dot(moveDir.normalized, forwardDirection.forward);
        float velocityX = Vector3.Dot(moveDir.normalized, forwardDirection.right);
        animator.SetFloat("LY", velocityZ);
        animator.SetFloat("LX", velocityX);
    }
    void Movement()
    {
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDir = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * moveDir;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            lastInputWasController = false;
        }
        if(Input.GetAxisRaw("VerticalController") != 0 || Input.GetAxisRaw("HorizontalController") != 0)
        {
            lastInputWasController = true;
        }

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
        if (moveDir.magnitude > 0.1f)
        {
            //dust
            if (!dustIsInEffect && isRunning)
            {
                dustIsInEffect = true;
                moveDust.Play();
            }
            if(!isRunning)
            {
                isRunning = false;
                animator.SetBool("Isrunning", isRunning);
                moveDust.Stop();
            }
        }
        else
        {
            isRunning = false;
            animator.SetBool("Isrunning", isRunning);
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
