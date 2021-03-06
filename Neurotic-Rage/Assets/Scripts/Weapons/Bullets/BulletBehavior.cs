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
    public GameObject Sparks;
    public GameObject bloodSpatDecal;
    public GameObject Explosion;
    public List<string> IgnoreTag;
    public LayerMask groundLayer;
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
            GameObject tempBloodDecal = Instantiate(bloodSpatDecal, CheckForGroundHeight(), rotation);
            tempBlood.GetComponent<VisualEffect>().Play();
            Destroy(tempBlood, 5);
            Destroy(tempBloodDecal, 5);
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
        else if(other.GetComponent<EnemyHealth>() && explosionRadius > 0)
        {
            Explode();
            if (pierceAmount > 0)
            {
                pierceAmount--;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (!IgnoreTag.Contains(other.gameObject.tag))
            {
                if (explosionRadius > 0)
                {
                    Explode();
                }
                GameObject tempSparks = Instantiate(Sparks, transform.position, rotation);
                Destroy(tempSparks, 1);
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
    void Explode()
    {
        GameObject boom = Instantiate(Explosion);
        boom.transform.SetPositionAndRotation(transform.position, transform.rotation);
        boom.GetComponent<VisualEffect>().SetFloat("Scale", explosionRadius);
        Destroy(boom, 5);

        Collider[] hitObjects = Physics.OverlapSphere(boom.transform.position, explosionRadius / 25);
        foreach (var item in hitObjects)
        {
            if (item.GetComponent<EnemyHealth>())
            {
                item.GetComponent<EnemyHealth>().DoDamage(damage);
                Vector3 pointToSpawn = item.transform.position;
                GameObject tempBlood = Instantiate(bloodSpat, pointToSpawn, rotation);
                GameObject tempBloodDecal = Instantiate(bloodSpatDecal, CheckForGroundHeight(), rotation);
                tempBlood.GetComponent<VisualEffect>().Play();
                Destroy(tempBlood, 5);
                Destroy(tempBloodDecal, 5);
            }
        }
    }
    Vector3 CheckForGroundHeight()
    {
        Vector3 groundPos = Vector3.zero;
        RaycastHit _hit;
        if(Physics.Raycast(transform.position, Vector3.down, out _hit, 10, groundLayer))
        {
            groundPos = _hit.point;
            groundPos.y -= 0.4f;
        }
        return groundPos;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, explosionRadius / 25);
    }
}
