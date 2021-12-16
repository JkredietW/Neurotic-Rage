using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GiantHealth : EnemyHealth
{
    public GameObject momDiedLight;
    public bool isDead;
    public Color deadOutline;
    private float chanceForDrop = 0;
    private WorldWeapon worldWeaponPrefab;
    private List<Weapon> dropItems;
    public override void Died()
    {
        FindObjectOfType<GameManager>().EnemyDied(gameObject);
        DropItems();
        for (int i = 0; i < outline.Length; i++)
        {
            outline[i].OutlineColor = deadOutline;
        }
        agent.enabled = false;
        col.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        anim.enabled = false;
        es.enabled = false;
        isDead = true;
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i].GetComponent<EnemyHealth>())
            {
                enemyList[i].transform.GetComponent<EnemyHealth>().PlayerDied();
            }
        }
        momDiedLight.SetActive(true);
        FindObjectOfType<GameManager>().MomDied();
    }
    public override void DropItems()
    {
        float roll = Random.Range(0, 100);
        if(roll < chanceForDrop)
        {
            FindObjectOfType<GameManager>().ResetDropChance();
            float chance = Random.Range(0, dropItems.Count);
            Instantiate(worldWeaponPrefab, transform.position, Quaternion.identity).Setup(dropItems[(int)chance], false);
        }
    }
    public override void EnemySetup(float _scaling, float _drop, WorldWeapon _worldWeapon, List<Weapon> _weapons)
    {
        maxhealth *= _scaling;
        health = maxhealth;

        anim.speed *= _scaling;
 
        agent.speed *= _scaling;

        chanceForDrop = _drop;
        worldWeaponPrefab = _worldWeapon;

        dropItems = new List<Weapon>(_weapons);
    }
    public override void DestroyObj()
	{
        Destroy(gameObject);
    }
    public override void PlayerDied()
    {
        es.enabled = false;
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
}
