using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[HideInInspector]
	public int waves;
	[HideInInspector]
	public float time;
	[HideInInspector]		
	public int money;
	private void Update()
	{
		time = Time.deltaTime;
	}
}
