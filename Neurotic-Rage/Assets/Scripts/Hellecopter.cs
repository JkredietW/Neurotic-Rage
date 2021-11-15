using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellecopter : MonoBehaviour
{
    public float speed= 3;
	public float nextPointDis = 1.5f;
	public float timeForEnding;
	public LayerMask defaultLayer;
	public List<GameObject> route;
	public GameObject[] clouds;
	public GameObject player;
	public GameObject heliCam;
	public Rigidbody rb;
    public bool tokeOff;
	private bool crash;

    void Update()
    {
		if (tokeOff)
		{
			print(route.Count);
			if (route.Count == 0)
			{
				if (!crash)
				{
					Crash();
				}
			}
			else
			{
				transform.LookAt(new Vector3(route[0].transform.position.x,transform.position.y, route[0].transform.position.z));
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
		GetComponent<Animator>().enabled = false;
		crash = true;
		rb.useGravity = true;
		rb.isKinematic = false;
		Invoke("EndScene",timeForEnding);
	}
	public void EndScene()
	{
		PlayerPrefs.SetFloat("CrashHelX", transform.position.x);
		PlayerPrefs.SetFloat("CrashHelY", transform.position.y);
		PlayerPrefs.SetFloat("CrashHelZ", transform.position.z);

		PlayerPrefs.SetFloat("CrashHelXRot", transform.rotation.eulerAngles.x);
		PlayerPrefs.SetFloat("CrashHelYRot", transform.rotation.eulerAngles.y);
		PlayerPrefs.SetFloat("CrashHelZRot", transform.rotation.eulerAngles.z);
	}
    public void StartTakeOff()
	{
		player.transform.parent = transform;
		heliCam.SetActive(true);
		player.SetActive(false);
		for (int i = 0; i < clouds.Length; i++)
		{
			clouds[i].layer = defaultLayer;
		}
        tokeOff = true;
    }
}