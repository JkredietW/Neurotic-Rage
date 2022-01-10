using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaScript : MonoBehaviour
{
	public int areaDistrict;
	[HideInInspector]
	public List<GameObject> setUnactive;
	private void Start()
	{
		PlayerPrefs.SetInt("Area" + areaDistrict.ToString(), 1);
		if (PlayerPrefs.GetInt("Area" + areaDistrict.ToString()) == 2)
		{
			Destroy(gameObject);
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			for (int i = 0; i < setUnactive.Count; i++)
			{
				setUnactive[i].SetActive(true);
			}
			if (PlayerPrefs.GetInt("Area" + areaDistrict.ToString()) == 1)
			{
				PlayerPrefs.SetInt("Area" + areaDistrict.ToString(), 2);
				int discoverdAreas = PlayerPrefs.GetInt("DiscoverdArea");
				PlayerPrefs.SetInt("DiscoverdArea", discoverdAreas + 1);
				Destroy(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
		else if (other.transform.CompareTag("Shop"))
		{
			setUnactive.Add(other.transform.gameObject);
			other.transform.gameObject.SetActive(false);
		}
		else if (other.transform.CompareTag("Weapon"))
		{
			setUnactive.Add(other.transform.gameObject);
			other.transform.gameObject.SetActive(false);
		}
	}
}
