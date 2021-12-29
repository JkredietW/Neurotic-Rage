using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float delayTime;
    public float timeBetweenWaves, timeBetweenSpawns;
    public float minimumDistance, maxDistance;
    public GameObject playerCanvas,momDiedCanvas,car;
    public Wave presetWave;
    public LoadingScreen ls;
	public List<Transform> spawnLocations;
    public Transform enemySpawnLocationParent;
    public List<Transform> spawnsExcluded;
    public GameObject[] Enemies;
    public List<GameObject> enemiesAlive;
    public List<GameObject> bossesAlive;
    public AudioMixer mixer;
    private bool isBossRound,pauseWave;

    //shop
    public GameObject shoppanel, shopUI;
    [SerializeField] Transform descriptionParent;
    [SerializeField] List<TextMeshProUGUI> descriptionText;
    [SerializeField] TextMeshProUGUI moneyText, waveText, shopResetCountText;

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
    private float lastBigEnemieAmount;
    [SerializeReference]
    private float lastMediumEnemieAmount;
    [SerializeReference]
    private float lastSmallEnemieAmount;

    //stats
    public StatHolder statsScript;
    float time;

    private void Start()
    {
        mixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat("Master")));
        mixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music")));
        mixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFX")));
        mixer.SetFloat("UI", Mathf.Log10(PlayerPrefs.GetFloat("UI")));
        time = 0;
    }
    public void Save()
    {
        string totalplayerstats = JsonUtility.ToJson(statsScript);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/Stats.json", totalplayerstats);
    }
    public void GetSaves()
    {
        if(System.IO.File.ReadAllText(Application.persistentDataPath + "/Stats.json") != null)
        {
            string data = System.IO.File.ReadAllText(Application.persistentDataPath + "/Stats.json");
            StatHolder _statsScript = JsonUtility.FromJson<StatHolder>(data);
            statsScript = _statsScript;
        }
    }
    private void Update()
    {
        time = 1 * Time.deltaTime;
        if (Keyboard.current.enterKey.IsPressed())
        {
            Save();
        }
    }
    public void DelayedStart()
	{
        statsScript = new StatHolder();
        GetSaves();
        player = FindObjectOfType<PlayerMovement>();
        foreach (Transform item in enemySpawnLocationParent)
        {
            spawnLocations.Add(item);
        }
        Clock();
        dropChancePerSec = dropChancePerMinute / 60;
        StartCoroutine(ItemDropChanceCalculator());

        //get description texts
        descriptionText = new List<TextMeshProUGUI>();
        foreach (Transform item in descriptionParent)
        {
            descriptionText.Add(item.GetComponent<TextMeshProUGUI>());
        }

        //waves/money to text and wave + 1
        totalWaveCount++;
        UpdateTexts();
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
    void GiveMoney(float _money)
    {
        statsScript.thisgame_moneyCollected += _money;
        statsScript.total_moneyCollected += _money;
        money += _money;
        UpdateTexts();
    }
    public void EnemyDied(GameObject enemyThatDied)
    {
		if (pauseWave)
		{
            return;
		}
        killAmount++;
        if(enemiesAlive.Contains(enemyThatDied))
        {
            GiveMoney(2 * totalWaveCount);
            enemiesAlive.Remove(enemyThatDied);
        }
		if (bossesAlive.Contains(enemyThatDied))
		{
            GiveMoney(10 * totalWaveCount);
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
        if (pauseWave)
        {
            return;
        }
        statsScript.thisgame_competedWaves++;
        statsScript.total_competedWaves++;
        totalWaveCount++;
        UpdateTexts();
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
    public void MomDied()
	{
        FindObjectOfType<PlayerMovement>().MayMove(false);
        playerCanvas.SetActive(false);
        momDiedCanvas.SetActive(true);
        pauseWave = true;
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i].GetComponent<EnemyHealth>())
            {
                enemyList[i].transform.GetComponent<EnemyHealth>().PlayerDied();
            }
        }
        Invoke("KillEnemies", 2.5f);
    }
    public void KillEnemies()
	{
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i].GetComponent<EnemyHealth>())
            {
                enemyList[i].transform.GetComponent<EnemyHealth>().PlayerDied();
            }
        }
    }
    public void ContinueEndless()
	{
        enemiesAlive.Clear();
        bossesAlive.Clear();
        FindObjectOfType<PlayerMovement>().MayMove(true);
        momDiedCanvas.SetActive(false);
        playerCanvas.SetActive(true);
        pauseWave = false;
        WaveComplete();
    }
    public void EndEnless()
	{
        statsScript.total_competedRuns++;
        statsScript.thisgame_timePlayed += time;
        statsScript.total_timePlayed += time;
        Save();
        StartCoroutine(EndWave());
	}
    public IEnumerator EndWave()
	{
        car.SetActive(true);
        car.GetComponent<CarScript>().drive = true;
        momDiedCanvas.SetActive(false);
        yield return new WaitForSeconds(5);
        FindObjectOfType<FadeToFromBlack>().FadeToBlack(2);
        yield return new WaitForSeconds(1);
        ls.ChargementScene(0);
	}
    #region player stats
    public void GiveLastShop(PlayerShop _shop)
    {
        lastShop = _shop;
    }
    public void GiveSelectedItem(UiItem _selectedItem)
    {
        if(_selectedItem.heldItem == null)
        {
            UpdateDescriptions();
            return;
        }
        selectedItem = _selectedItem.heldItem;
        selectedItemInUI.Setup(selectedItem);
        UpdateDescriptions();
    }
    void UpdateDescriptions()
    {
        if(selectedItemInUI.heldItem == null || selectedItem == null)
        {
            for (int i = 0; i < descriptionText.Count; i++)
            {
                descriptionText[i].gameObject.SetActive(false);
            }
            return;
        }
        //item discriptions
        if (selectedItem.itemType == ShopType.Upgrades || selectedItem.itemType == ShopType.Random)
        {
            ShopUpgradeItem item = selectedItem as ShopUpgradeItem;

            //filter out values
            #region attackspeed
            if (item.stats.attackSpeed != 0)
            {
                descriptionText[0].gameObject.SetActive(true);
                descriptionText[0].text = $"Attack speed : {item.stats.attackSpeed}";
            }
            else
            {
                descriptionText[0].gameObject.SetActive(false);
            }
            #endregion
            #region damage
            if (item.stats.damage != 0)
            {
                descriptionText[1].gameObject.SetActive(true);
                descriptionText[1].text = $"Damage : {item.stats.damage}";
            }
            else
            {
                descriptionText[1].gameObject.SetActive(false);
            }
            #endregion
            #region ammo
            if (item.stats.ammo != 0)
            {
                descriptionText[2].gameObject.SetActive(true);
                descriptionText[2].text = $"Max ammo : {item.stats.ammo}";
            }
            else
            {
                descriptionText[2].gameObject.SetActive(false);
            }
            #endregion
            #region pierce
            if (item.stats.pierces != 0)
            {
                descriptionText[3].gameObject.SetActive(true);
                descriptionText[3].text = $"Pierces : {item.stats.pierces}";
            }
            else
            {
                descriptionText[3].gameObject.SetActive(false);
            }
            #endregion
            #region bullets
            if (item.stats.extraBullets != 0)
            {
                descriptionText[4].gameObject.SetActive(true);
                descriptionText[4].text = $"Bullets : {item.stats.extraBullets}";
            }
            else
            {
                descriptionText[4].gameObject.SetActive(false);
            }
            #endregion
            #region health
            if (item.stats.health != 0)
            {
                descriptionText[5].gameObject.SetActive(true);
                descriptionText[5].text = $"Max health : {item.stats.health}";
            }
            else
            {
                descriptionText[5].gameObject.SetActive(false);
            }
            #endregion
        }
        else if(selectedItem.itemType == ShopType.Ammo)
        {
            ShopAmmo item = selectedItem as ShopAmmo;

            if(item.normalAmmoAmount != 0)
            {
                descriptionText[0].gameObject.SetActive(true);
                descriptionText[0].text = $"Restore light ammo : {item.normalAmmoAmount}";
            }
            else
            {
                descriptionText[0].gameObject.SetActive(false);
            }
            if (item.normalAmmoAmount != 0)
            {
                descriptionText[1].gameObject.SetActive(true);
                descriptionText[1].text = $"Restore heavy ammo : {item.specialAmmoAmount}";
            }
            else
            {
                descriptionText[1].gameObject.SetActive(false);
            }
        }
        else if (selectedItem.itemType == ShopType.Health)
        {
            ShopHealth item = selectedItem as ShopHealth;

            descriptionText[0].gameObject.SetActive(true);
            descriptionText[0].text = $"Restore health : {item.healthAmount}";
        }
    }
    public void BuyItem()
    {
        if(selectedItem == null)
        {
            return;
        }
        if (money >= selectedItem.moneyValue)
        {
            money -= selectedItem.moneyValue;
            statsScript.thisgame_moneySpend += selectedItem.moneyValue;
            statsScript.total_moneySpend += selectedItem.moneyValue;
            if (selectedItem.itemType == ShopType.Upgrades || selectedItem.itemType == ShopType.Random)
            {
                statsScript.thisgame_upgradesBought++;
                statsScript.total_upgradesBought++;
                HeldUpgrades.Add(selectedItem as ShopUpgradeItem);
                UpdateTexts();
                CalculateStats();
            }
            else if (selectedItem.itemType == ShopType.Ammo)
            {
                statsScript.thisgame_healthBought++;
                statsScript.total_healthBought++;
                ShopAmmo tempItem = selectedItem as ShopAmmo;
                player.GrantAmmo(tempItem.normalAmmoAmount, tempItem.specialAmmoAmount);
            }
            else if (selectedItem.itemType == ShopType.Health)
            {
                statsScript.thisgame_ammoBought++;
                statsScript.total_ammoBought++;
                ShopHealth tempItem = selectedItem as ShopHealth;
                statsScript.thisgame_damageHealed += tempItem.healthAmount;
                statsScript.thisgame_damageHealed += tempItem.healthAmount;
                player.health.RecieveHealth(tempItem.healthAmount);
            }
            selectedItemInUI.Setup(null);
            RemoveItemFromShop(selectedItem);
        }
        UpdateDescriptions();
    }
    void UpdateTexts()
    {
        moneyText.text = $"Money : {money}";
        waveText.text = $"Waves : {totalWaveCount}";
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
        if (pauseWave)
		{
            return;
		}
        waveIsInProgress = true;
        waveCount++;
        totalScaling = baseScaling * ((scalingPerWave * totalWaveCount) + 1);
        if (waveCount % 10 == 0)
        {      
            StartCoroutine(BossRound());
            LastBosAmount *= 1.25f;
        }
		else
		{
            StartCoroutine(SpawnEnemies());
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
		if (pauseWave)
		{
            yield break;
		}
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
    public void ShopIsOpened(PlayerShop _shop)
    {
        shopResetCountText.text = $"Reset after :{_shop.resetRoll} waves";
    }
    private void OnApplicationQuit()
    {
        statsScript.total_timePlayed += time;
        statsScript.thisgame_timeWastedNotShooting += player.timeWhileNotShooting;
        statsScript.total_timeWastedNotShooting += player.timeWhileNotShooting;
        statsScript.thisgame_timeWastedShooting += player.timeWhileShooting;
        statsScript.total_timeWastedShooting += player.timeWhileShooting;
        statsScript.thisgame_distanceWalked += player.distanceWalked;
        statsScript.total_distanceWalked += player.distanceWalked;
        Save();
    }
    public void TurnOffObj(GameObject obj)
	{
        obj.SetActive(false);
	}
}
[System.Serializable]
public class Wave
{
    public Vector2 spawnCluster;
    public GameObject big;
    public GameObject medium;
    public GameObject small;
    public GameObject boss;
}

