using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    PlayerMovement player;
    Camera playerCamera;
    LayerMask aimLayer;

    public Vector3 lookAtDirection;
    public void GetVariables()
    {
        player = GetComponentInParent<PlayerMovement>();
        playerCamera = player.GiveCamera();
        aimLayer = player.GiveLayerMask();
    }

    private void Update()
    {
        RaycastHit _hit;
        Ray _ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(_ray, out _hit, Mathf.Infinity, aimLayer))
        {
            lookAtDirection = _hit.point - player.transform.position;
            lookAtDirection.y = 0;
            lookAtDirection.Normalize();
            transform.rotation = Quaternion.LookRotation(lookAtDirection);
        }
        else
        {
            print("Raycast didnt hit anything, source: " + transform.name);
            return;
        }
    }
}
