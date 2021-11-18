using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount, specialAmmoAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovement>())
        {
            other.GetComponent<PlayerMovement>().GrantAmmo(ammoAmount, specialAmmoAmount);
        }
    }
}
