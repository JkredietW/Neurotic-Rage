using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeBetweenWaves, timeBetweenSpawns;
    public float minimumDistance, maxDistance;
    public Wave presetWave;
	public List<Transform> spawnLocations;
    public Transform enemySpawnLocationParent;
    public List<Transform> spawnsExcluded;
    public GameObject[] Enemies;
    public List<GameObject> enemiesAlive;
    public List<GameObject> bossesAlive;
    private bool isBossRound;

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
    public float dropChancePerMinute = 0.5f;
    float totalDropChance, dropChancePerSec;
    public WorldWeapon worldWeaponPrefab;
    public List<Weapon> dropItems;


    PlayerMovement player;
    int totalTimesThisSpawn;
    int totalSpawnsThisWave;
    int lastLocation;
    bool waveIsInProgress;

    [SerializeReference]
    private float LastBosAmount;
    [SerializeReference]
    private float lastMiniBosAmount;
    [SerializeReference]
    private float lastBigEnemieAmount;
    [SerializeReference]
    private float lastMediumEnemieAmount;
    [SerializeReference]
    private float lastSmallEnemieAmount;

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
        StartCoroutine(ItemDropChanceCalculator());
    }
    public void ResetDropChance()
    {
        totalDropChance = 0;
    }
    public void Clock()
    {
        NewRound();
    }
    public void EnemyDied(GameObject enemyThatDied)
    {
        killAmount++;
        if(enemiesAlive.Contains(enemyThatDied))
        {
            enemiesAlive.Remove(enemyThatDied);
        }
		if (bossesAlive.Contains(enemyThatDied))
		{
            enemiesAlive.Remove(enemyThatDied);
        }
		if (!isBossRound)
		{
            if (enemiesAlive.Count <= 5 && waveIsInProgress)
            {
                WaveComplete();
            }
        }
		else
		{
            if (bossesAlive.Count <= 0 && waveIsInProgress)
            {
                //geef hiero item ding op enemydied positie
                WaveComplete();
            }
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
        if(selectedItem == null)
        {
            return;
        }
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
    public void NewRound()
	{
        waveIsInProgress = true;
        waveCount++;
        totalScaling = baseScaling * ((scalingPerWave * totalWaveCount) + 1);
        if (waveCount % 10 == 0)
        {
            print(waveCount % 10);
            StartCoroutine(BossRound());
            LastBosAmount *= 1.25f;
        }
		else
		{
            StartCoroutine(SpawnEnemies());
        }
        if (waveCount % 5 == 0)
		{
            lastMiniBosAmount *=1.05f;
        }
        if (waveCount % 4 == 0)
        {
            lastBigEnemieAmount *= 1.1f;
        }
        if (waveCount % 2 == 0)
        {
            lastMediumEnemieAmount *= 1.075f;
        }
        lastSmallEnemieAmount *= 1.2f;
    }
    public IEnumerator BossRound()
	{
		for (int i = 0; i < (int)LastBosAmount; i++)
		{
            Spawn(presetWave.boss, true);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
	}
    public IEnumerator SpawnEnemies()
	{
		for (int i = 0; i < (int)lastSmallEnemieAmount; i++)
		{
            Spawn(presetWave.small,false);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        for (int i = 0; i < (int)lastMediumEnemieAmount; i++)
        {
            Spawn(presetWave.medium, false);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        for (int i = 0; i < (int)lastBigEnemieAmount; i++)
        {
            Spawn(presetWave.big, false);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        for (int i = 0; i < (int)lastMiniBosAmount; i++)
        {
            Spawn(presetWave.minibos, false);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
    public void Spawn(GameObject gameObject,bool isBoss)
	{
        isBossRound = isBoss;
        GameObject tempEnemy = Instantiate(gameObject, GetSpawnPositionNearPlayer(), Quaternion.identity);
        tempEnemy.GetComponent<EnemyHealth>().EnemySetup(totalScaling, totalDropChance, worldWeaponPrefab, dropItems);
        if (!isBoss)
        {
            enemiesAlive.Add(tempEnemy);
            totalSpawnsThisWave++;
        }
		else
		{
            bossesAlive.Add(tempEnemy);
            totalSpawnsThisWave++;
        }
    }
    #endregion
}
[System.Serializable]
public class Wave
{
    public Vector2 spawnCluster;
    public GameObject big;
    public GameObject medium;
    public GameObject small;
    public GameObject minibos;
    public GameObject boss;
}

