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
    public List<string> IgnoreTag;
    bool mayNotDoDamage;
    bool hasHitAtleastOne;
    [SerializeField] float explosionRadius;

    public void SetUp(float _damage, int _pierces, Quaternion _rotation, bool _mayNotDoDamage)
    {
        damage = _damage;
        pierceAmount = _pierces;
        rotation = _rotation;
        mayNotDoDamage = _mayNotDoDamage;
        Destroy(gameObject, 5);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(mayNotDoDamage)
        {
            return;
        }
        if (other.GetComponent<EnemyHealth>() && explosionRadius == 0)
        {
            //stats
            hasHitAtleastOne = true;
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
        else
        {
            if (explosionRadius > 0)
            {
                Explode();
            }
            if (!IgnoreTag.Contains(other.gameObject.tag))
            {
                float roll = Random.Range(0, 100);
                if (pierceAmount > 0 && roll > 50)
                {
                    pierceAmount--;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnDestroy()
    {
        if(!hasHitAtleastOne)
        {
            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsMissed++;
            FindObjectOfType<GameManager>().statsScript.total_bulletsMissed++;
        }
        else
        {
            FindObjectOfType<GameManager>().statsScript.thisgame_bulletsHit++;
            FindObjectOfType<GameManager>().statsScript.total_bulletsHit++;
        }
    }
    void DoDamageToEnemy(BaseHealth _health)
    {
        _health.DoDamage(damage);
    }
    private void Update()
    {
        if(transform.position.y <= 0 && explosionRadius > 0)
        {
            Explode();
        }
    }
    void Explode()
    {
        print(123132);
        //instantiate boom
        //remove boom in time
        //do damage in randius of boom
        //remove object
    }
}
