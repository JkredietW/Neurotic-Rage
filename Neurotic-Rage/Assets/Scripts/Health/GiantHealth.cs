using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class GiantHealth : EnemyHealth
{
    public GameObject momDiedLight;
    public Slider healthSlider;
    public GameObject firstText, SecondText,healtbar;
    public bool isDead;
    public Color deadOutline;

	private void Awake()
	{
        healthSlider.maxValue = maxhealth;
        healtbar.SetActive(false);
    }
	public override void Dying()
    {
        for (int i = 0; i < outline.Length; i++)
        {
            outline[i].OutlineColor = deadOutline;
        }
        isDead = true;
        momDiedLight.SetActive(true);
        FindObjectOfType<GameManager>().MomDied();
        Invoke("DeactivateFirst", 1f);
    }
    public void DeactivateFirst()
	{
        firstText.SetActive(false);
        SecondText.SetActive(true);
    }
	public override void UpdateHealthBar()
	{
        healthSlider.value = health;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
            healtbar.SetActive(true);
		}
	}
	private void OnTriggerExit(Collider other)
	{
        if (other.transform.CompareTag("Player"))
        {
            healtbar.SetActive(false);
        }
    }
}
