using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeBetweenWaves, timeBetweenSpawns;
    public float minimumDistance, maxDistance;
    public Wave[] waves;
	public List<Transform> spawnLocations;
    public Transform enemySpawnLocationParent;
    public List<Transform> spawnsExcluded;
    public GameObject[] Enemies;
    public List<GameObject> enemiesAlive;

    //shop
    public GameObject shoppanel, shopUI;

    //privates
    int waveCount; 
    int totalWaveCount;
    int killAmount;

    [Header("player Stats")]
    public float money;
    public List<ShopUpgradeItem> HeldUpgrades;
    public UiItem selectedItemInUI;
    PlayerShop lastShop;
    ShopItem selectedItem;

    [Header("Scaling")]
    public float baseScaling = 1, scalingPerWave = 0.1f;
    float totalScaling;
    //item drops
    public float dropChancePerMinute = 1;
    float totalDropChance, dropChancePerSec;
    public WorldWeapon worldWeaponPrefab;
    public List<Weapon> dropItems;


    PlayerMovement player;
    int totalTimesThisSpawn;
    int totalSpawnsThisWave;
    int lastLocation;
    bool waveIsInProgress;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        foreach (Transform item in enemySpawnLocationParent)
        {
            spawnLocations.Add(item);
        }
        Clock();
        dropChancePerSec = dropChancePerMinute / 60;
        StartCoroutine(ItemDropChanceCalculator());
    }
    IEnumerator ItemDropChanceCalculator()
    {
        totalDropChance += dropChancePerSec;
        yield return new WaitForSeconds(1);
    }
    public void ResetDropChance()
    {
        totalDropChance = 0;
    }
    public void Clock()
    {
        StartCoroutine(SpawnEnemy());
    }
    public void EnemyDied(GameObject enemyThatDied)
    {
        killAmount++;
        if(enemiesAlive.Contains(enemyThatDied))
        {
            enemiesAlive.Remove(enemyThatDied);
        }
        if (enemiesAlive.Count <= 5 && waveIsInProgress)
        {
            WaveComplete();
        }
    }
    void WaveComplete()
    {
        waveCount++;
        totalWaveCount++;
        waveIsInProgress = false;
        Invoke(nameof(Clock), timeBetweenWaves);
        //give information to shops in world
        PlayerShop[] shops = FindObjectsOfType<PlayerShop>();
        foreach (var shop in shops)
        {
            shop.AfterWave();
        }
    }
    public IEnumerator SpawnEnemy()
    {
        totalScaling = baseScaling * ((scalingPerWave * totalWaveCount) + 1);
        if (!waveIsInProgress)
        {
            waveIsInProgress = true;
            //when max waves is reached, use previous one
            if (waveCount > waves.Length)
            {
                waveCount--;
            }
            int tempInt = waves[waveCount].totalSpawnAmount;
            totalSpawnsThisWave = (int)(tempInt * totalScaling);
            for (int z = 0; z < waves[waveCount].totalSpawnAmount; z++)
            {
                int roll = (int)Random.Range(waves[waveCount].spawnCluster.x, waves[waveCount].spawnCluster.y);
                for (int i = 0; i < roll; i++)
                {
                    if(totalSpawnsThisWave > -1)
                    {
                        totalSpawnsThisWave--;
                        GameObject tempEnemy = Instantiate(Enemies[waves[waveCount].spawnThis], GetSpawnPositionNearPlayer(), Quaternion.identity);
                        tempEnemy.GetComponent<EnemyHealth>().EnemySetup(totalScaling, totalDropChance, worldWeaponPrefab, dropItems);
                        enemiesAlive.Add(tempEnemy);
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
        for (int i = 0; i < spawnLocations.Count; i++)
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
    #region player stats
    public void GiveLastShop(PlayerShop _shop)
    {
        lastShop = _shop;
    }
    public void GiveSelectedItem(UiItem _selectedItem)
    {
        selectedItem = _selectedItem.heldItem;
        selectedItemInUI.Setup(selectedItem);
    }
    public void BuyItem()
    {
        if (money >= selectedItem.moneyValue)
        {
            if (selectedItem.itemType == ShopType.Upgrades)
            {
                money -= selectedItem.moneyValue;
                HeldUpgrades.Add(selectedItem as ShopUpgradeItem);
                CalculateStats();
            }
            else if (selectedItem.itemType == ShopType.Ammo)
            {
                ShopAmmo tempItem = selectedItem as ShopAmmo;
                player.GrantAmmo(tempItem.normalAmmoAmount, tempItem.specialAmmoAmount);
            }
            else if (selectedItem.itemType == ShopType.Health)
            {
                ShopHealth tempItem = selectedItem as ShopHealth;
                player.health.RecieveHealth(tempItem.healthAmount);
            }
            selectedItemInUI.Setup(null);
            RemoveItemFromShop(selectedItem);
        }
    }
    void RemoveItemFromShop(ShopItem _removeThis)
    {
        lastShop.RemoveItem(_removeThis);
    }
    void CalculateStats()
    {
        int total_pierces = 0;
        float total_damage = 0;
        float total_attackSpeed = 0;
        int total_ammo = 0;
        int total_health = 0;
        int total_bullets = 0;
        for (int i = 0; i < HeldUpgrades.Count; i++)
        {
            total_pierces += HeldUpgrades[i].stats.pierces;
            total_damage += HeldUpgrades[i].stats.damage;
            total_attackSpeed += HeldUpgrades[i].stats.attackSpeed;
            total_ammo += HeldUpgrades[i].stats.ammo;
            total_health += HeldUpgrades[i].stats.health;
            total_bullets += HeldUpgrades[i].stats.extraBullets;
        }
        player.GiveStats(total_pierces, total_damage, total_attackSpeed, total_ammo, total_health, total_bullets);
    }
    #endregion
}


[System.Serializable]
public class Wave
{
    public int spawnThis;
	public int totalSpawnAmount;
    public Vector2 spawnCluster;
}

