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

    [Header("Sounds")]
    public AudioSource AttackSound;
    public AudioSource walkingSound;
    public AudioSource[] screamSounds;

    EnemyStates currentEnemyState;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ScreamSound());
    }
    enum EnemyStates
    {
        standby,
        chase,
        patrol,
        attack,
        dying
    }
    public IEnumerator ScreamSound()
	{
        float waitTime = Random.Range(0.5f, 1.25f);
        yield return new WaitForSeconds(waitTime);
        int randomSound = Random.Range(0, screamSounds.Length);
        screamSounds[randomSound].Play();
        ReturnScream();
    }
    public void ReturnScream()
	{
        StartCoroutine(ScreamSound());
    }

    void Update()
    {
        if (hitbox)
        {
            Collider[] hitObjects=Physics.OverlapSphere(handPos.position, hitBoxRange);
			for (int i = 0; i < hitObjects.Length; i++)
			{
				if (hitObjects[i].transform.gameObject.CompareTag("Player"))
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

                break;
            case EnemyStates.chase:
                EnemyChaseState();

                break;
            case EnemyStates.patrol:
                EnemyPatrolState();

                break;
            case EnemyStates.attack:
                EnemyAttackState();

                break;
            case EnemyStates.dying:
                StartCoroutine(EnemyDyingState());
                break;
            default:
                Debug.LogError("stateChanger reached default state");

                break;
        }
		if (player == null)
		{
            return;
		}
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) >= toCloseRange)
		{
			if (!walkingSound.isPlaying)
			{
                walkingSound.Play();
            }
			if (!navMeshAgent.enabled)
			{
                return;
			}
            navMeshAgent.SetDestination(player.transform.position);

            var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed/2 * Time.deltaTime);

            head.transform.LookAt(player.transform.position - new Vector3(0, 1.5f, 0));
            anim.SetBool("Feet", false);
        }
		else
		{
            if (walkingSound.isPlaying)
            {
                walkingSound.Stop();
            }
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
		if (player == null)
		{
            return;
		}
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
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    public void EnterChaseState()
    {
        currentEnemyState = EnemyStates.chase;
    }
    public void Attack()
	{
        if (walkingSound.isPlaying)
        {
            walkingSound.Stop();
        }
        anim.SetLayerWeight(1, 100);
        int randomAtt = Random.Range(1, numberOffAttacks+1);
        anim.SetTrigger("Attack" + randomAtt.ToString());
        anim.SetBool("Feet", true);
        attacking = true;
        Invoke("ResetAnim", attackCoolDown);
        PlayAttackSound();
    }
	public void ResetAnim()
	{
        anim.SetBool("Feet", false);
        anim.SetLayerWeight(1, 0);
        attacking = false;
    }
    public void PlayAttackSound()
    {
        AttackSound.Play();
    }
    public void HitBoxTrigger(int i)
	{
		if (i == 1)
		{
            hitbox = false;
            doDamage = false;
        }
		else
		{
            hitbox = true;
        }
	}
    public void StopAlAudio()
	{
        AttackSound.Stop();
        walkingSound.Stop();
		for (int i = 0; i < screamSounds.Length; i++)
		{
            screamSounds[i].Stop();

        }
    }
}