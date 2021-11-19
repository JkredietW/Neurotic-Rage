using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldWeapon : MonoBehaviour
{
    public Weapon heldItem;
    public bool alreadyInWorld;
    bool playerInRange;
    PlayerMovement playerMovement;

    private void Start()
    {
        GetComponent<MeshFilter>().mesh = heldItem.weaponMesh;
        float time = alreadyInWorld == true ? 0 : 0.5f;
        if(alreadyInWorld)
        {
            heldItem.ammo = heldItem.maxAmmo;
        }
        Invoke("DelayAfterStart", time);
    }
    void DelayAfterStart()
    {
        gameObject.AddComponent<BoxCollider>();
        alreadyInWorld = true;
        Collider[] player = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
        foreach (var item in player)
        {
            if(item.GetComponent<PlayerMovement>())
            {
                playerInRange = true;
                playerMovement = item.GetComponent<PlayerMovement>();
                playerMovement.InWeaponRange(this);
            }
        }
    }
    public void Setup(Weapon _newWeapon)
    {
        heldItem = _newWeapon;
        GetComponent<MeshFilter>().mesh = heldItem.weaponMesh;
        GetComponent<Rigidbody>().AddForce(transform.forward + transform.up, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>() && alreadyInWorld)
        {
            playerInRange = true;
            playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.InWeaponRange(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() && alreadyInWorld)
        {
            playerInRange = false;
            playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.OutOfWeaponRange(this);
        }
    }

    public void ItemIsPickedUp()
    {
        Destroy(gameObject);
    }
}
