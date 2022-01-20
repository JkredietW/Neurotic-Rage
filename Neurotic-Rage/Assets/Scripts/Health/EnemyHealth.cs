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
    public EnemyType enemyType;
    public AudioSource getHit;

    public enum EnemyType
    {
        small,
        medium,
        big,
        glitch,
        giant,
    }
    public  override void Died()
    {
        UpdateStats();
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
            es.StopAlAudio();
            es.enabled = false;
        }
		else
		{
            ed.StopAlAudio();
            ed.enabled = false;
		}
        Dying();
        Invoke("DestroyObj",4);
    }
    void UpdateStats()
    {
        FindObjectOfType<GameManager>().statsScript.thisgame_kills++;
        FindObjectOfType<GameManager>().statsScript.total_kills++;
        switch (enemyType)
        {
            case EnemyType.small:
                FindObjectOfType<GameManager>().statsScript.thisgame_smallEnemyKills++;
                FindObjectOfType<GameManager>().statsScript.total_smallEnemyKills++;
                break;
            case EnemyType.medium:
                FindObjectOfType<GameManager>().statsScript.thisgame_mediumEnemyKills++;
                FindObjectOfType<GameManager>().statsScript.total_mediumEnemyKills++;
                break;
            case EnemyType.big:
                FindObjectOfType<GameManager>().statsScript.thisgame_bigEnemyKills++;
                FindObjectOfType<GameManager>().statsScript.total_bigEnemyKills++;
                break;
            case EnemyType.glitch:
                FindObjectOfType<GameManager>().statsScript.thisgame_glitchEnemyKills++;
                FindObjectOfType<GameManager>().statsScript.total_glitchEnemyKills++;
                break;
            case EnemyType.giant:
                FindObjectOfType<GameManager>().statsScript.thisgame_giantEnemyKills++;
                FindObjectOfType<GameManager>().statsScript.total_giantEnemyKills++;
                break;
        }
    }
    public override void DoDamage(float _damage)
    {
        FindObjectOfType<GameManager>().statsScript.thisgame_damageDone += _damage;
        FindObjectOfType<GameManager>().statsScript.total_damageDone += _damage;
        base.DoDamage(_damage);
        anim.SetTrigger("GetHit");
        getHit.Play();
        UpdateHealthBar();
    }
    public virtual void UpdateHealthBar()
	{

	}
    public virtual void Dying()
	{

	}
    public virtual void DropItems()
    {
        float roll = Random.Range(0, 100);
        if(roll < chanceForDrop)
        {
            FindObjectOfType<GameManager>().ResetDropChance();
            float chance = Random.Range(0, dropItems.Count);
            GameObject droppedWeapon = Instantiate(dropItems[(int)chance].objectprefabWorld, transform.position, Quaternion.identity);
            droppedWeapon.GetComponent<WorldWeapon>().Setup(dropItems[(int)chance], false);
        }
    }
    public virtual void EnemySetup(float _scaling, float _drop, WorldWeapon _worldWeapon, List<Weapon> _weapons)
    {
        maxhealth *= _scaling;
        maxhealth *= Random.Range(0.8f, 1.2f);
        health = maxhealth;

        anim.speed *= _scaling * 0.5f;
        anim.speed *= Random.Range(0.8f, 1.2f);
        if (type == TypeEnemy.normal)
        {
            es.damage *= _scaling;
            es.damage *= Random.Range(0.8f, 1.2f);
        }
        else
        {
            ed.damage *= _scaling;
            ed.damage *= Random.Range(0.8f, 1.2f);
        }
        agent.speed *= _scaling * 0.5f;
        agent.speed *= Random.Range(0.8f, 1.2f);

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
