using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShop : InterActable
{
    [SerializeField] ShopType type;
    List<ShopItem> upgradeSlots;
    List<ShopItem> ammoSlots;
    List<ShopItem> healthSlots;

    [SerializeField] float chanceForFirstItem = 100, chanceForSecondItem = 50, chanceForThirdItem = 20;

    int resetRoll;
    [SerializeField] int resetAfterWaveCount;
    //will be sscripable objects later
    [SerializeField] List<ShopItem> upgradeTypes;
    [SerializeField] Discounts discountList;
    private void Awake()
    {
        upgradeSlots = new List<ShopItem>(3);
        ammoSlots = new List<ShopItem>(4);
        healthSlots = new List<ShopItem>(4);
        upgradeTypes = new List<ShopItem>();
    }
    private void Start()
    {
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
                upgradeSlots[0] = upgradeTypes[Random.Range(0, upgradeTypes.Count)];
            }
            roll = Random.Range(0, 101);
            if (chanceForSecondItem >= roll)
            {
                upgradeSlots[1] = upgradeTypes[Random.Range(0, upgradeTypes.Count)];
            }
            roll = Random.Range(0, 101);
            if (chanceForThirdItem >= roll)
            {
                upgradeSlots[2] = upgradeTypes[Random.Range(0, upgradeTypes.Count)];
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