using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShop : InterActable
{
    [SerializeField] ShopType type;
    List<GameObject> ItemSlots;

    [SerializeField] float chanceForFirstItem = 100, chanceForSecondItem = 50, chanceForThirdItem = 20;

    int resetRoll;
    [SerializeField] int resetAfterWaveCount;
    [SerializeField] List<GameObject> UpgradeTypes;

    public enum ShopType
    {
        Health,
        Ammo,
        Upgrades,
    }
    private void Awake()
    {
        ItemSlots = new List<GameObject>(3);
        UpgradeTypes = new List<GameObject>();
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
        float roll = Random.Range(0, 101);
        if(chanceForFirstItem >= roll)
        {
            ItemSlots[0] = UpgradeTypes[Random.Range(0, UpgradeTypes.Count)];
        }
        roll = Random.Range(0, 101);
        if (chanceForSecondItem >= roll)
        {
            ItemSlots[1] = UpgradeTypes[Random.Range(0, UpgradeTypes.Count)];
        }
        roll = Random.Range(0, 101);
        if (chanceForThirdItem >= roll)
        {
            ItemSlots[2] = UpgradeTypes[Random.Range(0, UpgradeTypes.Count)];
        }
    }
    //open shop here
    public override void OnPlayerEnter(PlayerMovement _thisOne)
    {
        base.OnPlayerEnter(_thisOne);
    }
}
