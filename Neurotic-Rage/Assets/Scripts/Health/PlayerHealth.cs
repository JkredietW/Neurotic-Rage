using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    Animator animator;
    public Slider healthSlider;

    private void Start()
    {
        animator = GetComponent<PlayerMovement>().animator;
        healthSlider.maxValue = maxhealth;
        healthSlider.value = maxhealth;
        healthSlider.minValue = 0;
        health = maxhealth;
    }
    public override void DoDamage(float _damage)
    {
        base.DoDamage(_damage);
        animator.SetTrigger("GetHit");
        healthSlider.value = health;
    }
    public void GainMoreMaxHealth(float _extraHealth)
    {
        maxhealth = baseMaxHealth + _extraHealth;
        healthSlider.value = health;
    }
}
