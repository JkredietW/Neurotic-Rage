using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    

    EnemyStates currentEnemyState;

    enum EnemyStates
    {
        standby,
        chase,
        patrol,
        attack
    }

    void Start()
    {
        
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
            default:
                print("5");
                Debug.LogError("stateChanger reached default state");

                break;
        }
    }

    public void EnemyStandbyState()
    {

    }

    public void EnemyChaseState()
    {

    }

    public void EnemyPatrolState()
    {

    }

    public void EnemyAttackState()
    {

    }

    //

    public void EnterChaseState()
    {
        if()
    }
}
