using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellecopter : MonoBehaviour
{
	public Animator propelor;
	public GameObject helliCam;
	public GameObject player;
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
		helliCam.SetActive(false);
		player.SetActive(true);
	}
}