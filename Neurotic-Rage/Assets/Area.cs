using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
	public List<GameObject> enemies;
	private void Update()
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i] == null) 
			{
				enemies.Remove(enemies[i]);
			}
		}
		if (enemies.Count == 0)
		{
			Destroy(gameObject);
		}
	}
}
