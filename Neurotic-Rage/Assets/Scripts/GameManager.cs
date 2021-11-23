using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeBetweenWaves, timeBetweenSpawns;
    public float minimumDistance, maxDistance;
    public Wave[] waves;
	public Transform[] spawnLocations;
    public List<Transform> spawnsExcluded;
    public GameObject[] Enemies;
    public List<GameObject> enemiesAlive;

    //privates
    int waveCount;
    int totalWaveCount; //this one for stats <---
    int killAmount;
    float money;


    PlayerMovement player;
    int totalTimesThisSpawn;
    int totalSpawnsThisWave;
    int lastLocation;
    bool waveIsInProgress;
    bool checkIfWaveHasEnded;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        Clock();
    }
    public void Clock()
    {
        StartCoroutine(SpawnEnemy());
    }
    public void EnemyDied()
    {
        killAmount++;
        if (enemiesAlive.Count <= 5 && waveIsInProgress)
        {
            waveCount++;
            totalWaveCount++;
            waveIsInProgress = false;
            Invoke(nameof(Clock), timeBetweenWaves);
        }
    }
    public IEnumerator SpawnEnemy()
    {
        if (!waveIsInProgress)
        {
            waveIsInProgress = true;
            if (waves[waveCount].totalSpawnAmount == 0)
            {
                waveCount--;
            }
            totalSpawnsThisWave = waves[waveCount].totalSpawnAmount;
            for (int z = 0; z < waves[waveCount].totalSpawnAmount; z++)
            {
                int roll = (int)Random.Range(waves[waveCount].spawnCluster.x, waves[waveCount].spawnCluster.y);
                for (int i = 0; i < roll; i++)
                {
                    if(totalSpawnsThisWave > -1)
                    {
                        totalSpawnsThisWave--;
                        enemiesAlive.Add(Instantiate(Enemies[waves[waveCount].spawnThis], GetSpawnPositionNearPlayer(), Quaternion.identity));
                    }
                }
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }
    public Vector3 GetSpawnPositionNearPlayer()
    {
        if (totalTimesThisSpawn == 0)
        {
            RollLocation();
        }
        else
        {
            totalTimesThisSpawn--;
        }
        if(!spawnsExcluded.Contains(spawnLocations[lastLocation]))
        {
            spawnsExcluded.Add(spawnLocations[lastLocation]);
        }
        if (spawnsExcluded.Count > 3)
        {
            spawnsExcluded.RemoveRange(0, 1);
        }
        return spawnLocations[lastLocation].position;
    }
    void RollLocation()
    {
        totalTimesThisSpawn = Random.Range(3, 6);
        float closest = Mathf.Infinity;
        int thisOneinList = -1;
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            float distance = Vector3.Distance(player.transform.position, spawnLocations[i].position);
            if (distance > maxDistance || distance < minimumDistance || spawnsExcluded.Contains(spawnLocations[i]))
            {
                continue;
            }
            if (distance < closest)
            {
                closest = distance;
                thisOneinList = i;
                lastLocation = thisOneinList;
                continue;
            }
            if (thisOneinList == -1)
            {
                spawnsExcluded.RemoveRange(0, 1);
                RollLocation();
                return;
            }
        }
    }
}


[System.Serializable]
public class Wave
{
    public int spawnThis;
	public int totalSpawnAmount;
    public Vector2 spawnCluster;
}

