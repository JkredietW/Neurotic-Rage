using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerHealth>())
		{
			other.GetComponent<PlayerHealth>().CarHIt();
		}
	}
}
