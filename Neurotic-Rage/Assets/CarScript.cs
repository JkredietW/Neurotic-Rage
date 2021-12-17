using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
	public bool drive;
	public float speed;
	public GameObject otherPos;
	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerHealth>())
		{
			other.GetComponent<PlayerHealth>().CarHIt();
		}
	}
	private void Update()
	{
		if (drive)
		{
			transform.LookAt(otherPos.transform.position);
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, otherPos.transform.localPosition, Time.deltaTime * speed);
		}
	}
}
