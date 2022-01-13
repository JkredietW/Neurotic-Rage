using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public GameObject endStats;
    public PlayerMovement pm;
    public GameManager gm;
    public LoadingScreen ls;
    public TextMeshProUGUI[] statsText;
    public AudioSource gruntPlayer;
    public AudioClip[] grunts;

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
        int randomgrunt = Random.Range(0, grunts.Length);
        gruntPlayer.clip = grunts[randomgrunt];
        gruntPlayer.Play();
        healthSlider.value = health;
    }
    public void GainMoreMaxHealth(float _extraHealth)
    {
        maxhealth = baseMaxHealth + _extraHealth;
        healthSlider.value = health;
    }
    public override void Died()
    {
        Destroy(FindObjectOfType<GiantHealth>().transform.gameObject);
        ShowStats();
        FindObjectOfType<GameManager>().statsScript.total_deaths++;
        FindObjectOfType<GameManager>().Save();
        pm.MayMove(false);
        mesh.SetActive(false);
        deathPlayer.SetActive(true);
        deathLight.SetActive(true);
        Time.timeScale = 0.3f;
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
        Invoke("ShowLoss", 1);
    }
    public void ShowLoss()
	{
        youDied.SetActive(true);
	}
    public IEnumerator LoadNewScene()
	{
        yield return new WaitForSeconds(4);
        youDied.SetActive(false);
        endStats.SetActive(true);
        yield return new WaitForSeconds(2);
        endStats.SetActive(false);
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
        ShowStats();
        FindObjectOfType<CarScript>().otherPos.transform.SetParent(null);
        FindObjectOfType<CarScript>().transform.SetParent(null);
        pm.MayMove(false);
        mesh.SetActive(false);
        deathPlayer.SetActive(true);
        deathLight.SetActive(true);
        Time.timeScale = 0.3f;
        pm.swordInHand.SetActive(false);
        pm.swordOnBack.SetActive(false);
        pm.enabled = false;
        yield return new WaitForSeconds(1.5f);
        endStats.SetActive(true);
        yield return new WaitForSeconds(4);
        endStats.SetActive(false);
        fdb.FadeToBlack(2.5f);
        yield return new WaitForSeconds(0.15f);
        ls.ChargementScene(0);
    }
    public void ShowStats()
	{
        statsText[0].text = "Time Alive"+"  "+ Mathf.Round(gm.time).ToString();
        statsText[1].text = "Waves Completed" + "  " + Mathf.Round(gm.statsScript.thisgame_competedWaves).ToString();
        statsText[2].text = "kills" + "  " + Mathf.Round(gm.statsScript.thisgame_kills).ToString();
        statsText[3].text = "Money Collected" + "  " + Mathf.Round(gm.statsScript.thisgame_moneyCollected).ToString();
        statsText[4].text = "Money Spend" + "  " + Mathf.Round(gm.statsScript.thisgame_moneySpend).ToString();
        statsText[5].text = "Upgrades" + "  " + Mathf.Round(gm.statsScript.thisgame_upgradesBought).ToString();
        statsText[6].text = "Distance Travaled" + "  " + Mathf.Round(gm.statsScript.thisgame_distanceWalked).ToString();
        statsText[7].text = "Damage Taken" + "  " + Mathf.Round(gm.statsScript.thisgame_damageTaken).ToString();
        statsText[8].text = "Small Enemys Killed" + "  " + Mathf.Round(gm.statsScript.thisgame_smallEnemyKills).ToString();
        statsText[9].text = "Medium Enemys Killed" + "  " + Mathf.Round(gm.statsScript.thisgame_mediumEnemyKills).ToString();
        statsText[10].text = "Medium Enemys Killed" + "  " + Mathf.Round(gm.statsScript.thisgame_bigEnemyKills).ToString();
        statsText[11].text = "Glitch Enemys Killed" + "  " + Mathf.Round(gm.statsScript.thisgame_glitchEnemyKills).ToString();
        statsText[12].text = "Time Waisted Not Shooting" + "  " + Mathf.Round(gm.statsScript.thisgame_timeWastedNotShooting).ToString();
        statsText[13].text = "Bullets Mised" + "  " + Mathf.Round(gm.statsScript.thisgame_bulletsMissed).ToString();
        statsText[14].text = "Bullets Hit" + "  " + Mathf.Round(gm.statsScript.thisgame_bulletsHit).ToString();
        statsText[15].text = "Times Reloaded" + "  " + Mathf.Round(gm.statsScript.thisgame_timesReloaded).ToString();
    }

}
