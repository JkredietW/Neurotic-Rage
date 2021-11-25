using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public override void Died()
    {
        FindObjectOfType<GameManager>().EnemyDied(gameObject);
        Destroy(gameObject);
        //hier ragdoll;
    }
}
