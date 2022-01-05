using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGameObject : MonoBehaviour
{
    public GameObject obj;
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			obj.SetActive(true);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			obj.SetActive(false);
		}
	}
}
