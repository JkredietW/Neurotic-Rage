using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletBehavior : MonoBehaviour
{
    float damage;
    int pierceAmount;
    public GameObject bloodSpat;

    public void SetUp(float _damage, int _pierces)
    {
        damage = _damage;
        pierceAmount = _pierces;
        Destroy(gameObject, 5);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseHealth>())
        {
            BaseHealth health = other.GetComponent<BaseHealth>();
            GameObject tempBlood = Instantiate(bloodSpat, transform.position - transform.forward, transform.rotation);
            tempBlood.GetComponent<VisualEffect>().Play();
            Destroy(tempBlood.gameObject, 5);
            if (pierceAmount > 0)
            {
                pierceAmount--;
                DoDamageToEnemy(health);
            }
            else
            {
                DoDamageToEnemy(health);
                Destroy(gameObject);
            }
        }
    }
    void DoDamageToEnemy(BaseHealth _health)
    {
        _health.DoDamage(damage);
    }
}
