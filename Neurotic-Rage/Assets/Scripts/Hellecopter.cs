using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hellecopter : MonoBehaviour
{
	public float timeBetweenSwitch;
	public int sceneIndexToAdd;
	public GameObject greenLight;
	public GameObject redLight;
	public Animator propelor;
	public GameObject helliCam;
	public GameObject player;
	public GameManager gm;
	public PlayerMovement[] pm;
	public GameObject fireObject;
	public AudioSource flyingSound;
	public AudioClip altmostCrash,crash;
	private bool greenLightB;
	private void Start()
	{
		StartCoroutine(Light());
		GetComponent<FadeToFromBlack>().FadeFromBlack(2);
	}
	public IEnumerator Light()
	{
		greenLightB = !greenLightB;
		if (greenLightB)
		{
			redLight.SetActive(false);
			greenLight.SetActive(true);
		}
		else
		{
			greenLight.SetActive(false);
			redLight.SetActive(true);
		}
		yield return new WaitForSeconds(timeBetweenSwitch);
		StartCoroutine(Light());
	} 
	public void FirstStage()
	{
		propelor.SetBool("FirstStage", true);
		flyingSound.clip= altmostCrash;
		flyingSound.Play();
		Invoke("SetFire", 1);
	}
	public void SetFire()
	{
		fireObject.SetActive(true);
	}
	public void SecondStage()
	{ 
		propelor.SetBool("SecondStage", true);
	}
	public void StartScene()
	{
		flyingSound.clip = crash;
		flyingSound.Play();
		flyingSound.loop = false;
		SceneManager.LoadScene(sceneIndexToAdd, LoadSceneMode.Additive);
		helliCam.SetActive(false);
		player.SetActive(true);
		int playerInt = PlayerPrefs.GetInt("Skin");
		print(playerInt);
		pm[playerInt].MayMove(true);
		gm.DelayedStart();
	}
}