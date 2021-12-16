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
    public TypeEnemy type;
    public EnemyStateMachine es;
    public EnemyDash ed;
    public Outline[] outline;
    public GameObject mapindicator;
    private float chanceForDrop = 0;
    private WorldWeapon worldWeaponPrefab;
    private List<Weapon> dropItems;
    public override void Died()
    {
        FindObjectOfType<GameManager>().EnemyDied(gameObject);
        DropItems();
        for (int i = 0; i < outline.Length; i++)
        {
            outline[i].enabled = false;
        }
        agent.enabled = false;
        col.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        anim.enabled = false;
        mapindicator.SetActive(false);
        if (type == TypeEnemy.normal)
		{
            es.enabled = false;
        }
		else
		{
            ed.enabled = false;
		}
        Invoke("DestroyObj",4);
    }
    public virtual void DropItems()
    {
        float roll = Random.Range(0, 100);
        if(roll < chanceForDrop)
        {
            FindObjectOfType<GameManager>().ResetDropChance();
            float chance = Random.Range(0, dropItems.Count);
            Instantiate(worldWeaponPrefab, transform.position, Quaternion.identity).Setup(dropItems[(int)chance], false);
        }
    }
    public virtual void EnemySetup(float _scaling, float _drop, WorldWeapon _worldWeapon, List<Weapon> _weapons)
    {
        maxhealth *= _scaling;
        health = maxhealth;

        anim.speed *= _scaling;
        if (type == TypeEnemy.normal)
        {
            es.damage *= _scaling;
        }
        else
        {
            ed.damage *= _scaling;
        }
        agent.speed *= _scaling;

        chanceForDrop = _drop;
        worldWeaponPrefab = _worldWeapon;

        dropItems = new List<Weapon>(_weapons);
    }
    public virtual void DestroyObj()
	{
        Destroy(gameObject);
    }
    public virtual void PlayerDied()
    {
        if (type == TypeEnemy.normal)
        {
            es.enabled = false;
        }
        else
        {
            ed.enabled = false;
        }
        for (int i = 0; i < outline.Length; i++)
        {
            outline[i].enabled = false;
        }
        agent.enabled = false;
        col.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        mapindicator.SetActive(false);
        anim.enabled = false;
        Invoke("DestroyObj", 3.5f);
    }
    public enum TypeEnemy
	{
        normal,
        dash,
	}
}
