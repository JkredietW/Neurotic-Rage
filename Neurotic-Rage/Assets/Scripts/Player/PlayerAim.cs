using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    PlayerMovement player;
    Camera playerCamera;
    LayerMask aimLayer;

    Vector3 lookAtDirection;
    Vector3 lastMousePosition;
    float timer;
    bool firstTime;
    public void GetVariables()
    {
        player = GetComponentInParent<PlayerMovement>();
        playerCamera = player.GiveCamera();
        aimLayer = player.GiveLayerMask();
    }
    private void Update()
    {
        if (player.lastInputWasController)
        {
            string[] controllers = Input.GetJoystickNames();
            if (controllers.Length > 0)
            {
                Vector3 lookRotationWithController = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(Input.GetAxis("HorizontalTurn"), 0, -Input.GetAxis("VerticalTurn"));
                Vector3 lookRotationWithControllerMovementBased = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
                if (lookRotationWithController.magnitude != 0)
                {
                    if (firstTime)
                    {
                        firstTime = false;
                        timer = Time.time + 0.1f;
                    }
                    lookAtDirection = lookRotationWithController;
                    transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
                    if (Time.time >= timer)
                    {
                        player.FireWeapon();
                    }
                }
                else if (lookRotationWithControllerMovementBased.magnitude != 0)
                {
                    firstTime = true;
                    lookAtDirection = -lookRotationWithControllerMovementBased;
                    transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(lookAtDirection.normalized), transform.rotation, 0.5f);
                }
                else
                {
                    firstTime = true;
                }
            }
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
}
