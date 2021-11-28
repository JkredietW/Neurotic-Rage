using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

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
    bool mayMove;
    bool isSwitchingWeapon;

    [Header("CameraStats")]
    public LayerMask aimLayer;
    public Joystick movementJoystick;
    public GameObject desktopUI, mobileUI;

    [Header("Weapons")]
    public Weapon currentWeapon;
    public List<Weapon> weaponSlots;
    float nextAttack, attackCooldown;
    int currentWeaponSlot;
    bool isReloading;
    [HideInInspector] public bool isShooting;
    bool hasMeleeAttacked;
    List<WorldWeapon> weaponsInRange;
    public WorldWeapon worldWeaponPrefab;
    public TextMeshProUGUI pickUpWeaponText;

    [Header("Bullets")]
    public Transform bulletOrigin;
    public Rigidbody bulletPrefab;
    float total;
    public VisualEffect muzzleFlashObject;
    public Transform playerRotation;
    public TextMeshProUGUI normalAmmoText, specialAmmoText, weaponAmmoText;

    [Header("MiniMap")]
    public GameObject miniMapObject;
    public GameObject miniMapCameraObject;
    public GameObject bigMapObject;
    public GameObject bigMapCameraObject;

    [Header("animations")]
    public Animator animator;
    public Animator babyAnimator;
    public GameObject swordOnBack, swordInHand;

    [Header("Shop")]
    bool shopInRange, shopIsOpen;
    public GameObject shop;
    PlayerShop lastShopTouched;

    private void Awake()
    {
        //get components
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        playerAim = GetComponentInChildren<PlayerAim>();

        //set variables
        playerAim.GetVariables();
        attackCooldown = currentWeapon.OnSwap();
        weaponsInRange = new List<WorldWeapon>();

        //check what device is being used
        if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            desktopUI.SetActive(true);
            mobileUI.SetActive(false);
        }
        else if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            desktopUI.SetActive(false);
            mobileUI.SetActive(true);
        }
    }
    private void Start()
    {
        weaponSlots[0].ammo = weaponSlots[0].maxAmmo;
        weaponSlots[1].ammo = weaponSlots[1].maxAmmo;
        UpdateAmmoText();
        mayMove = true;
        shop = FindObjectOfType<GameManager>().shopUI;
    }

    private void Update()
    {
        if (mayMove)
        {
            Movement();
        }
        DefineDirection();
        ScrollWeapon();
        ToggleMap();

        //inputs
        //attacks
        if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0.5f)
        {
            isShooting = true;
            FireWeapon();
        }
        else
        {
            isShooting = false;
        }
        //melee
        if (Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") > 0.5f)
        {
            StartCoroutine(MeleeAttack());
        }
        if(Input.GetKeyDown(KeyCode.R) && mayMove || Input.GetButtonDown("ReloadButton") && mayMove)
        {
            StartCoroutine(ReloadWeapon());
        }
        if(Input.GetButtonDown("Sprint"))
        {
            if (!isShooting)
            {
                StartStopRunning(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && mayMove || Input.GetButtonDown("AGamePadButton")  && mayMove)
        {
            if (shopInRange)
            {
                OpenShop();
            }
            else if(weaponsInRange.Count > 0)
            {
                moveDir = Vector3.zero;
                StartStopRunning(false);

                mayMove = false;
                Invoke(nameof(MayMoveAgain), 1);

                SwapWithWorldWeapon();
                RemoveOutOfRangeWeapons();
            }
            else
            {
                return;
            }
        }
    }
    private void FixedUpdate()
    {
        float extraSprintSpeed = 0;
        if(isRunning)
        {
            extraSprintSpeed = movementSpeed * 0.7f;
        }
        if (mayMove)
        {
            controller.Move((movementSpeed + extraSprintSpeed) * Time.deltaTime * moveDir.normalized);
        }
    }
    void MayMoveAgain()
    {
        mayMove = true;
    }

    public void GrantAmmo(int amount, int specialAmount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        currentSpecialAmmo = Mathf.Clamp(currentSpecialAmmo + specialAmount, 0, maxSpecialAmmo);
        UpdateAmmoText();
    }
    void StartStopRunning(bool _bool)
    {
        isRunning = _bool;
        animator.SetBool("Isrunning", isRunning);
        if(isRunning)
        {
            babyAnimator.SetTrigger("DoRunning");
        }
        babyAnimator.SetBool("IsRunning", isRunning);
    }
    void OpenShop()
    {
        shopIsOpen = !shopIsOpen;
        shop.SetActive(shopIsOpen);
        lastShopTouched.ShopOpened();
    }
    void SwapWithWorldWeapon()
    {
        if(isSwitchingWeapon)
        {
            return;
        }
        animator.SetTrigger("WeaponPickUp");
        bool swapCurrentWeapon = currentWeapon.type == weaponType.light ? true : false;
        Weapon oldWeapon = currentWeapon;
        if(weaponsInRange[0].heldItem.type == weaponType.light)
        {
            weaponSlots[0] = weaponsInRange[0].heldItem;
            if(swapCurrentWeapon)
            {
                currentWeapon = weaponSlots[0];
            }
        }
        else if(weaponsInRange[0].heldItem.type == weaponType.heavy)
        {
            weaponSlots[1] = weaponsInRange[0].heldItem;
            if(!swapCurrentWeapon)
            {
                currentWeapon = weaponSlots[1];
            }
        }
        //instatiate weapon that was held
        Instantiate(worldWeaponPrefab, bulletOrigin.position, playerAim.transform.rotation).Setup(oldWeapon);

        WorldWeapon temp = weaponsInRange[0];
        weaponsInRange.Remove(weaponsInRange[0]);
        Destroy(temp.gameObject);
        ShowTextE();
    }
    public void ScrollWeapon()
    {
        if(!mayMove || isSwitchingWeapon)
        {
            return;
        }
        if (Input.mouseScrollDelta.y > 0.5f || Input.mouseScrollDelta.y < -0.5f)
        {
            isSwitchingWeapon = true;
            animator.SetTrigger("SwitchWeapon");
            babyAnimator.SetTrigger("Switch");
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
            Invoke(nameof(SecAfterSwapWeapon), 0.5f);
        }
        if(Input.GetButtonDown("RightBumber"))
        {
            isSwitchingWeapon = true;
            animator.SetTrigger("SwitchWeapon");
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
            Invoke(nameof(SecAfterSwapWeapon), 0.5f);
        }
        UpdateAmmoText();
    }
    void SecAfterSwapWeapon()
    {
        isSwitchingWeapon = false;
    }
    public IEnumerator MeleeAttack()
    {
        if (Time.time >= nextAttack && !hasMeleeAttacked && mayMove)
        {
            StartStopRunning(false);
            swordOnBack.SetActive(false);
            swordInHand.SetActive(true);
            hasMeleeAttacked = true;
            nextAttack = Time.time + meleeAttackCooldown;
            animator.SetTrigger("MeleeAttack");
            //babyAnimator.SetTrigger("MeleeAttack");
            //damage/hitbox in animator
            yield return new WaitForSeconds(meleeAttackCooldown);
            swordOnBack.SetActive(true);
            swordInHand.SetActive(false);
            hasMeleeAttacked = false;
        }
    }
    public void FireWeapon()
    {
        if(isReloading || isSwitchingWeapon)
        {
            return;
        }
        if (Input.GetButton("Fire1"))
        {
            lastInputWasController = false;
        }
        else if (Input.GetAxisRaw("Fire1") > 0.5f)
        {
            lastInputWasController = true;
        }
        if (Time.time >= nextAttack && !isReloading && mayMove)
        {
            if(isRunning)
            {
                isRunning = false;
                playerAim.RotateToAim();
            }
            nextAttack = Time.time + attackCooldown;
            if (currentWeapon.ammo > 0)
            {
                animator.SetTrigger("Shoot");
                currentWeapon.ammo -= 1;
                UpdateAmmoText();
                muzzleFlashObject.Play();
                for (int i = 0; i < currentWeapon.projectileCount; i++)
                {
                    //how much each projectile is away from eachother
                    total = currentWeapon.shootAngle / currentWeapon.projectileCount;

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
                if(currentWeapon.ammo == 0 && mayMove)
                {
                    StartCoroutine(ReloadWeapon());
                }
            }
            else if(mayMove)
            {
                StartCoroutine(ReloadWeapon());
            }
        }
    }
    public IEnumerator ReloadWeapon()
    {
        if(isSwitchingWeapon)
        {
            yield break;
        }
        isReloading = true;
        yield return new WaitForSeconds(1);
        if (!hasMeleeAttacked || currentWeapon.ammo == currentWeapon.maxAmmo)
        {
            if (currentWeapon.type == weaponType.light)
            {
                if (currentAmmo == 0)
                {
                    print("no more ammo");
                    isReloading = false;
                    yield break;
                }
                currentAmmo -= currentWeapon.maxAmmo - currentWeapon.ammo;
                if (currentAmmo > 0)
                {
                    currentWeapon.ammo = currentWeapon.maxAmmo;
                }
                else
                {
                    currentWeapon.ammo = currentWeapon.maxAmmo + currentAmmo;
                    currentAmmo = 0;
                }
            }
            else if (currentWeapon.type == weaponType.heavy)
            {
                if (currentSpecialAmmo == 0)
                {
                    print("no more ammo");
                    yield break;
                }
                currentSpecialAmmo -= currentWeapon.maxAmmo - currentWeapon.ammo;
                if(currentSpecialAmmo > 0)
                {
                    currentWeapon.ammo = currentWeapon.maxAmmo;
                }
                else
                {
                    currentWeapon.ammo = currentWeapon.maxAmmo + currentSpecialAmmo;
                    currentSpecialAmmo = 0;
                }
            }
            isReloading = false;
        }
        else
        {
            isReloading = false;
        }
        UpdateAmmoText();
    }
    void UpdateAmmoText()
    {
        normalAmmoText.text = $"Normal ammo : " + currentAmmo.ToString() + $"/" + maxAmmo.ToString();
        specialAmmoText.text = $"Special ammo : " + currentSpecialAmmo.ToString() + $"/" + maxSpecialAmmo.ToString();
        weaponAmmoText.text = $"Weapon ammo : " + currentWeapon.ammo.ToString() + $"/" + currentWeapon.maxAmmo.ToString();
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
        if(lastInputWasController || SystemInfo.deviceType == DeviceType.Desktop)
        {
            moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }
        else
        {
            moveDir = new Vector3(movementJoystick.input.x, 0, movementJoystick.input.y);
        }
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
                StartStopRunning(false);
                moveDust.Stop();
            }
        }
        else
        {
            StartStopRunning(false);
            dustIsInEffect = false;
            moveDust.Stop();
        }
    }
    void RemoveOutOfRangeWeapons()
    {
        foreach (var item in weaponsInRange)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if(distance > 5)
            {
                weaponsInRange.Remove(item);
            }
        }
        ShowTextE();
    }
    public void InWeaponRange(WorldWeapon newWeapon)
    {
        RemoveOutOfRangeWeapons();
        //press e
        weaponsInRange.Add(newWeapon);
        ShowTextE();
    }
    public void OutOfWeaponRange(WorldWeapon oldWeapon)
    {
        RemoveOutOfRangeWeapons();
        //press e
        weaponsInRange.Remove(oldWeapon);
        ShowTextE();
    }
    public void ShopToggle(bool _bool, PlayerShop _shop)
    {
        lastShopTouched = _shop;
        shopInRange = _bool;
        ShowTextE();
    }
    void ShowTextE()
    {
        if(lastInputWasController)
        {
            pickUpWeaponText.text = "press A.";
        }
        else if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            pickUpWeaponText.text = "press E.";
        }
        else if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            //hier button tevoorschijn halen
        }


        //on/off
        if (weaponsInRange.Count > 0 || shopInRange)
        {
            pickUpWeaponText.gameObject.SetActive(true);
        }
        else
        {
            pickUpWeaponText.gameObject.SetActive(false);
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
