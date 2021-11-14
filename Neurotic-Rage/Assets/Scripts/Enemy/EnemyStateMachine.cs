using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    public float attackRange;
    public float chaseRange;
    public float destroyTime = 1.25f;

    public GameObject player;
    public NavMeshAgent navMeshAgent;

    EnemyStates currentEnemyState;

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
    public void EnemyStandbyState()
    {
        EnterChaseState();
    }
    public void EnemyChaseState()
    {
        navMeshAgent.SetDestination(player.transform.position);
        if(Vector3.Distance(gameObject.transform.position, player.transform.position) <= attackRange)
        {
            currentEnemyState = EnemyStates.attack;
        }
    }

    public void EnemyPatrolState()
    {
        EnterChaseState();
    }

    public void EnemyAttackState()
    {
        //attack animation and damage stuff (maybe spawn in hitbox)
        print("Attack!");
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
        print("enter chase 1");
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= chaseRange)
        {
            print("enter chase 2");
            Debug.DrawLine(transform.position, (player.transform.position - gameObject.transform.position));
            RaycastHit hit;
            if(Physics.Raycast(transform.position, (player.transform.position - gameObject.transform.position), out hit, Mathf.Infinity))
            {
                print("enter chase 3");
                if (hit.transform.gameObject.layer == 12)
                {
                    print("Chasing");
                    currentEnemyState = EnemyStates.chase;
                }
            }
        }
    }
}