using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrassedHelli : MonoBehaviour
{
	private void Start()
	{
		float x = PlayerPrefs.GetFloat("CrashHelX");
		float y = PlayerPrefs.GetFloat("CrashHelY");
		float z = PlayerPrefs.GetFloat("CrashHelZ");

		transform.position = new Vector3(x, y, z);
		
		float xR = PlayerPrefs.GetFloat("CrashHelXRot");
		float yR = PlayerPrefs.GetFloat("CrashHelXRot");
		float zR = PlayerPrefs.GetFloat("CrashHelXRot");

		transform.eulerAngles = new Vector3(xR, yR, zR);
		
	}
}