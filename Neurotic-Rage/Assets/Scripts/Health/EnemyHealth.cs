    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyHealth : BaseHealth
{
    public NavMeshAgent agent;
    public BoxCollider col;
    public Rigidbody rb;
    public Animator anim;
    public EnemyStateMachine es;

    private float chanceForDrop = 0;
    private WorldWeapon worldWeaponPrefab;
    private List<Weapon> dropItems;
    public override void Died()
    {
        FindObjectOfType<GameManager>().EnemyDied(gameObject);
        DropItems();
        agent.enabled = false;
        col.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        anim.enabled = false;
        es.enabled = false;
        Invoke("DestroyObj",4);
    }
    void DropItems()
    {
        float roll = Random.Range(0, 100);
        if(roll > chanceForDrop)
        {
            FindObjectOfType<GameManager>().ResetDropChance();
            float chance = Random.Range(0, dropItems.Count);
            Instantiate(worldWeaponPrefab, transform.position, Quaternion.identity).Setup(dropItems[(int)chance], false);
        }
    }
    public void EnemySetup(float _scaling, float _drop, WorldWeapon _worldWeapon, List<Weapon> _weapons)
    {
        maxhealth *= _scaling;
        health = maxhealth;

        anim.speed *= _scaling;
        es.damage *= _scaling;
        agent.speed *= _scaling;

        chanceForDrop = _drop;
        worldWeaponPrefab = _worldWeapon;

        dropItems = new List<Weapon>(_weapons);
    }
    public void DestroyObj()
	{
        Destroy(gameObject);
    }
}
