using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletBehavior : MonoBehaviour
{
    float damage;
    int pierceAmount;
    Quaternion rotation;
    public GameObject bloodSpat;

    public void SetUp(float _damage, int _pierces, Quaternion _rotation)
    {
        damage = _damage;
        pierceAmount = _pierces;
        rotation = _rotation;
        Destroy(gameObject, 5);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseHealth>())
        {
            BaseHealth health = other.GetComponent<BaseHealth>();
            Vector3 pointToSpawn = other.ClosestPoint(transform.position);
            GameObject tempBlood = Instantiate(bloodSpat, pointToSpawn, rotation);
            tempBlood.GetComponent<VisualEffect>().Play();
            Destroy(tempBlood, 5);
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
