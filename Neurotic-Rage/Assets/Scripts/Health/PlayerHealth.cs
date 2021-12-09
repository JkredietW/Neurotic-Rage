using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    Animator animator;
    public Slider healthSlider;
    public GameObject deathPlayer;
    public GameObject deathLight;
    public GameObject mesh;
    public PlayerMovement pm;


    private void Start()
    {
        animator = GetComponent<PlayerMovement>().animator;
        healthSlider.maxValue = maxhealth;
        healthSlider.value = maxhealth;
        healthSlider.minValue = 0;
        health = maxhealth;
    }
    public override void DoDamage(float _damage)
    {
        base.DoDamage(_damage);
        animator.SetTrigger("GetHit");
        healthSlider.value = health;
    }
    public void GainMoreMaxHealth(float _extraHealth)
    {
        maxhealth = baseMaxHealth + _extraHealth;
        healthSlider.value = health;
    }
    public override void Died()
    {
        mesh.SetActive(false);
        deathPlayer.SetActive(true);
        deathLight.SetActive(true);
        pm.swordInHand.SetActive(false);
        pm.swordOnBack.SetActive(false);
        pm.enabled = false;
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i].GetComponent<EnemyHealth>())
            {
                enemyList[i].transform.GetComponent<EnemyHealth>().PlayerDied();
            }
        }
    }

}
