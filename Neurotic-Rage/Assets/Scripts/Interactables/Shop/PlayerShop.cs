using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerShop : InterActable
{
    [SerializeField] ShopType type;
    List<ShopItem> upgradeSlots;
    List<ShopItem> ammoSlots;
    List<ShopItem> healthSlots;

    public List<Transform> slotLocations;

    [SerializeField] float chanceForFirstItem = 100, chanceForSecondItem = 50, chanceForThirdItem = 20;

    [HideInInspector] public int resetRoll;
    [SerializeField] int resetAfterWaveCount;
    //will be sscripable objects later
    public List<ShopItem> ItemTypes;
    [SerializeField] Discounts discountList;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        upgradeSlots = new List<ShopItem>();
        for (int i = 0; i < 3; i++)
        {
            upgradeSlots.Add(default);
        }
        ammoSlots = new List<ShopItem>();
        for (int i = 0; i < 6; i++)
        {
            ammoSlots.Add(default);
        }
        healthSlots = new List<ShopItem>();
        for (int i = 0; i < 4; i++)
        {
            healthSlots.Add(default);
        }
    }
    private void Start()
    {
        GameObject shoppanel = FindObjectOfType<GameManager>().shoppanel;
        foreach (Transform item in shoppanel.transform)
        {
            slotLocations.Add(item);
        }
        RollItems();
        resetRoll = Random.Range(5, 15);
    }
    public void AfterWave()
    {
        resetRoll--;
        if(resetRoll == 0)
        {
            RollItems();
            resetRoll = resetAfterWaveCount;
        }
    }
    void RollItems()
    {
        if (type == ShopType.Upgrades)
        {
            float roll = Random.Range(0, 101);
            if (chanceForFirstItem >= roll)
            {
                ShopItem item = ItemTypes[Random.Range(0, ItemTypes.Count)];
                if(item.itemType == ShopType.Random)
                {
                    RandomizeItem(item);
                }
                upgradeSlots[0] = item;
            }
            roll = Random.Range(0, 101);
            if (chanceForSecondItem >= roll)
            {
                ShopItem item = ItemTypes[Random.Range(0, ItemTypes.Count)];
                if (item.itemType == ShopType.Random)
                {
                    RandomizeItem(item);
                }
                upgradeSlots[1] = item;
            }
            roll = Random.Range(0, 101);
            if (chanceForThirdItem >= roll)
            {
                ShopItem item = ItemTypes[Random.Range(0, ItemTypes.Count)];
                if (item.itemType == ShopType.Random)
                {
                    RandomizeItem(item);
                }
                upgradeSlots[2] = item;
            }
        }
        else if (type == ShopType.Ammo)
        {
            //normal ammo
            ammoSlots[0] = ItemTypes[0];
            ammoSlots[1] = ItemTypes[1];
            ammoSlots[2] = ItemTypes[Random.Range(2, 4)];
            //special ammo
            ammoSlots[3] = ItemTypes[4];
            ammoSlots[4] = ItemTypes[5];
            ammoSlots[5] = ItemTypes[Random.Range(6, 8)];
        }
        else if(type == ShopType.Health)
        {
            ammoSlots[0] = ItemTypes[0];
            ammoSlots[1] = ItemTypes[1];
            ammoSlots[2] = ItemTypes[2];
            ammoSlots[3] = ItemTypes[3];
        }
    }
    ShopItem RandomizeItem(ShopItem _item)
    {
        ShopUpgradeItem randomizedItem = _item as ShopUpgradeItem;
        randomizedItem.stats.attackSpeed = Random.Range(-10, 11);
        randomizedItem.stats.damage = Random.Range(-50, 51);
        randomizedItem.stats.ammo = Random.Range(-500, 501);
        randomizedItem.stats.health = Random.Range(-100, 101);
        randomizedItem.stats.pierces = Random.Range(-5, 6);
        randomizedItem.stats.extraBullets = Random.Range(-5, 6);
        return randomizedItem;
    }
    //open shop here
    public override void OnPlayerEnter(PlayerMovement _thisOne)
    {
        base.OnPlayerEnter(_thisOne);
        player.ShopToggle(true, this);
        FindObjectOfType<GameManager>().GiveLastShop(this);
    }
    public override void OnPlayerExit()
    {
        player.ShopToggle(false, null);
        base.OnPlayerExit();
    }
    public void ShopOpened()
    {
        //put items in slots
        source.Play();
        if (type == ShopType.Upgrades)
        {
            for (int i = 0; i < slotLocations.Count; i++)
            {
                if (i < upgradeSlots.Count && upgradeSlots[i] != null)
                {
                    slotLocations[i].GetComponent<UiItem>().Setup(upgradeSlots[i]);
                }
                else
                {
                    slotLocations[i].GetComponent<UiItem>().Setup(default);
                }
            }
        }
        else if (type == ShopType.Ammo)
        {
            for (int i = 0; i < slotLocations.Count; i++)
            {
                if (i < ammoSlots.Count && ammoSlots[i] != null)
                {
                    slotLocations[i].GetComponent<UiItem>().Setup(ammoSlots[i]);
                }
                else
                {
                    slotLocations[i].GetComponent<UiItem>().Setup(default);
                }
            }
        }
        else if (type == ShopType.Health)
        {
            for (int i = 0; i < slotLocations.Count; i++)
            {
                if (i < healthSlots.Count && healthSlots[i] != null)
                {
                    slotLocations[i].GetComponent<UiItem>().Setup(healthSlots[i]);
                }
                else
                {
                    slotLocations[i].GetComponent<UiItem>().Setup(default);
                }
            }
        }
    }
    public void RemoveItem(ShopItem _item)
    {
        if(_item.itemType == ShopType.Upgrades || _item.itemType == ShopType.Random)
        {
            upgradeSlots.Remove(_item);
        }
        else if(_item.itemType == ShopType.Ammo)
        {
            ammoSlots.Remove(_item);
        }
        else if(_item.itemType == ShopType.Health)
        {
            healthSlots.Remove(_item);
        }
        ShopOpened();
    }
}
[System.Serializable]
public class Discounts
{
    public float minimumDiscount;
    public float mediumDiscount;
    public float highDiscount;
    public float sexyDiscount;
}
public enum ShopType
{
    Health,
    Ammo,
    Upgrades,
    Random,
}