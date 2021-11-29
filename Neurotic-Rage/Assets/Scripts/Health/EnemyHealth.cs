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
    public override void Died()
    {
        FindObjectOfType<GameManager>().EnemyDied(gameObject);
        agent.enabled = false;
        col.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        anim.enabled = false;
        es.enabled = false;
        Invoke("DestroyObj",4);
    }
    public void DestroyObj()
	{
        Destroy(gameObject);
    }
}
