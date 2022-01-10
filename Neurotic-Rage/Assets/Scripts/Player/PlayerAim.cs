using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    PlayerMovement player;
    Camera playerCamera;
    LayerMask aimLayer;

    public GameObject meleeHitLocation;
    public float meleeRange = 1, meleeDamage = 10;
    public GameObject bloodSpat;

    Vector3 lookAtDirection;
    Vector3 lastMousePosition;
    private bool firstTimeTwo;
    private float timerTwo;
    float timer;
    bool firstTime;

    public Joystick mobileAimDirection;

    public bool twoPlayers = false;
    public Transform babyRotation;

    Vector3 middleOfScreen;

    public void GetVariables()
    {
        player = GetComponentInParent<PlayerMovement>();
        playerCamera = player.GiveCamera();
        aimLayer = player.GiveLayerMask();
        middleOfScreen = new Vector3(Screen.width / 2, 0, Screen.height / 2);
    }
    private void Update()
    {
        RotateToAim();
        if(twoPlayers)
        {
            BabyRotateToAim();
        }
    }
    public void RotateToAim()
    {
        if(player.shopIsOpen || player.bigMapObject.activeSelf)
        {
            return;
        }
        if (player.lastInputWasController)
        {
            string[] controllers = Input.GetJoystickNames();
            if (player.playerOne != null)
            {
                if (controllers.Length > 0)
                {
                    //devine controller look rotation
                    Vector3 lookRotationWithController = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * player.GetAim();
                    Vector3 lookRotationWithControllerMovementBased = player.GetMoveDirection();
                    //look torwards lookrotation
                    if (lookRotationWithController.magnitude != 0 && !player.isRunning)
                    {
                        if (firstTime)
                        {
                            firstTime = false;
                            timer = Time.time + 0.1f;
                        }
                        lookAtDirection = lookRotationWithController;
                        if (lookAtDirection == Vector3.zero)
                        {
                            return;
                        }
                        transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
                        if (Time.time >= timer && lookRotationWithController.magnitude > 0.8f)
                        {
                            player.isShooting = true;
                            StartCoroutine(player.FireWeapon());
                        }
                    }
                    else if (lookRotationWithControllerMovementBased.magnitude > 0.1f)
                    {
                        firstTime = true;
                        lookAtDirection = lookRotationWithControllerMovementBased;
                        if (lookAtDirection == Vector3.zero)
                        {
                            return;
                        }
                        transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(transform.forward.normalized), transform.rotation, 0.5f);
                        player.isShooting = false;
                        firstTime = true;
                    }
                }
            }
        }
        else if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            //mouse look rotation
            if (player.isRunning)
            {
                lookAtDirection = player.moveDir;
                lookAtDirection.y = 0;
                lookAtDirection.Normalize();
                if (lookAtDirection == Vector3.zero)
                {
                    return;
                }
                transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
                player.GiveFireCooldown();
            }
            else
            {
                if (lastMousePosition != Input.mousePosition)
                {
                    //check if mouse moved
                    lastMousePosition = Input.mousePosition;
                    //define direction
                    lookAtDirection = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y) - middleOfScreen;
                    lookAtDirection.y = 0;
                    //add camera rotation
                    Vector3 newDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * lookAtDirection;
                    newDirection.Normalize();
                    //apply rotation
                    transform.rotation = Quaternion.LookRotation(newDirection);
                }
            }
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            Vector3 lookRotationWithJoystick = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(mobileAimDirection.input.x, 0, mobileAimDirection.input.y);
            Vector3 lookRotationWithJoystickMovementBased = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(mobileAimDirection.input.x, 0, mobileAimDirection.input.y);

            //look torwards lookrotation
            if (lookRotationWithJoystick.magnitude != 0 && !player.isRunning)
            {
                if (firstTime)
                {
                    firstTime = false;
                    timer = Time.time + 0.1f;
                }
                lookAtDirection = lookRotationWithJoystick;
                if (lookAtDirection == Vector3.zero)
                {
                    return;
                }
                transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
                if (Time.time >= timer)
                {
                    player.isShooting = true;
                    StartCoroutine(player.FireWeapon());
                }
            }
            else if (lookRotationWithJoystickMovementBased.magnitude != 0)
            {
                firstTime = true;
                lookAtDirection = -lookRotationWithJoystickMovementBased;
                if(lookAtDirection == Vector3.zero)
                {
                    return;
                }
                transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
            }
            else
            {
                player.isShooting = false;
                firstTime = true;
            }
        }
    }
    public void BabyRotateToAim()
    {
        Vector3 lookRotationWithController = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * player.GetAimTwo();
        if (firstTimeTwo)
        {
            firstTimeTwo = false;
            timerTwo = Time.time + 0.1f;
        }
        if(lookRotationWithController != Vector3.zero)
        {
            babyRotation.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookRotationWithController.normalized), babyRotation.rotation, 0.5f);
        }
        if (Time.time >= timerTwo && lookRotationWithController.magnitude > 0.8f)
        {
            player.isShootingTwo = true;
            StartCoroutine(player.FireWeaponTwo());
        }
        else
        {
            firstTimeTwo = false;
        }
    }
    public void MeleeDamageHitBox()
    {
        Collider[] hitObjects = Physics.OverlapSphere(meleeHitLocation.transform.position, meleeRange);
        foreach (var item in hitObjects)
        {
            if(item.GetComponent<EnemyHealth>())
            {
                item.GetComponent<EnemyHealth>().DoDamage(meleeDamage);
                Vector3 pointToSpawn = item.transform.position;
                GameObject tempBlood = Instantiate(bloodSpat, pointToSpawn, transform.rotation);
                tempBlood.GetComponent<VisualEffect>().Play();
            }
        }
    }
    public void EnabledTwoPlayers()
    {
        twoPlayers = true;
    }
}
