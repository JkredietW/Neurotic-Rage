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
    [HideInInspector]public PlayerHealth health;

    [Header("PlayerStats")]
    public float meleeAttackCooldown;
    public float movementSpeed = 1;
    public float gravity;
    public int currentAmmo, maxAmmo, currentHeavyAmmo, maxHeavyAmmo;
    public int baseAmmo, baseHeavyAmmo;
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
    [SerializeField] GameObject weaponInHand; //used for mesh
    public List<GameObject> specialWeapons;
    public Weapon currentWeapon;
    public List<Weapon> weaponSlots;
    float nextAttack, attackCooldown;
    int currentWeaponSlot;
    bool isReloading;
    [HideInInspector] public bool isShooting;
    bool hasMeleeAttacked;
    private List<WorldWeapon> weaponsInRange;
    public WorldWeapon worldWeaponPrefab;
    public TextMeshProUGUI pickUpWeaponText;

    [Header("Bullets")]
    public Transform bulletOrigin;
    public Rigidbody bulletPrefab;
    float total;
    public VisualEffect muzzleFlashObject;
    public Transform playerRotation;
    public TextMeshProUGUI normalAmmoText, heavyAmmoText, weaponAmmoText;

    //upgradeStats
    int extra_pierces;
    float extra_damage;
    float extra_attackSpeed;
    int extra_ammo;
    int extra_health;
    int extra_bullets;

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
        health = GetComponentInChildren<PlayerHealth>();

        //set variables
        playerAim.GetVariables();
        attackCooldown = currentWeapon.OnSwap(0);
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
        baseAmmo = maxAmmo;
        baseHeavyAmmo = maxHeavyAmmo;
        weaponSlots[0].ammo = weaponSlots[0].maxAmmo;
        weaponSlots[1].ammo = weaponSlots[1].maxAmmo;
        UpdateAmmoText();
        mayMove = true;
        if (FindObjectOfType<GameManager>())
        {
            shop = FindObjectOfType<GameManager>().shopUI;
        }
    }

    private void Update()
    {
        if (mayMove)
        {
            Movement();
        }
        DefineDirection();
        ScrollWeapon();

        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("ToggleMap"))
        {
            ToggleMap();
        }
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
        if(Input.GetButtonDown("ReloadButton") && mayMove)
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
        if (Input.GetButtonDown("Interact")  && mayMove)
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
    public void GiveStats(int _pierces, float _damage, float _attckSpeed, int _ammo, int _health, int _bullets)
    {
        extra_pierces = _pierces;
        extra_damage = _damage;
        extra_attackSpeed = _attckSpeed;
        extra_ammo = _ammo;
        MoreMaxAmmo();
        extra_health = _health;
        health.GainMoreMaxHealth(extra_health);
        extra_bullets = _bullets;
        //stats moeten nog op attack applied worden
    }
    public void GrantAmmo(int _amount, int _heavyAmmo)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + _amount, 0, maxAmmo);
        currentHeavyAmmo = Mathf.Clamp(currentHeavyAmmo + _heavyAmmo, 0, maxHeavyAmmo);
        UpdateAmmoText();
    }
    void MoreMaxAmmo()
    {
        maxAmmo = baseAmmo + extra_ammo;
        maxHeavyAmmo = baseAmmo + extra_ammo;
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
        Time.timeScale = shopIsOpen == true ? 0 : 1;
    }
    void SwapWithWorldWeapon()
    {
        if(isSwitchingWeapon)
        {
            return;
        }
        animator.SetTrigger("WeaponPickUp");
        int swapCurrentWeaponType = -1;
        Weapon oldWeapon = currentWeapon;
        switch (currentWeapon.type)
        {
            case weaponType.light:
                swapCurrentWeaponType = 0;
                break;
            case weaponType.heavy:
                swapCurrentWeaponType = 1;
                break;
            case weaponType.special:
                swapCurrentWeaponType = 2;
                break;
        }
        switch (weaponsInRange[0].heldItem.type)
        {
            case weaponType.light:
                weaponSlots[0] = weaponsInRange[0].heldItem;
                if (swapCurrentWeaponType == 0)
                {
                    currentWeapon = weaponSlots[0];
                }
                DropWeapon(oldWeapon);
                break;
            case weaponType.heavy:
                weaponSlots[1] = weaponsInRange[0].heldItem;
                if (swapCurrentWeaponType == 1)
                {
                    currentWeapon = weaponSlots[1];
                }
                DropWeapon(oldWeapon);
                break;
            case weaponType.special:
                currentWeapon = weaponsInRange[0].heldItem;
                currentWeapon.OnSwap(extra_attackSpeed);
                break;
        }
        weaponInHand.GetComponent<MeshFilter>().mesh = currentWeapon.weaponMesh;
        if (currentWeapon.type == weaponType.special)
        {
            weaponInHand.GetComponent<MeshFilter>().mesh = null;
            Instantiate(specialWeapons[currentWeapon.specialWeaponId], weaponInHand.transform.position, weaponInHand.transform.rotation, weaponInHand.transform);
        }
        WorldWeapon temp = weaponsInRange[0];
        weaponsInRange.Remove(weaponsInRange[0]);
        if(temp != null)
        {
            Destroy(temp.gameObject);
        }
        ShowTextE();
    }
    public void ScrollWeapon()
    {
        if(!mayMove || isSwitchingWeapon || shopIsOpen)
        {
            return;
        }
        if (Input.mouseScrollDelta.y > 0.5f || Input.mouseScrollDelta.y < -0.5f)
        {
            if (currentWeapon.type == weaponType.special)
            {
                DropWeapon(currentWeapon);
            }

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
            attackCooldown = currentWeapon.OnSwap(extra_attackSpeed);
            Invoke(nameof(SecAfterSwapWeapon), 0.5f);
            if (weaponInHand.transform.childCount > 0)
            {
                Destroy(weaponInHand.transform.GetChild(0).gameObject);
            }
            if (currentWeapon.type == weaponType.special)
            {
                weaponInHand.GetComponent<MeshFilter>().mesh = null;
                Instantiate(specialWeapons[currentWeapon.specialWeaponId], weaponInHand.transform.position, weaponInHand.transform.rotation, weaponInHand.transform);
            }
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
            attackCooldown = currentWeapon.OnSwap(extra_attackSpeed);
            Invoke(nameof(SecAfterSwapWeapon), 0.5f);
            if (weaponInHand.transform.childCount > 0)
            {
                Destroy(weaponInHand.transform.GetChild(0).gameObject);
            }
            if (currentWeapon.type == weaponType.special)
            {
                weaponInHand.GetComponent<MeshFilter>().mesh = null;
                Instantiate(specialWeapons[currentWeapon.specialWeaponId], weaponInHand.transform.position, weaponInHand.transform.rotation, weaponInHand.transform);
            }
        }
        UpdateAmmoText();
    }
    void DropWeapon(Weapon _oldWeapon)
    {
        //instatiate weapon that was held
        Instantiate(worldWeaponPrefab, bulletOrigin.position, playerAim.transform.rotation).Setup(_oldWeapon, true);
        currentWeapon.OnSwap(extra_attackSpeed);
    }
    void SecAfterSwapWeapon()
    {
        isSwitchingWeapon = false;
    }
    public IEnumerator MeleeAttack()
    {
        if(shopIsOpen || !mayMove)
        {
            yield break;
        }
        if (Time.time >= nextAttack && !hasMeleeAttacked && mayMove)
        {
            StartStopRunning(false);
            swordOnBack.SetActive(false);
            swordInHand.SetActive(true);
            hasMeleeAttacked = true;
            nextAttack = Time.time + meleeAttackCooldown;
            animator.SetTrigger("MeleeAttack");
            babyAnimator.SetTrigger("MeleeAttack");
            //damage/hitbox in animator
            yield return new WaitForSeconds(meleeAttackCooldown);
            swordOnBack.SetActive(true);
            swordInHand.SetActive(false);
            hasMeleeAttacked = false;
        }
    }
    public void FireWeapon()
    {
        if(isReloading || isSwitchingWeapon || shopIsOpen)
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
                for (int i = 0; i < currentWeapon.projectileCount + extra_bullets; i++)
                {
                    //how much each projectile is away from eachother
                    total = currentWeapon.shootAngle / currentWeapon.projectileCount + extra_bullets;

                    //get max rotation in radius
                    float value = (float)(Mathf.Atan2(playerAim.transform.rotation.y, playerAim.transform.rotation.w) / Mathf.PI) * 180;
                    if (value > 180)
                    {
                        value -= 360;
                    }
                    //set random bullet offset
                    float roll = Random.Range(-currentWeapon.rotationOffset, currentWeapon.rotationOffset);

                    //spawn bullet
                    Rigidbody spawnedBullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.Euler(new Vector3(0, value - (total * (currentWeapon.projectileCount + extra_bullets / 2)) + (total * i) + roll, 0)));
                    spawnedBullet.GetComponent<BulletBehavior>().SetUp(currentWeapon.damage + extra_damage, currentWeapon.pierceAmount + extra_pierces, playerRotation.rotation);
                    spawnedBullet.velocity = spawnedBullet.transform.TransformDirection(spawnedBullet.transform.forward) * (currentWeapon.bulletSpeed * Random.Range(0.8f, 1.2f));
                }
                if(currentWeapon.ammo == 0 && mayMove)
                {
                    if(currentWeapon.type == weaponType.special)
                    {
                        DropWeapon(currentWeapon);
                        currentWeapon = weaponSlots[currentWeaponSlot];
                    }
                    else
                    {
                        StartCoroutine(ReloadWeapon());
                    }
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
        if(isSwitchingWeapon || shopIsOpen)
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
                if (currentHeavyAmmo == 0)
                {
                    print("no more ammo");
                    yield break;
                }
                currentHeavyAmmo -= currentWeapon.maxAmmo - currentWeapon.ammo;
                if(currentHeavyAmmo > 0)
                {
                    currentWeapon.ammo = currentWeapon.maxAmmo;
                }
                else
                {
                    currentWeapon.ammo = currentWeapon.maxAmmo + currentHeavyAmmo;
                    currentHeavyAmmo = 0;
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
        heavyAmmoText.text = $"Heavy ammo : " + currentHeavyAmmo.ToString() + $"/" + maxHeavyAmmo.ToString();
        weaponAmmoText.text = $"Weapon ammo : " + currentWeapon.ammo.ToString() + $"/" + currentWeapon.maxAmmo.ToString();
    }
    void ToggleMap()
    {
        if(bigMapObject.activeSelf)
        {
            Time.timeScale = 1;
            bigMapObject.SetActive(false);
            bigMapCameraObject.SetActive(false);
            miniMapCameraObject.SetActive(true);
            miniMapObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 0;
            bigMapObject.SetActive(true);
            bigMapCameraObject.SetActive(true);
            miniMapCameraObject.SetActive(false);
            miniMapObject.SetActive(false);
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
            if(item == null)
            {
                weaponsInRange.Remove(item);
            }
            else
            {
                float distance = Vector3.Distance(transform.position, item.transform.position);
                if(distance > 5)
                {
                    weaponsInRange.Remove(item);
                }
            }
        }
        ShowTextE();
    }
    public void InWeaponRange(WorldWeapon newWeapon)
    {
        weaponsInRange.Add(newWeapon);
        ShowTextE();
    }
    public void OutOfWeaponRange(WorldWeapon oldWeapon)
    {
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
