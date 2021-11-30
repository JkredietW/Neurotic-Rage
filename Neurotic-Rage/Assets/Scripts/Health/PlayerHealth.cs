using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<PlayerMovement>().animator;
    }
    public override void DoDamage(float _damage)
    {
        base.DoDamage(_damage);
        animator.SetTrigger("GetHit");
    }
    public void GainMoreMaxHealth(float _extraHealth)
    {
        maxhealth = baseMaxHealth + _extraHealth;
    }
}
