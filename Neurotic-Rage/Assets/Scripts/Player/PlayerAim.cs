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
    float timer;
    bool firstTime;

    public Joystick mobileAimDirection;

    public bool twoPlayers;

    public void GetVariables()
    {
        player = GetComponentInParent<PlayerMovement>();
        playerCamera = player.GiveCamera();
        aimLayer = player.GiveLayerMask();
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
            if (controllers.Length > 0)
            {
                //devine controller look rotation
                Vector3 lookRotationWithController = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * player.GetAim();
                Vector3 lookRotationWithControllerMovementBased = player.GetMoveDirection();
                print(lookRotationWithControllerMovementBased.magnitude);
                //look torwards lookrotation
                if (lookRotationWithController.magnitude != 0 && !player.isRunning)
                {
                    if (firstTime)
                    {
                        firstTime = false;
                        timer = Time.time + 0.1f;
                    }
                    lookAtDirection = lookRotationWithController;
                    transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
                    if (Time.time >= timer && lookRotationWithController.magnitude > 0.8f)
                    {
                        player.isShooting = true;
                        player.FireWeapon();
                    }
                }
                else if (lookRotationWithControllerMovementBased.magnitude > 0.1f)
                {
                    firstTime = true;
                    lookAtDirection = lookRotationWithControllerMovementBased;
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
        else if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            //mouse look rotation
            if (player.isRunning)
            {
                lookAtDirection = player.moveDir;
                lookAtDirection.y = 0;
                lookAtDirection.Normalize();
                transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
                player.GiveFireCooldown();
            }
            else
            {
                RaycastHit _hit;
                Ray _ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, aimLayer))
                {
                    if (lastMousePosition != Input.mousePosition)
                    {
                        lastMousePosition = Input.mousePosition;
                        lookAtDirection = _hit.point - player.transform.position;
                        lookAtDirection.y = 0;
                        lookAtDirection.Normalize();
                        transform.rotation = Quaternion.LookRotation(lookAtDirection);
                    }
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
                transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
                if (Time.time >= timer)
                {
                    player.isShooting = true;
                    player.FireWeapon();
                }
            }
            else if (lookRotationWithJoystickMovementBased.magnitude != 0)
            {
                firstTime = true;
                lookAtDirection = -lookRotationWithJoystickMovementBased;
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
