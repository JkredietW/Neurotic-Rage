using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public float maxhealth;
    float health;

    private void Start()
    {
        health = maxhealth;
    }
    public virtual void DoDamage(float _damage)
    {
        health = Mathf.Clamp(health -= _damage, 0, maxhealth);
        if(health == 0)
        {
            Died();
        }
    }
    public virtual void Died()
    {
        Destroy(gameObject);
        //hier ragdoll;
    }
}
