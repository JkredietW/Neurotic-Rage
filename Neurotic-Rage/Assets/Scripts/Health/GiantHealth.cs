using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GiantHealth : EnemyHealth
{
    public GameObject momDiedLight;
    public bool isDead;
    public Color deadOutline;
    public override void Dying()
    {
        for (int i = 0; i < outline.Length; i++)
        {
            outline[i].OutlineColor = deadOutline;
        }
        isDead = true;
        momDiedLight.SetActive(true);
        FindObjectOfType<GameManager>().MomDied();
    }
}
