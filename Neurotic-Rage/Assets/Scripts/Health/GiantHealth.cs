using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class GiantHealth : EnemyHealth
{
    public GameObject momDiedLight;
    public Slider healthSlider;
    public GameObject firstText, SecondText,healtbar,buttons;
    public bool isDead;
    public Color deadOutline;

	private void start()
	{
        healthSlider.maxValue = maxhealth;
        healthSlider.value = maxhealth;
        healtbar.SetActive(false);
    }
	public override void Dying()
    {
        healtbar.SetActive(false);
        for (int i = 0; i < outline.Length; i++)
        {
            outline[i].OutlineColor = deadOutline;
        }
        isDead = true;
        momDiedLight.SetActive(true);
        FindObjectOfType<GameManager>().MomDied();
        Invoke("DeactivateFirst", 2.5f);
    }
    public void DeactivateFirst()
	{
        firstText.SetActive(false);
        SecondText.SetActive(true);
        buttons.SetActive(true);
    }
	public override void UpdateHealthBar()
	{
        healthSlider.value = health;
        print("updates");
	}
}
