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
    public List<ShopItem> upgradeTypes;
    [SerializeField] Discounts discountList;
    private void Awake()
    {
        upgradeSlots = new List<ShopItem>();
        for (int i = 0; i < 3; i++)
        {
            upgradeSlots.Add(default);
        }
        ammoSlots = new List<ShopItem>();
        healthSlots = new List<ShopItem>();
    }
    private void Start()
    {
        GameObject shoppanel = FindObjectOfType<GameManager>().shoppanel;
        foreach (Transform item in shoppanel.transform)
        {
            slotLocations.Add(item);
        }
        RollItems();
        resetRoll = Random.Range(0, 6);
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
                upgradeSlots[0] = upgradeTypes[0];
            }
            roll = Random.Range(0, 101);
            if (chanceForSecondItem >= roll)
            {
                upgradeSlots[1] = upgradeTypes[0];
            }
            roll = Random.Range(0, 101);
            if (chanceForThirdItem >= roll)
            {
                upgradeSlots[2] = upgradeTypes[0];
            }
        }
        else if (type == ShopType.Ammo || type == ShopType.Health)
        {
            //maybe needed later
        }
    }
    //open shop here
    public override void OnPlayerEnter(PlayerMovement _thisOne)
    {
        base.OnPlayerEnter(_thisOne);
        player.ShopToggle(true);
    }
    public override void OnPlayerExit()
    {
        player.ShopToggle(false);
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