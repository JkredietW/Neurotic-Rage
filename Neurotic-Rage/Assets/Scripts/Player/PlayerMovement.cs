using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    
    [SerializeField]
    private bool tuturial;
    [Header("CameraStats")]
    public LayerMask aimLayer;
    public Joystick movementJoystick;
    public GameObject desktopUI, mobileUI;

    [Header("Weapons")]
    [SerializeField] GameObject weaponInHand; //used for mesh
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
    [SerializeField] private GameObject babyUiWeaponSlot;
    [SerializeField] private Image babyWeaponSpriteUi;
    public TextMeshProUGUI weaponAmmoTwoText;
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
    bool shopInRange;
    [HideInInspector]public bool shopIsOpen;
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

    //input nonsense
    int playerOneInputType;
    public Gamepad playerOne;
    public Gamepad playerTwo;

    //keyboard
    private Mouse mouse;
    private Keyboard keyboard;

    //stats
    float statsCooldown;
    float statsCooldownTwo;
    public float timeWhileNotShooting;
    public float timeWhileShooting;
    public float distanceWalked;

    //1
    private Vector3 aimDirection;
    bool isRunningWithController;
    //2
    private Vector3 secondAimDirection;

    //player two variables
    private bool isReloadingTwo;
    private float nextAttackTwo;
    private float attackCooldownTwo;
    [HideInInspector]public bool isShootingTwo;
    public Weapon currentWeaponTwo;
    private Transform bulletOriginTwo;
    [SerializeField] GameObject weaponInHandTwo;
    public VisualEffect muzzleFlashObjectTwo;
    private int currentWeaponSlotTwo;
    public List<Weapon> weaponSlotsTwo;

    //audio
    public AudioSource audioSourceLoop;
    public AudioClip audio_walking;
    public AudioClip audio_running;

    public Slider ammoSlider1;
    public Slider ammoSlider2;
    private bool isSwitchingWeaponTwo;

    private void Awake()
    {
        //get components
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        playerAim = GetComponentInChildren<PlayerAim>();
        health = GetComponentInChildren<PlayerHealth>();
        keyboard = Keyboard.current;
        mouse = Mouse.current;
        var allGamepads = Gamepad.all;
        if (allGamepads.Count > 0)
        {
            playerOne = allGamepads[0];
            if (playerAim.twoPlayers)
            {
                if (allGamepads.Count > 1)
                {
                    playerTwo = allGamepads[1];
                }
            }
        }

        if (!tuturial)
		{
            MayMove(false);
		}
		else
		{
            MayMove(true);
        }

        //set variables
        playerAim.GetVariables();
        attackCooldown = currentWeapon.OnSwap(0);
        attackCooldownTwo = currentWeaponTwo.OnSwap(0);
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
        //set ammo
        baseAmmo = maxAmmo;
        baseHeavyAmmo = maxHeavyAmmo;
        weaponSlots[0].ammo = weaponSlots[0].maxAmmo;
        weaponSlots[1].ammo = weaponSlots[1].maxAmmo;
        UpdateAmmoText();

        //make weapon
        playerOneInputType = PlayerPrefs.GetInt("playerinput", 0);
        GameObject weapon = Instantiate(currentWeapon.objectprefab);
        //done like this so that scale is normal
        weapon.transform.SetPositionAndRotation(weaponInHand.transform.position, weaponInHand.transform.rotation);
        weapon.transform.SetParent(weaponInHand.transform);
        weapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        animator.SetInteger("SpecialStanceState", currentWeapon.specialWeaponId);

        if (playerAim.twoPlayers)
        {
            //make weapon
            GameObject weaponTwo = Instantiate(currentWeaponTwo.objectprefab);
            //done like this so that scale is normal
            weaponTwo.transform.SetPositionAndRotation(weaponInHandTwo.transform.position, weaponInHandTwo.transform.rotation);
            weaponTwo.transform.SetParent(weaponInHandTwo.transform);
            weaponTwo.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            babyAnimator.SetInteger("WeaponState", currentWeaponTwo.specialWeaponId);
            currentWeaponTwo.ammo = currentWeaponTwo.maxAmmo;
            babyUiWeaponSlot.SetActive(true);
        }


        if (FindObjectOfType<GameManager>())
        {
            shop = FindObjectOfType<GameManager>().shopUI;
        }
        //Twoplayers();
    }

    private void Update()
    {
        if (mayMove)
        {
            //input directly put in movedir
            Movement();
        }
        DefineDirection();
        Inputs();

        if(Gamepad.all.Count > 0)
        {
            for (int i = 0; i < Gamepad.all.Count; i++)
            {
                if(Gamepad.all[i].selectButton.IsPressed() && Gamepad.all[i].startButton.IsPressed())
                {
                    if(playerTwo != null)
                    {
                        return;
                    }
                    if(Gamepad.all[i] == playerOne)
                    {
                        playerOne = null;
                        playerOneInputType = 0;
                    }
                    playerTwo = Gamepad.all[i];
                    //make weapon
                    GameObject weaponTwo = Instantiate(currentWeaponTwo.objectprefab);
                    //done like this so that scale is normal
                    weaponTwo.transform.SetPositionAndRotation(weaponInHandTwo.transform.position, weaponInHandTwo.transform.rotation);
                    weaponTwo.transform.SetParent(weaponInHandTwo.transform);
                    weaponTwo.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    babyAnimator.SetInteger("WeaponState", currentWeaponTwo.specialWeaponId);
                    currentWeaponTwo.ammo = currentWeaponTwo.maxAmmo;
                    babyUiWeaponSlot.SetActive(true);
                    playerAim.EnabledTwoPlayers();
                }
            }
        }

        //stats
        distanceWalked += moveDir.magnitude * Time.deltaTime; 
        if(isShooting)
        {
            timeWhileShooting = 100 * Time.deltaTime;
        }
        else
        {
            timeWhileNotShooting = 100 * Time.deltaTime;
        }

        //ammo sliders
        if(ammoSlider1.IsActive())
        {
            ammoSlider1.value -= 1 * Time.deltaTime;
        }
        if (ammoSlider2.IsActive())
        {
            ammoSlider2.value -= 1 * Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        float extraSprintSpeed = 0;
        if(isRunning)
        {
            audioSourceLoop.clip = audio_running;
            extraSprintSpeed = movementSpeed * 0.7f;
        }
        else
        {
            audioSourceLoop.clip = audio_walking;
        }
        if (mayMove)
        {
            controller.Move((movementSpeed + extraSprintSpeed) * Time.deltaTime * moveDir.normalized);
        }
    }
    public void Twoplayers()
    {
        playerAim.EnabledTwoPlayers();
    }
    void Inputs()
    {
        //inputs
        if(playerOneInputType == 0)
        {
            #region keyboard
            //swap weapon
            if (mouse.scroll.y.ReadValue() > 0.5f || mouse.scroll.y.ReadValue() < -0.5f)
            {
                ScrollWeapon();
            }
            //shoot 
            if (mouse.leftButton.IsPressed())
            {
                isShooting = true;
                StartCoroutine(FireWeapon());
            }
            else
            {
                isShooting = false;
            }
            //interact 
            if (keyboard.eKey.IsPressed())
            {
                if (Time.time > statsCooldown)
                {
                    statsCooldown = Time.time + 0.1f;
                    Interact();
                }
            }
            //reload 
            if (keyboard.rKey.IsPressed())
            {
                if (mayMove)
                {
                    StartCoroutine(ReloadWeapon());
                }
            }
            //sprint
            if (keyboard.leftShiftKey.IsPressed())
            {
                if (!isShooting || isRunning)
                {
                    StartStopRunning(true);
                }
            }
            else if (!isRunningWithController)
            {
                babyRunFix = true;
                StartStopRunning(false);
            }
            //toggle big map
            if (keyboard.mKey.IsPressed())
            {
                if (Time.time > statsCooldown)
                {
                    statsCooldown = Time.time + 0.1f;
                    ToggleMap();
                }
            }
            //melee
            if (mouse.rightButton.IsPressed())
            {
                StartCoroutine(MeleeAttack());
            }
            //show stats
            if (keyboard.tabKey.IsPressed())
            {
                if (Time.time > statsCooldown)
                {
                    statsCooldown = Time.time + 0.1f;
                    ShowStatsToggle(!statsPanel.activeSelf);
                }
            }
            #endregion
        }
        else if(playerOneInputType == 1)
        {
            #region controller 1
            if (playerOne != null)
            {
                //1
                //swap weapon
                if (playerOne.rightShoulder.IsPressed())
                {
                    ScrollWeapon();
                }
                //shoot 
                if (playerOne.rightTrigger.IsPressed())
                {
                    isShooting = true;
                    StartCoroutine(FireWeapon());
                }
                else
                {
                    isShooting = false;
                }
                //interact 
                if (playerOne.buttonWest.IsPressed())
                {
                    if (Time.time > statsCooldown)
                    {
                        statsCooldown = Time.time + 0.1f;
                        Interact();
                    }
                }
                //reload 
                if (playerOne.buttonEast.IsPressed())
                {
                    if (mayMove)
                    {
                        StartCoroutine(ReloadWeapon());
                    }
                }
                //sprint
                if (playerOne.leftShoulder.IsPressed())
                {
                    if (!isShooting || !isRunning)
                    {
                        isRunningWithController = true;
                        StartStopRunning(true);
                    }
                }
                else
                {
                    isRunningWithController = false;
                    babyRunFix = true;
                    StartStopRunning(false);
                }
                //toggle big map
                if (playerOne.buttonNorth.IsPressed())
                {
                    if (Time.time > statsCooldown)
                    {
                        statsCooldown = Time.time + 0.1f;
                        ToggleMap();
                    }
                }
                //melee
                if (playerOne.leftTrigger.IsPressed())
                {
                    StartCoroutine(MeleeAttack());
                }
                //show stats
                if (playerOne.selectButton.IsPressed())
                {
                    if (Time.time > statsCooldown)
                    {
                        statsCooldown = Time.time + 0.1f;
                        ShowStatsToggle(!statsPanel.activeSelf);
                    }
                }
            }
            #endregion
        }
        #region controller 2
        //player two
        if (playerTwo != null)
        {
            //reload 2
            if (playerTwo.buttonEast.IsPressed())
            {
                if (mayMove)
                {
                    if (Time.time > statsCooldownTwo)
                    {
                        statsCooldownTwo = Time.time + 0.1f;
                        StartCoroutine(ReloadWeaponTwo());
                    }
                }
            }
            //shoot 2
            if (playerTwo.rightTrigger.IsPressed())
            {
                isShootingTwo = true;
                StartCoroutine(FireWeaponTwo());
            }
            else
            {
                isShootingTwo = false;
            }
            //scroll
            if(playerTwo.rightShoulder.IsPressed())
            {
                if (Time.time > statsCooldownTwo)
                {
                    statsCooldownTwo = Time.time + 0.1f;
                    ScrollWeaponTwo();
                }
            }
        }
        #endregion
    }
    public Vector3 GetAim()
    {
        aimDirection = new Vector3(playerOne.rightStick.x.ReadValue(), 0, playerOne.rightStick.y.ReadValue());
        return aimDirection;
    }
    public Vector3 GetAimTwo()
    {
        aimDirection = new Vector3(playerTwo.rightStick.x.ReadValue(), 0, playerTwo.rightStick.y.ReadValue());
        return aimDirection;
    }
    public Vector3 GetMoveDirection()
    {
        return moveDir;
    }
    void Interact()
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
        else if (lastShopTouched != null)
        {
            OpenShop();
        }
    }
    void ShowStatsToggle(bool _bool)
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
        extra_bullets = Mathf.Clamp(extra_bullets = _bullets, 1, 10000000);
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
        if (!playerAim.twoPlayers)
        {
            babyAnimator.SetBool("IsRunning", false);
        }
        if (moveDir.magnitude > 0.1f)
        {
            if (babyRunFix && isRunning)
            {
                babyRunFix = false;
                if (!playerAim.twoPlayers)
                {
                    babyAnimator.SetTrigger("DoRunning");
                }
            }
            animator.SetBool("Isrunning", isRunning);
            if (!playerAim.twoPlayers)
            {
                babyAnimator.SetBool("IsRunning", isRunning);
            }
        }
    }
    void OpenShop()
    {
        if(!shopIsOpen)
        {
            FindObjectOfType<GameManager>().statsScript.thisgame_shopsOpened++;
            FindObjectOfType<GameManager>().statsScript.total_shopsOpened++;
        }
        shopIsOpen = !shopIsOpen;
        shop.SetActive(shopIsOpen);
        lastShopTouched.ShopOpened();
        FindObjectOfType<GameManager>().ShopIsOpened(lastShopTouched);
        Time.timeScale = shopIsOpen == true ? 0 : 1;
    }
    void SwapWithWorldWeapon()
    {
        //return when already swapping weapon
        if(isSwitchingWeapon)
        {
            return;
        }

        //set previous weapon off, no double weapons
        weaponInHand.transform.GetChild(0).gameObject.SetActive(false);

        //animation for picking up a weapon
        animator.SetTrigger("WeaponPickUp");

        //set all weapon indecator off
        for (int i = 0; i < selectWeaponIndecator.Count; i++)
        {
            selectWeaponIndecator[i].SetActive(false);
        }

        //get right indecator
        Weapon oldWeapon = currentWeapon;

        //swap weapons
        switch (weaponsInRange[0].heldItem.type)
        {
            case weaponType.light:
                selectWeaponIndecator[0].SetActive(true);
                weaponSlots[0] = weaponsInRange[0].heldItem;
                currentWeapon = weaponSlots[0];
                DropWeapon(oldWeapon);
                UiWeaponSlots[2].SetActive(false);
                break;

            case weaponType.heavy:
                selectWeaponIndecator[1].SetActive(true);
                weaponSlots[1] = weaponsInRange[0].heldItem;
                currentWeapon = weaponSlots[1];
                DropWeapon(oldWeapon);
                UiWeaponSlots[2].SetActive(false);
                break;

            case weaponType.special:
                selectWeaponIndecator[2].SetActive(true);
                currentWeapon = weaponsInRange[0].heldItem;
                UiWeaponSlots[2].SetActive(true);
                break;
        }
        attackCooldown = currentWeapon.OnSwap(extra_attackSpeed);

        //spawn in weapon
        GameObject specialWeapon = Instantiate(currentWeapon.objectprefab);
        //done like this so that scale is normal
        specialWeapon.transform.SetPositionAndRotation(weaponInHand.transform.position, weaponInHand.transform.rotation);
        specialWeapon.transform.SetParent(weaponInHand.transform);
        specialWeapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        animator.SetInteger("SpecialStanceState", currentWeapon.specialWeaponId);

        //destroy old world weapon
        WorldWeapon temp = weaponsInRange[0];
        weaponsInRange.Remove(weaponsInRange[0]);

        //ui sprite
        weaponSpritesUi[0].sprite = weaponSlots[0].Ui_sprite;
        weaponSpritesUi[1].sprite = weaponSlots[1].Ui_sprite;
        weaponSpritesUi[2].sprite = currentWeapon.Ui_sprite;

        if (temp != null)
        {
            Destroy(temp.gameObject);
        }

        //remove old weapon in hand
        if (weaponInHand.transform.childCount > 0)
        {
            Destroy(weaponInHand.transform.GetChild(0).gameObject);
        }
        ShowTextE();
        UpdateStats();
    }
    public void ScrollWeapon()
    {
        if(!mayMove || isSwitchingWeapon || shopIsOpen || isReloading)
        {
            return;
        }
        if (currentWeapon.type == weaponType.special)
        {
            Weapon oldWeapon = currentWeapon;
            currentWeapon = weaponSlots[currentWeaponSlot];
            attackCooldown = currentWeapon.OnSwap(extra_attackSpeed);

            //set previous weapon off, no double weapons
            weaponInHand.transform.GetChild(0).gameObject.SetActive(false);

            //set animation to default
            animator.SetInteger("SpecialStanceState", currentWeapon.specialWeaponId);

            //animations for switching weapon
            isSwitchingWeapon = true;
            animator.SetTrigger("SwitchWeapon");
            if (!playerAim.twoPlayers)
            {
                babyAnimator.SetTrigger("Switch");
            }
            //make weapon
            GameObject weapon = Instantiate(currentWeapon.objectprefab);
            //done like this so that scale is normal
            weapon.transform.SetPositionAndRotation(weaponInHand.transform.position, weaponInHand.transform.rotation);
            weapon.transform.SetParent(weaponInHand.transform);
            weapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            //swap cooldown
            Invoke(nameof(SecAfterSwapWeapon), 0.5f);
            UiWeaponSlots[2].SetActive(false);

            //ui sprite
            weaponSpritesUi[0].sprite = weaponSlots[0].Ui_sprite;
            weaponSpritesUi[1].sprite = weaponSlots[1].Ui_sprite;
            weaponSpritesUi[2].sprite = currentWeapon.Ui_sprite;

            //ui indecator
            for (int i = 0; i < selectWeaponIndecator.Count; i++)
            {
                selectWeaponIndecator[i].SetActive(false);
            }
            selectWeaponIndecator[currentWeaponSlot].SetActive(true);

            //remove old weapons
            if (weaponInHand.transform.childCount > 0)
            {
                Destroy(weaponInHand.transform.GetChild(0).gameObject);
            }
            UpdateAmmoText();
            UpdateStats();
            DropWeapon(oldWeapon);
            return;
        }

        //set previous weapon off, no double weapons
        weaponInHand.transform.GetChild(0).gameObject.SetActive(false);

        //animations for switching weapon
        isSwitchingWeapon = true;
        animator.SetTrigger("SwitchWeapon");
        if (!playerAim.twoPlayers)
        {
            babyAnimator.SetTrigger("Switch");
        }

        //actual switch values
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

        //set animation to default
        animator.SetInteger("SpecialStanceState", currentWeapon.specialWeaponId);

        //make weapon
        GameObject specialWeapon = Instantiate(currentWeapon.objectprefab);
        //done like this so that scale is normal
        specialWeapon.transform.SetPositionAndRotation(weaponInHand.transform.position, weaponInHand.transform.rotation);
        specialWeapon.transform.SetParent(weaponInHand.transform);
        specialWeapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        //swap cooldown
        Invoke(nameof(SecAfterSwapWeapon), 0.5f);
        UiWeaponSlots[2].SetActive(false);

        //ui sprite
        weaponSpritesUi[0].sprite = weaponSlots[0].Ui_sprite;
        weaponSpritesUi[1].sprite = weaponSlots[1].Ui_sprite;
        weaponSpritesUi[2].sprite = currentWeapon.Ui_sprite;

        //ui indecator
        for (int i = 0; i < selectWeaponIndecator.Count; i++)
        {
            selectWeaponIndecator[i].SetActive(false);
        }
        selectWeaponIndecator[currentWeaponSlot].SetActive(true);

        //remove old weapons
        if (weaponInHand.transform.childCount > 0)
        {
            Destroy(weaponInHand.transform.GetChild(0).gameObject);
        }
        UpdateAmmoText();
        UpdateStats();
    }
    public void ScrollWeaponTwo()
    {
        if (isSwitchingWeapon || shopIsOpen || isReloading)
        {
            return;
        }

        //set previous weapon off, no double weapons
        weaponInHandTwo.transform.GetChild(0).gameObject.SetActive(false);

        //animations for switching weapon
        isSwitchingWeaponTwo = true;
        //animations hier <-----
        babyAnimator.SetTrigger("SwitchWeapon");

        //actual switch values
        currentWeaponSlotTwo -= 1;
        if (currentWeaponSlotTwo > weaponSlotsTwo.Count - 1)
        {
            currentWeaponSlotTwo = 0;
        }
        if (currentWeaponSlotTwo < 0)
        {
            currentWeaponSlotTwo = weaponSlotsTwo.Count - 1;
        }
        currentWeaponTwo = weaponSlotsTwo[currentWeaponSlot];
        attackCooldownTwo = currentWeapon.OnSwap(extra_attackSpeed);
        babyAnimator.SetInteger("WeaponState", currentWeaponTwo.specialWeaponId);

        //make weapon
        GameObject specialWeapon = Instantiate(currentWeapon.objectprefab);
        //done like this so that scale is normal
        specialWeapon.transform.SetPositionAndRotation(weaponInHandTwo.transform.position, weaponInHandTwo.transform.rotation);
        specialWeapon.transform.SetParent(weaponInHandTwo.transform);
        specialWeapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        //swap cooldown
        Invoke(nameof(SecAfterSwapWeaponTwo), 0.5f);

        //ui sprite
        babyWeaponSpriteUi.sprite = currentWeaponTwo.Ui_sprite;
        //ui indecator

        //remove old weapons
        if (weaponInHandTwo.transform.childCount > 0)
        {
            Destroy(weaponInHandTwo.transform.GetChild(0).gameObject);
        }
        UpdateAmmoText();
        UpdateStats();
    }
    void DropWeapon(Weapon _oldWeapon)
    {
        //instatiate weapon that was held
        GameObject droppedWeapon = Instantiate(_oldWeapon.objectprefab, bulletOrigin.position, playerAim.transform.rotation);
        droppedWeapon.AddComponent<WorldWeapon>().Setup(_oldWeapon, true);

        currentWeapon.OnSwap(extra_attackSpeed);
    }
    void SecAfterSwapWeapon()
    {
        isSwitchingWeapon = false;
        babyAnimator.SetLayerWeight(babyAnimator.GetLayerIndex("AnnyStates"), 0);
    }
    void SecAfterSwapWeaponTwo()
    {
        isSwitchingWeaponTwo = false;
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
            isRunningWithController = false;
            if (weaponInHand.transform.childCount > 0)
            {
                weaponInHand.transform.GetChild(0).gameObject.SetActive(false);
            }
            StartStopRunning(false);
            swordOnBack.SetActive(false);
            swordInHand.SetActive(true);
            hasMeleeAttacked = true;
            nextAttack = Time.time + meleeAttackCooldown;
            animator.SetTrigger("MeleeAttack");
            if(!playerAim.twoPlayers)
            {
                babyAnimator.SetTrigger("MeleeAttack");
            }
            //damage/hitbox in animator
            yield return new WaitForSeconds(meleeAttackCooldown);
            swordOnBack.SetActive(true);
            swordInHand.SetActive(false);
            hasMeleeAttacked = false;
            if (weaponInHand.transform.childCount > 0)
            {
                weaponInHand.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
    public IEnumerator FireWeapon()
    {
        if(isReloading || isSwitchingWeapon || shopIsOpen || !mayMove)
        {
            yield break;
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
            nextAttack = Time.time + (attackCooldown * currentWeapon.burstShotAmount);
            if (currentWeapon.ammo > 0)
            {
                animator.SetTrigger("Shoot");
                animator.SetTrigger("ShootWithSpecial");
                currentWeapon.ammo -= 1;
                UpdateAmmoText();

                //set pos of muzzle for new weapon
                bulletOrigin.position = weaponInHand.transform.GetChild(0).GetComponent<Shootpos>().shootpos.position;
                muzzleFlashObject.Play();
                //stats
                FindObjectOfType<GameManager>().statsScript.thisgame_bulletsShot += currentWeapon.projectileCount + extra_bullets;
                FindObjectOfType<GameManager>().statsScript.total_bulletsShot += currentWeapon.projectileCount + extra_bullets;

                for (int b = 0; b < currentWeapon.burstShotAmount; b++)
                {
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
                        Rigidbody spawnedBullet = Instantiate(currentWeapon.Bullet, bulletOrigin.position, Quaternion.Euler(new Vector3(0, value - (total * ((currentWeapon.projectileCount + extra_bullets) / 2)) + (total * i) + roll, 0)));

                        //check for bullet speed, 50+ sometimes goes through things....
                        if (currentWeapon.bulletSpeed > 50)
                        {
                            spawnedBullet.GetComponent<BulletBehavior>().SetUp(currentWeapon.damage + extra_damage, currentWeapon.pierceAmount + extra_pierces, playerRotation.rotation, true);
                            spawnedBullet.velocity = spawnedBullet.transform.TransformDirection(spawnedBullet.transform.forward) * (currentWeapon.bulletSpeed * Random.Range(0.8f, 1.2f));

                            int pierces = currentWeapon.pierceAmount + extra_pierces;
                            RaycastHit[] hitByRaycast = Physics.RaycastAll(bulletOrigin.position, spawnedBullet.velocity);
                            bool hitEnemy = false;
                            for (int r = 0; r < hitByRaycast.Length; r++)
                            {
                                if (hitByRaycast[r].transform.GetComponent<EnemyHealth>())
                                {
                                    hitEnemy = true;
                                    pierces--;
                                    hitByRaycast[r].transform.GetComponent<EnemyHealth>().DoDamage(currentWeapon.damage + extra_damage);

                                    GameObject tempBlood = Instantiate(spawnedBullet.GetComponent<BulletBehavior>().bloodSpat, hitByRaycast[r].point, playerRotation.rotation);
                                    tempBlood.GetComponent<VisualEffect>().Play();
                                    Destroy(tempBlood, 5);

                                    //return when pierces all gone
                                    if (pierces == 0)
                                    {
                                        if (hitEnemy)
                                        {
                                            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsHit++;
                                            FindObjectOfType<GameManager>().statsScript.total_bulletsHit++;
                                        }
                                        else
                                        {
                                            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsMissed++;
                                            FindObjectOfType<GameManager>().statsScript.total_bulletsMissed++;
                                        }
                                        yield break;
                                    }
                                }
                                else //hits nothing
                                {
                                    pierces--;
                                    //return when pierces all gone
                                    if (pierces == 0)
                                    {
                                        if (hitEnemy)
                                        {
                                            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsHit++;
                                            FindObjectOfType<GameManager>().statsScript.total_bulletsHit++;
                                        }
                                        else
                                        {
                                            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsMissed++;
                                            FindObjectOfType<GameManager>().statsScript.total_bulletsMissed++;
                                        }
                                        yield break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            spawnedBullet.GetComponent<BulletBehavior>().SetUp(currentWeapon.damage + extra_damage, currentWeapon.pierceAmount + extra_pierces, playerRotation.rotation, false);
                            spawnedBullet.velocity = spawnedBullet.transform.TransformDirection(spawnedBullet.transform.forward) * (currentWeapon.bulletSpeed * Random.Range(0.8f, 1.2f));
                        }
                    }
                    if (currentWeapon.ammo == 0 && mayMove)
                    {
                        if (currentWeapon.type == weaponType.special)
                        {
                            DropWeapon(currentWeapon);
                            currentWeapon = weaponSlots[currentWeaponSlot];
                        }
                        else
                        {
                            StartCoroutine(ReloadWeapon());
                        }
                    }
                    yield return new WaitForSeconds(0.1f * currentWeapon.burstShotAmount / currentWeapon.attacksPerSecond);
                }
            }
            else if(mayMove)
            {
                StartCoroutine(ReloadWeapon());
            }
        }
    }
    public IEnumerator FireWeaponTwo()
    {
        //check if doing something
        if (isReloadingTwo)
        {
            yield break;
        }

        //shooting
        if (Time.time >= nextAttackTwo && mayMove)
        {
            if (isRunning)
            {
                isRunning = false;
                playerAim.RotateToAim();
            }
            nextAttackTwo = Time.time + (attackCooldownTwo * currentWeaponTwo.burstShotAmount);
            if (currentWeaponTwo.ammo > 0)
            {
                //shooting animation here <-----
                currentWeaponTwo.ammo -= 1;
                UpdateAmmoText();
                ///ammo for second player needs to be added

                //set pos of muzzle for new weapon
                bulletOriginTwo = weaponInHandTwo.transform.GetChild(0).GetComponent<Shootpos>().shootpos;
                muzzleFlashObjectTwo.Play();
                muzzleFlashObjectTwo.transform.position = bulletOriginTwo.position;
                muzzleFlashObjectTwo.transform.rotation = bulletOriginTwo.rotation;

                for (int b = 0; b < currentWeaponTwo.burstShotAmount; b++)
                {
                    //bullets
                    for (int i = 0; i < currentWeapon.projectileCount + extra_bullets; i++)
                    {
                        //how much each projectile is away from eachother
                        float totalTwo = currentWeaponTwo.shootAngle / currentWeaponTwo.projectileCount + extra_bullets;

                        //get max rotation in radius
                        float value = (float)(Mathf.Atan2(playerAim.babyRotation.rotation.y, playerAim.babyRotation.rotation.w) / Mathf.PI) * 180;
                        if (value > 180)
                        {
                            value -= 360;
                        }
                        //set random bullet offset
                        float roll = Random.Range(-currentWeaponTwo.rotationOffset, currentWeaponTwo.rotationOffset);

                        //spawn bullet
                        Rigidbody spawnedBullet = Instantiate(currentWeaponTwo.Bullet, bulletOriginTwo.position, Quaternion.Euler(new Vector3(0, value - (totalTwo * ((currentWeaponTwo.projectileCount + extra_bullets) / 2)) + (totalTwo * i) + roll, 0)));

                        //check for bullet speed, 50+ sometimes goes through things....
                        if (currentWeaponTwo.bulletSpeed > 50)
                        {
                            //spawns in bullet for visuals
                            spawnedBullet.GetComponent<BulletBehavior>().SetUp(currentWeaponTwo.damage + extra_damage, currentWeaponTwo.pierceAmount + extra_pierces, playerAim.babyRotation.rotation, true);
                            spawnedBullet.velocity = spawnedBullet.transform.TransformDirection(spawnedBullet.transform.forward) * (currentWeaponTwo.bulletSpeed * Random.Range(0.8f, 1.2f));

                            int pierces = currentWeaponTwo.pierceAmount + extra_pierces;
                            RaycastHit[] hitByRaycast = Physics.RaycastAll(bulletOriginTwo.position, spawnedBullet.velocity);
                            bool hitEnemy = false;
                            for (int r = 0; r < hitByRaycast.Length; r++)
                            {
                                if (hitByRaycast[r].transform.GetComponent<EnemyHealth>())
                                {
                                    hitEnemy = true;
                                    pierces--;
                                    hitByRaycast[r].transform.GetComponent<EnemyHealth>().DoDamage(currentWeaponTwo.damage + extra_damage);

                                    GameObject tempBlood = Instantiate(spawnedBullet.GetComponent<BulletBehavior>().bloodSpat, hitByRaycast[r].point, playerAim.babyRotation.rotation);
                                    tempBlood.GetComponent<VisualEffect>().Play();
                                    Destroy(tempBlood, 5);

                                    //return when pierces all gone
                                    if (pierces == 0)
                                    {
                                        if (hitEnemy)
                                        {
                                            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsHit++;
                                            FindObjectOfType<GameManager>().statsScript.total_bulletsHit++;
                                        }
                                        else
                                        {
                                            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsMissed++;
                                            FindObjectOfType<GameManager>().statsScript.total_bulletsMissed++;
                                        }
                                        yield break;
                                    }
                                }
                                else //hits nothing
                                {
                                    pierces--;
                                    //return when pierces all gone
                                    if (pierces == 0)
                                    {
                                        if (hitEnemy)
                                        {
                                            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsHit++;
                                            FindObjectOfType<GameManager>().statsScript.total_bulletsHit++;
                                        }
                                        else
                                        {
                                            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsMissed++;
                                            FindObjectOfType<GameManager>().statsScript.total_bulletsMissed++;
                                        }
                                        yield break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            spawnedBullet.GetComponent<BulletBehavior>().SetUp(currentWeaponTwo.damage + extra_damage, currentWeaponTwo.pierceAmount + extra_pierces, playerAim.babyRotation.rotation, false);
                            spawnedBullet.velocity = spawnedBullet.transform.TransformDirection(spawnedBullet.transform.forward) * (currentWeaponTwo.bulletSpeed * Random.Range(0.8f, 1.2f));
                        }

                    }
                    if (currentWeaponTwo.ammo == 0 && mayMove)
                    {
                        if (currentWeaponTwo.type == weaponType.special)
                        {
                            DropWeapon(currentWeaponTwo);
                            currentWeaponTwo = weaponSlots[currentWeaponSlot];
                        }
                        else
                        {
                            StartCoroutine(ReloadWeaponTwo());
                        }
                    }
                    yield return new WaitForSeconds(0.1f * currentWeaponTwo.burstShotAmount / currentWeaponTwo.attacksPerSecond);
                }
            }
            else if (mayMove)
            {
                StartCoroutine(ReloadWeaponTwo());
            }
        }
    }
    public IEnumerator ReloadWeapon()
    {
        if(isReloading || isSwitchingWeapon || shopIsOpen || currentWeapon.ammo >= currentWeapon.maxAmmo || currentWeapon.type == weaponType.special)
        {
            yield break;
        }
        FindObjectOfType<GameManager>().statsScript.thisgame_timesReloaded++;
        FindObjectOfType<GameManager>().statsScript.total_timesReloaded++;
        isReloading = true;
        animator.SetTrigger("Reload");
        float oldspeed = animator.GetFloat("ReloadSpeed");
        ammoSlider1.gameObject.SetActive(true);
        ammoSlider1.maxValue = currentWeapon.reloadTime;
        ammoSlider1.value = currentWeapon.reloadTime;
        animator.SetFloat("ReloadSpeed", oldspeed / currentWeapon.reloadTime);
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        ammoSlider1.gameObject.SetActive(false);
        animator.SetFloat("ReloadSpeed", oldspeed);
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
    public IEnumerator ReloadWeaponTwo()
    {
        if (isReloadingTwo || currentWeaponTwo.ammo >= currentWeaponTwo.maxAmmo)
        {
            yield break;
        }
        isReloadingTwo = true;
        //hier animation
        float oldspeed = animator.GetFloat("ReloadSpeed");
        babyAnimator.SetFloat("ReloadSpeed", oldspeed / currentWeapon.reloadTime);
        babyAnimator.SetTrigger("Reload");
        ammoSlider2.gameObject.SetActive(true);
        ammoSlider2.maxValue = currentWeaponTwo.reloadTime;
        ammoSlider2.value = currentWeaponTwo.reloadTime;
        yield return new WaitForSeconds(currentWeaponTwo.reloadTime);
        ammoSlider2.gameObject.SetActive(false);
        babyAnimator.SetFloat("ReloadSpeed", oldspeed);
        if (currentWeaponTwo.ammo == currentWeaponTwo.maxAmmo)
        {
            if (currentWeaponTwo.type == weaponType.light)
            {
                if (currentAmmo == 0)
                {
                    print("no more ammo");
                    isReloading = false;
                    yield break;
                }
                currentAmmo -= currentWeaponTwo.maxAmmo - currentWeaponTwo.ammo;
                if (currentAmmo > 0)
                {
                    currentWeaponTwo.ammo = currentWeaponTwo.maxAmmo;
                }
                else
                {
                    currentWeaponTwo.ammo = currentWeaponTwo.maxAmmo + currentAmmo;
                    currentAmmo = 0;
                }
            }
            else if (currentWeaponTwo.type == weaponType.heavy)
            {
                if (currentHeavyAmmo == 0)
                {
                    print("no more ammo");
                    yield break;
                }
                currentHeavyAmmo -= currentWeaponTwo.maxAmmo - currentWeaponTwo.ammo;
                if (currentHeavyAmmo > 0)
                {
                    currentWeaponTwo.ammo = currentWeaponTwo.maxAmmo;
                }
                else
                {
                    currentWeaponTwo.ammo = currentWeaponTwo.maxAmmo + currentHeavyAmmo;
                    currentHeavyAmmo = 0;
                }
            }
            isReloadingTwo = false;
        }
        else
        {
            isReloadingTwo = false;
        }
        UpdateAmmoTwoText();
    }
    void UpdateAmmoText()
    {
        normalAmmoText.text = $"Light ammo : {currentAmmo} / {maxAmmo}";
        heavyAmmoText.text = $"Heavy ammo : {currentHeavyAmmo} / {maxHeavyAmmo}";
        weaponAmmoText.text = $"Weapon ammo : {currentWeapon.ammo} / {currentWeapon.maxAmmo}";

        //slots
        slot1WeaponAmmoText.text = $"Ammo : {weaponSlots[0].ammo} / {weaponSlots[0].maxAmmo}";
        slot2WeaponAmmoText.text = $"Ammo : {weaponSlots[1].ammo} / {weaponSlots[1].maxAmmo}";
        slotSpecialWeaponAmmo.text = $"Ammo : {currentWeapon.ammo} / {currentWeapon.maxAmmo}";
    }
    void UpdateAmmoTwoText()
    {
        weaponAmmoTwoText.text = $"Weapon ammo : {currentWeaponTwo.ammo} / {currentWeaponTwo.maxAmmo}";
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
		//controller
		if (playerOne != null)
        {
            moveDir = new Vector3(playerOne.leftStick.x.ReadValue(), 0, playerOne.leftStick.y.ReadValue());
        }

        //keyboard
        float vertical = keyboard.wKey.ReadValue() - keyboard.sKey.ReadValue();
        float horizontal = keyboard.dKey.ReadValue() - keyboard.aKey.ReadValue();
        Vector3 keyboardmovement = new Vector3(horizontal, 0, vertical);
        if (keyboardmovement.magnitude > 0)
        {
            moveDir = new Vector3(horizontal, 0, vertical);
        }
        else if(playerOne == null)
        {
            moveDir = Vector3.zero;
        }

        //sound
        if(moveDir.magnitude > 0)
        {
            if (!audioSourceLoop.isPlaying)
            {
                audioSourceLoop.Play();
                audioSourceLoop.volume = 1;
            }
        }
        else
        {
            if (audioSourceLoop.isPlaying)
            {
                audioSourceLoop.Stop();
                audioSourceLoop.volume = 0;
            }
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
    public void MayMove(bool b)
	{
        mayMove = b;
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

    public void ChangeInput(TMP_Dropdown _value)
    {
        playerOneInputType = _value.value;
        PlayerPrefs.SetInt("playerinput", playerOneInputType);
    }
}
