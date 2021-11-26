using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldWeapon : InterActable
{
    public Weapon heldItem;
    public bool alreadyInWorld;

    private void Start()
    {
        GetComponent<MeshFilter>().mesh = heldItem.weaponMesh;
        float time = alreadyInWorld == true ? 0 : 0.5f;
        if(alreadyInWorld)
        {
            heldItem.ammo = heldItem.maxAmmo;
        }
        Invoke(nameof(DelayAfterStart), time);
    }
    void DelayAfterStart()
    {
        gameObject.AddComponent<BoxCollider>();
        alreadyInWorld = true;
        Collider[] players = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
        foreach (var item in players)
        {
            if(item.GetComponent<PlayerMovement>())
            {
                player = item.GetComponent<PlayerMovement>();
                player.InWeaponRange(this);
            }
        }
    }
    public void Setup(Weapon _newWeapon)
    {
        heldItem = _newWeapon;
        GetComponent<MeshFilter>().mesh = heldItem.weaponMesh;
        GetComponent<Rigidbody>().AddForce(transform.forward + transform.up, ForceMode.VelocityChange);
    }

    public override void OnPlayerEnter(PlayerMovement _thisOne)
    {
        base.OnPlayerEnter(_thisOne);
        if (alreadyInWorld)
        {
            player.InWeaponRange(this);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() && alreadyInWorld)
        {
            player = other.GetComponent<PlayerMovement>();
            player.OutOfWeaponRange(this);
        }
    }

    public void ItemIsPickedUp()
    {
        Destroy(gameObject);
    }
}
