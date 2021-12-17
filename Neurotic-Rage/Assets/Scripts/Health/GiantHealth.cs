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
        FindObjectOfType<GameManager>().EnemyDied(gameObject);
        DropItems();
        for (int i = 0; i < outline.Length; i++)
        {
            outline[i].OutlineColor = deadOutline;
        }
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
}
