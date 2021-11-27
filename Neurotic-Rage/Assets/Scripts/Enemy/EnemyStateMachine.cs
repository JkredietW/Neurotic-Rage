using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    public float attackCoolDown;
    public float attackRange;
    public float chaseRange;
    public float destroyTime = 1.25f;
    public float hitBoxRange;
    public float damage;
    public float toCloseRange;
    public float rotationSpeed;

    public int numberOffAttacks;

    public Transform handPos;
    public GameObject player;
    public NavMeshAgent navMeshAgent;
    private bool doDamage;
    private bool attacking;
    private bool hitbox;
    
    public Animator anim;
    public GameObject head;

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
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) >= toCloseRange)
		{
            navMeshAgent.SetDestination(player.transform.position);

            var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed/2 * Time.deltaTime);

            head.transform.LookAt(player.transform.position - new Vector3(0, 1.5f, 0));
            anim.SetBool("Feet", false);
        }
		else
		{
            var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            navMeshAgent.SetDestination(transform.position);
            head.transform.LookAt(player.transform.position- new Vector3(0,1.5f,0));
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
    }
    public void Attack()
	{
        anim.SetLayerWeight(1, 100);
        int randomAtt = Random.Range(1, numberOffAttacks+1);
        anim.SetTrigger("Attack" + randomAtt.ToString());
        anim.SetBool("Feet", true);
        attacking = true;
        Invoke("ResetAnim", attackCoolDown);
	}
	public void ResetAnim()
	{
        anim.SetBool("Feet", false);
        anim.SetLayerWeight(1, 0);
        attacking = false;
    }
	public void HitBoxTrigger(int i)
	{
		if (i == 1)
		{
            hitbox = false;
		}
		else
		{
            hitbox = true;
        }
	}
}