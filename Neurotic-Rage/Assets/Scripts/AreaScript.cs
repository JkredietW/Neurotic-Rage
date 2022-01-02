using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaScript : MonoBehaviour
{
	public int areaDistrict;
	private void Start()
	{
		//PlayerPrefs.SetInt("Area" + areaDistrict.ToString(), 1);
		if (PlayerPrefs.GetInt("Area" + areaDistrict.ToString()) == 2)
		{
			Destroy(gameObject);
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			if (PlayerPrefs.GetInt("Area" + areaDistrict.ToString())==1)
			{
				PlayerPrefs.SetInt("Area" + areaDistrict.ToString(),2);
				int discoverdAreas = PlayerPrefs.GetInt("DiscoverdArea");
				PlayerPrefs.SetInt("DiscoverdArea", discoverdAreas + 1);
				Destroy(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}
