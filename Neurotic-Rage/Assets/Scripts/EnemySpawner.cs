using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[HideInInspector]
	public List<GameObject> enemyList;
	public float timeBetweenWaves;
	private bool newWave;
	private void Update()
	{
		for (int i = 0; i < enemyList.Count; i++)
		{
			if (enemyList[i] == null)
			{
				enemyList.Remove(enemyList[i]);
			}
		}
		if (enemyList.Count <= 0)
		{
			if (!newWave)
			{
				StartCoroutine(SpawnWave());
			}
		}
	}
	public IEnumerator SpawnWave()
	{
		newWave = true;
		//spawn hier weer meuk
		yield return new WaitForSeconds(timeBetweenWaves);
		newWave = false;
	}
}
