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
	private bool greenLightB;
	private void Start()
	{
		StartCoroutine(Light());
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
	}
	public void SecondStage()
	{
		propelor.SetBool("SecondStage", true);
	}
	public void StartScene()
	{
		SceneManager.LoadScene(sceneIndexToAdd, LoadSceneMode.Additive);
		helliCam.SetActive(false);
		player.SetActive(true);

	}
}