using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldWeapon : InterActable
{
    public Weapon heldItem;
    public bool alreadyInWorld;

    bool destoryThisObjectNextDrop;

    private void Start()
    {
        if(alreadyInWorld)
        {
            Setup(heldItem, false);
        }
    }
    void DelayAfterStart()
    {
        if (destoryThisObjectNextDrop)
        {
            RemoveSpecialWeapon();
        }
        else
        {
            Collider[] players = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
            foreach (var item in players)
            {
                if (item.GetComponent<PlayerMovement>())
                {
                    player = item.GetComponent<PlayerMovement>();
                }
            }
            alreadyInWorld = true;
        }
    }
    public void Setup(Weapon _newWeapon, bool _destory)
    {
        destoryThisObjectNextDrop = _destory;
        heldItem = _newWeapon;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().AddForce(transform.forward + transform.up, ForceMode.VelocityChange);
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<SphereCollider>().radius = 2;
        GetComponent<BoxCollider>().enabled = true;
        //gameObject.AddComponent<BoxCollider>();
        float time = alreadyInWorld == true ? 0 : 0.5f;
        if (alreadyInWorld)
        {
            heldItem.ammo = heldItem.maxAmmo;
        }
        Invoke(nameof(DelayAfterStart), time);
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
    void RemoveSpecialWeapon()
    {
        Destroy(gameObject);
    }
}
