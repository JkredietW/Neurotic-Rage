using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShop : InterActable
{
    [SerializeField] ShopType type;
    List<ShopItem> upgradeSlots;
    List<ShopItem> ammoSlots;
    List<ShopItem> healthSlots;

    public List<Transform> slotLocations;

    [SerializeField] float chanceForFirstItem = 100, chanceForSecondItem = 50, chanceForThirdItem = 20;

    int resetRoll;
    [SerializeField] int resetAfterWaveCount;
    //will be sscripable objects later
    public List<ShopItem> ItemTypes;
    [SerializeField] Discounts discountList;
    private void Awake()
    {
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
        resetRoll = Random.Range(3, 6);
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
                upgradeSlots[0] = ItemTypes[Random.Range(0, ItemTypes.Count)];
            }
            roll = Random.Range(0, 101);
            if (chanceForSecondItem >= roll)
            {
                upgradeSlots[1] = ItemTypes[Random.Range(0, ItemTypes.Count)];
            }
            roll = Random.Range(0, 101);
            if (chanceForThirdItem >= roll)
            {
                upgradeSlots[2] = ItemTypes[Random.Range(0, ItemTypes.Count)];
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
    //open shop here
    public override void OnPlayerEnter(PlayerMovement _thisOne)
    {
        base.OnPlayerEnter(_thisOne);
        player.ShopToggle(true, this);
    }
    public override void OnPlayerExit()
    {
        player.ShopToggle(false, null);
        base.OnPlayerExit();
    }
    public void ShopOpened()
    {
        //put items in slots
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
}