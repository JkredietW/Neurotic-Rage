using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Camera playerCamera;

    [Header("playerStats")]
    public float movementSpeed = 1;

    Vector3 moveDir;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(moveDir.magnitude != 0)
        {
            moveDir += playerCamera.transform.forward;
        }
    }
    private void FixedUpdate()
    {
        controller.Move(movementSpeed * Time.deltaTime * moveDir.normalized);
    }
}
