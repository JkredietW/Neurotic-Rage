using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    public float attackRange;
    public float chaseRange;
    public float destroyTime = 1.25f;
    public float hitBoxRange;
    public float damage;

    public int numberOffAttacks;

    public Transform handPos;
    public GameObject player;
    public NavMeshAgent navMeshAgent;
    private bool doDamage;
    private bool attacking;
    private bool hitbox;
    
    public Animator anim;

    EnemyStates currentEnemyState;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    enum EnemyStates
    {
        standby,
        chase,
        patrol,
        attack,
        dying
    }
    void Update()
    {
        if (hitbox)
        {
            Collider[] hitObjects=Physics.OverlapSphere(handPos.position, hitBoxRange);
			for (int i = 0; i < hitObjects.Length; i++)
			{
				if (hitObjects[i].transform.tag == "Player")
				{
					if (!doDamage)
					{
                        StartCoroutine(DoDamage(hitObjects[i].transform.gameObject));
					}
				}
			}
        }
        
        switch (currentEnemyState)
        {
            case EnemyStates.standby:
                EnemyStandbyState();
                print("1");

                break;
            case EnemyStates.chase:
                EnemyChaseState();
                print("2");

                break;
            case EnemyStates.patrol:
                EnemyPatrolState();
                print("3");

                break;
            case EnemyStates.attack:
                EnemyAttackState();
                print("4");

                break;
            case EnemyStates.dying:
                StartCoroutine(EnemyDyingState());
                break;
            default:
                print("5");
                Debug.LogError("stateChanger reached default state");

                break;
        }
    }
    public IEnumerator DoDamage(GameObject player)
	{
        doDamage = true;
        player.GetComponent<PlayerHealth>().DoDamage(damage);
        yield return new WaitForSeconds(numberOffAttacks / 2);
        doDamage = false;
    }
    public void EnemyStandbyState()
    {
        EnterChaseState();
    }
    public void EnemyChaseState()
    {
        navMeshAgent.SetDestination(player.transform.position);
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= attackRange)
        {
            currentEnemyState = EnemyStates.attack;
        }
        else
        {
            currentEnemyState = EnemyStates.chase;
        }
    }

    public void EnemyPatrolState()
    {
        EnterChaseState();
    }

    public void EnemyAttackState()
    {
        if (!attacking)
        {
            Attack();
        }
        currentEnemyState = EnemyStates.chase;
    }

    public IEnumerator EnemyDyingState()
    {
        //idk
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    //

    public void EnterChaseState()
    {
        currentEnemyState = EnemyStates.chase;
        //if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= chaseRange)
        //{
        //    Debug.DrawLine(transform.position, (player.transform.position - gameObject.transform.position));
        //    RaycastHit hit;
        //    if(Physics.Raycast(transform.position, (player.transform.position - gameObject.transform.position), out hit, Mathf.Infinity))
        //    {
        //        if (hit.transform.gameObject.layer == 12)
        //        {
        //            currentEnemyState = EnemyStates.chase;
        //        }
        //    }
        //}
    }
    public void Attack()
	{
        int randomAtt = Random.Range(1, numberOffAttacks);
        attacking = true;
        anim.SetBool("Attack" + randomAtt.ToString(), true);
	}
    public void HitBoxTrigger(int i)
	{
		if (i == 0)
		{
            hitbox = false;
		}
		else
		{
            hitbox = true;
        }
	}
}