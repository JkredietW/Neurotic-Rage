using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellecopter : MonoBehaviour
{
    public float speed=3;
    public float nextPointDis = 1.5f;
    public List<GameObject> route;
	public Rigidbody rb;
    public bool tokeOff;
	private bool crash;

    void Update()
    {
		if (tokeOff)
		{
			if (route.Count == 0)
			{
				if (!crash)
				{
					Crash();
				}
			}
			else
			{
				float dist = Vector3.Distance(transform.position, route[0].transform.position);
				if (dist <= nextPointDis)
				{
					route.Remove(route[0]);
				}

				transform.position = Vector3.MoveTowards(transform.position, route[0].transform.position, speed);
			}
		}
    }
	public void Crash()
	{
		crash = true;
		rb.useGravity = true;
		rb.isKinematic = false;
	}
    public void StartTakeOff()
	{
        tokeOff = true;
    }
}
