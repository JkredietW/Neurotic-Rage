using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private int baseAmmo, baseHeavyAmmo;
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
    [SerializeField] private List<Image> weaponSpritesUi;
    [SerializeField] private List<GameObject> selectWeaponIndecator;
    [SerializeField] private List<GameObject> UiWeaponSlots;
    [SerializeField] private GameObject statsPanel;
    bool babyRunFix;

    [Header("Bullets")]
    public Transform bulletOrigin;
    public Rigidbody bulletPrefab;
    float total;
    public VisualEffect muzzleFlashObject;
    public Transform playerRotation;
    public TextMeshProUGUI normalAmmoText, heavyAmmoText, weaponAmmoText;
    public TextMeshProUGUI slot1WeaponAmmoText, slot2WeaponAmmoText, slotSpecialWeaponAmmo;

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
    private GameObject shop;
    PlayerShop lastShopTouched;

    [Header("ItemStats")]
    int extra_pierces;
    float extra_damage;
    float extra_attackSpeed;
    int extra_ammo;
    int extra_health;
    int extra_bullets;

    private List<TextMeshProUGUI> weaponStatsUi, itemStatsUi;
    [SerializeField] private Transform weaponStatsParent, upgradeItemStatsParent;

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
        weaponStatsUi = new List<TextMeshProUGUI>();
        itemStatsUi = new List<TextMeshProUGUI>();

        foreach (Transform child in weaponStatsParent)
        {
            weaponStatsUi.Add(child.GetComponent<TextMeshProUGUI>());
        }
        foreach (Transform child in upgradeItemStatsParent)
        {
            itemStatsUi.Add(child.GetComponent<TextMeshProUGUI>());
        }

        //check what device is being used
        if (SystemInfo.deviceType == DeviceType.Desktop)
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
        weaponInHand.GetComponent<MeshFilter>().mesh = currentWeapon.weaponMesh;
        weaponInHand.GetComponent<MeshRenderer>().material = currentWeapon.mat;
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
        if(Input.GetButton("Sprint"))
        {
            if (!isShooting)
            {
                StartStopRunning(true);
            }
        }
        else
        {
            babyRunFix = true;
            StartStopRunning(false);
        }
        if (Input.GetButtonDown("Interact")  && mayMove)
        {
            weaponsInRange.RemoveAll(item => item == null);
            if (weaponsInRange.Count > 0)
            {
                float distanceToWeeapon = Vector3.Distance(weaponsInRange[0].transform.position, transform.position);
                float distanceToShop = Vector3.Distance(shop.transform.position, transform.position);
                if (distanceToShop < distanceToWeeapon)
                {
                    OpenShop();
                }
                else
                {
                    moveDir = Vector3.zero;
                    StartStopRunning(false);

                    mayMove = false;
                    Invoke(nameof(MayMoveAgain), 1);

                    SwapWithWorldWeapon();
                    RemoveOutOfRangeWeapons();
                }
            }
            else if(lastShopTouched != null)
            {
                OpenShop();
            }
        }
        ShowStats(Input.GetButton("Tab"));
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
    void ShowStats(bool _bool)
    {
        statsPanel.SetActive(_bool);
        UpdateStats();
    }
    void UpdateStats()
    {
        //current weapon stats
        weaponStatsUi[0].text = $"Attack speed : {currentWeapon.attacksPerSecond}";
        weaponStatsUi[1].text = $"Damage : {currentWeapon.damage}";
        weaponStatsUi[2].text = $"Max ammo : {currentWeapon.maxAmmo}";
        if(currentWeapon.pierceAmount > 100)
        {
            weaponStatsUi[3].text = $"Pierces : INFINITE";
        }
        else
        {
            weaponStatsUi[3].text = $"Pierces : {currentWeapon.pierceAmount}";
        }
        weaponStatsUi[4].text = $"Bullets : {currentWeapon.projectileCount}";

        //item stats
        itemStatsUi[0].text = $"Attack speed : {extra_attackSpeed}";
        itemStatsUi[1].text = $"Damage : {extra_damage}";
        itemStatsUi[2].text = $"Ammo : {extra_ammo}";
        itemStatsUi[3].text = $"Pierces : {extra_pierces}";
        itemStatsUi[4].text = $"Bullets : {extra_bullets}";
        itemStatsUi[5].text = $"Health : {extra_health}";
    }
    public void GiveFireCooldown()
    {
        nextAttack = Time.time + 0.1f;
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
        animator.SetBool("Isrunning", false);
        babyAnimator.SetBool("IsRunning", false);
        if (moveDir.magnitude > 0.1f)
        {
            if (babyRunFix && isRunning)
            {
                babyRunFix = false;
                babyAnimator.SetTrigger("DoRunning");
            }
            animator.SetBool("Isrunning", isRunning);
            babyAnimator.SetBool("IsRunning", isRunning);
        }
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
        //set all weapon indecator off
        for (int i = 0; i < selectWeaponIndecator.Count; i++)
        {
            selectWeaponIndecator[i].SetActive(false);
        }
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
        selectWeaponIndecator[swapCurrentWeaponType].SetActive(true);
        //swap weapons
        switch (weaponsInRange[0].heldItem.type)
        {
            case weaponType.light:
                weaponSlots[0] = weaponsInRange[0].heldItem;
                if (swapCurrentWeaponType == 0)
                {
                    currentWeapon = weaponSlots[0];
                }
                DropWeapon(oldWeapon);
                UiWeaponSlots[2].SetActive(false);
                break;
            case weaponType.heavy:
                weaponSlots[1] = weaponsInRange[0].heldItem;
                if (swapCurrentWeaponType == 1)
                {
                    currentWeapon = weaponSlots[1];
                }
                DropWeapon(oldWeapon);
                UiWeaponSlots[2].SetActive(false);
                break;
            case weaponType.special:
                currentWeapon = weaponsInRange[0].heldItem;
                UiWeaponSlots[2].SetActive(true);
                break;
        }
        attackCooldown = currentWeapon.OnSwap(extra_attackSpeed);
        weaponInHand.GetComponent<MeshFilter>().mesh = currentWeapon.weaponMesh;
        if (currentWeapon.type == weaponType.special)
        {
            weaponInHand.GetComponent<MeshFilter>().mesh = null;
            GameObject specialWeapon = Instantiate(specialWeapons[currentWeapon.specialWeaponId]);
            //done like this so that scale is normal
            specialWeapon.transform.position = weaponInHand.transform.position;
            specialWeapon.transform.rotation = weaponInHand.transform.rotation;
            specialWeapon.transform.SetParent(weaponInHand.transform);

            //animations
            animator.SetInteger("SpecialStanceState", currentWeapon.specialWeaponId);
        }
        else
        {
            //animation back
            animator.SetInteger("SpecialStanceState", -1);
        }
        WorldWeapon temp = weaponsInRange[0];
        weaponsInRange.Remove(weaponsInRange[0]);
        if(temp != null)
        {
            Destroy(temp.gameObject);
        }
        SwapWeaponSprites();
        ShowTextE();
        UpdateStats();
    }
    public void ScrollWeapon()
    {
        if(!mayMove || isSwitchingWeapon || shopIsOpen)
        {
            return;
        }
        if (Input.mouseScrollDelta.y > 0.5f || Input.mouseScrollDelta.y < -0.5f)
        {
            animator.SetInteger("SpecialStanceState", -1);
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
            weaponInHand.GetComponent<MeshFilter>().mesh = currentWeapon.weaponMesh;
            weaponInHand.GetComponent<MeshRenderer>().material = currentWeapon.mat;
            Invoke(nameof(SecAfterSwapWeapon), 0.5f);
            UiWeaponSlots[2].SetActive(false);
            for (int i = 0; i < selectWeaponIndecator.Count; i++)
            {
                selectWeaponIndecator[i].SetActive(false);
            }
            selectWeaponIndecator[currentWeaponSlot].SetActive(true);
            if (weaponInHand.transform.childCount > 0)
            {
                Destroy(weaponInHand.transform.GetChild(0).gameObject);
            }
        }
        //for controller/mobile
        if(Input.GetButtonDown("RightBumber"))
        {
            isSwitchingWeapon = true;
            animator.SetTrigger("SwitchWeapon");
            babyAnimator.SetTrigger("Switch");
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
            weaponInHand.GetComponent<MeshFilter>().mesh = currentWeapon.weaponMesh;
            weaponInHand.GetComponent<MeshRenderer>().material = currentWeapon.mat;
            Invoke(nameof(SecAfterSwapWeapon), 0.5f);
            UiWeaponSlots[2].SetActive(false);
            for (int i = 0; i < selectWeaponIndecator.Count; i++)
            {
                selectWeaponIndecator[i].SetActive(false);
            }
            selectWeaponIndecator[currentWeaponSlot].SetActive(true);
            if (weaponInHand.transform.childCount > 0)
            {
                Destroy(weaponInHand.transform.GetChild(0).gameObject);
            }
        }
        SwapWeaponSprites();
        UpdateAmmoText();
        UpdateStats();
    }
    void SwapWeaponSprites()
    {
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if(weaponSpritesUi[i] != null)
            {
                weaponSpritesUi[i].sprite = weaponSlots[i].Ui_sprite;
            }
        }
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
        babyAnimator.SetLayerWeight(babyAnimator.GetLayerIndex("AnnyStates"), 0);
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
        StartStopRunning(false);
        babyAnimator.SetLayerWeight(babyAnimator.GetLayerIndex("AnnyStates"), 0);
        if (Input.GetButton("Fire1"))
        {
            lastInputWasController = false;
        }
        else if (Input.GetAxisRaw("Fire1") > 0.5f)
        {
            lastInputWasController = true;
        }
        if (Time.time >= nextAttack && mayMove)
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
        if(isReloading || isSwitchingWeapon || shopIsOpen)
        {
            yield break;
        }
        isReloading = true;
        animator.SetTrigger("Reload");
        float oldSpeed = animator.speed;
        animator.speed /= currentWeapon.reloadTime;
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        animator.speed = oldSpeed;
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
        normalAmmoText.text = $"Normal ammo : {currentAmmo} / {maxAmmo}";
        heavyAmmoText.text = $"Heavy ammo : {currentHeavyAmmo} / {maxHeavyAmmo}";
        weaponAmmoText.text = $"Weapon ammo : {currentWeapon.ammo} / {currentWeapon.maxAmmo}";

        //slots
        slot1WeaponAmmoText.text = $"Ammo : {weaponSlots[0].ammo} / {weaponSlots[0].maxAmmo}";
        slot2WeaponAmmoText.text = $"Ammo : {weaponSlots[1].ammo} / {weaponSlots[1].maxAmmo}";
        slotSpecialWeaponAmmo.text = $"Ammo : {currentWeapon.ammo} / {currentWeapon.maxAmmo}";
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
            if (!dustIsInEffect)
            {
                if(isRunning)
                {
                    StartStopRunning(true);
                }
                dustIsInEffect = true;
                moveDust.Play();
            }
            if(!isRunning)
            {
                moveDust.Stop();
            }
        }
        else
        {
            babyRunFix = true;
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
