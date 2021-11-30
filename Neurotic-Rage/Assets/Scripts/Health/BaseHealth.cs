using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public float maxhealth;
    float health;
    protected float baseMaxHealth;

    private void Start()
    {
        health = maxhealth;
        baseMaxHealth = maxhealth;
    }
    public virtual void DoDamage(float _damage)
    {
        if (health > 0)
        {
            health = Mathf.Clamp(health -= _damage, 0, maxhealth);
            if (health == 0)
            {
                Died();
            }
        }
    }
    public virtual void RecieveHealth(float _heal)
    {
        if (health < maxhealth)
        {
            health = Mathf.Clamp(health += _heal, 0, maxhealth);
        }
    }
    public virtual void Died()
    {
        print("Uses base health script");
    }
}
