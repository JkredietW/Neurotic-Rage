using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : BaseHealth
{
    Animator animator;
    public GameObject youDied;
    public FadeToFromBlack fdb;
    public GameObject[] turnThisOff;
    public Slider healthSlider;
    public GameObject deathPlayer;
    public GameObject deathLight;
    public GameObject mesh;
    public PlayerMovement pm;
    public LoadingScreen ls;


    protected override void Start()
    {
        base.Start();
        animator = GetComponent<PlayerMovement>().animator;
        healthSlider.maxValue = maxhealth;
        healthSlider.value = maxhealth;
        healthSlider.minValue = 0;
    }
    public override void DoDamage(float _damage)
    {
        FindObjectOfType<GameManager>().statsScript.thisgame_damageTaken += _damage;
        FindObjectOfType<GameManager>().statsScript.total_damageTaken += _damage;
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
        //stats
        FindObjectOfType<GameManager>().statsScript.total_deaths++;
        FindObjectOfType<GameManager>().Save();
        pm.MayMove(false);
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
		for (int i = 0; i < turnThisOff.Length; i++)
		{
            turnThisOff[i].SetActive(false);

        }
        StartCoroutine(LoadNewScene());
        Invoke("ShowLoss", 4);
    }
    public void ShowLoss()
	{
        youDied.SetActive(true);
	}
    public IEnumerator LoadNewScene()
	{
        yield return new WaitForSeconds(10);
        fdb.FadeToBlack(2.5f);
        yield return new WaitForSeconds(1);
        ls.ChargementScene(0);
    }
    public void CarHIt()
	{
        StartCoroutine(CarComes());
    }
    public IEnumerator CarComes()
	{
        FindObjectOfType<CarScript>().otherPos.transform.SetParent(null);
        FindObjectOfType<CarScript>().transform.SetParent(null);
        pm.MayMove(false);
        mesh.SetActive(false);
        deathPlayer.SetActive(true);
        deathLight.SetActive(true);
        pm.swordInHand.SetActive(false);
        pm.swordOnBack.SetActive(false);
        pm.enabled = false;
        yield return new WaitForSeconds(1.5f);
        fdb.FadeToBlack(2.5f);
        yield return new WaitForSeconds(1);
        ls.ChargementScene(0);
    }

}
