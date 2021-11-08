using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    Camera playerCamera;

    [Header("PlayerStats")]
    public float movementSpeed = 1;

    Vector3 moveDir;

    [Header("CameraStats")]
    public LayerMask aimLayer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        GetComponentInChildren<PlayerAim>().GetVariables();
    }

    private void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDir = Quaternion.Euler(0,playerCamera.transform.eulerAngles.y,0) * moveDir;
    }
    private void FixedUpdate()
    {
        controller.Move(movementSpeed * Time.deltaTime * moveDir.normalized);
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
