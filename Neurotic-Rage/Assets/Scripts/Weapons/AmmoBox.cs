using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : InterActable
{
    public int ammoAmount, specialAmmoAmount;

    public override void OnPlayerEnter(PlayerMovement _thisOne)
    {
        base.OnPlayerEnter(_thisOne);
        player.GrantAmmo(ammoAmount, specialAmmoAmount);
        Destroy(gameObject);
    }
}
