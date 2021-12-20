using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class StatHolder
{
    #region player global stats
    public float total_deaths;
    public float total_competedRuns;
    public float total_competedWaves;
    public float thisgame_competedWaves;
    public float total_timePlayed;
    public float thisgame_timePlayed;
    #endregion

    #region player small stats
    public float total_damageDone;
    public float thisgame_damageDone;
    public float total_damageTaken;
    public float thisgame_damageTaken;
    public float total_damageHealed;
    public float thisgame_damageHealed;
    public float total_bulletsShot;
    public float thisgame_bulletsShot;
    public float total_bulletsHit;
    public float thisgame_bulletsHit;
    public float total_bulletsMissed;
    public float thisgame_bulletsMissed;
    public float total_timesReloaded;
    public float thisgame_timesReloaded;
    public float total_timeWastedNotShooting;
    public float thisgame_timeWastedNotShooting;
    public float total_timeWastedShooting;
    public float thisgame_timeWastedShooting;
    public float total_distanceWalked;
    public float thisgame_distanceWalked;
    public float total_highfives;
    public float thisgame_highfives;
    public float total_boxes;
    public float thisgame_boxes;
    #endregion

    #region money/shop
    public float total_moneyCollected;
    public float thisgame_moneyCollected;
    public float total_moneySpend;
    public float thisgame_moneySpend;
    public float thisgame_itemsBought;
    public float total_itemsBought;
    public float thisgame_ammoBought;
    public float total_ammoBought;
    public float thisgame_healthBought;
    public float total_healthBought;
    public float total_upgradesBought;
    public float thisgame_upgradesBought;
    public float total_shopsOpened;
    public float thisgame_shopsOpened;
    #endregion

    #region enemy
    public float total_kills;
    public float total_smallEnemyKills;
    public float total_mediumEnemyKills;
    public float total_bigEnemyKills;
    public float total_glitchEnemyKills;
    public float total_giantEnemyKills;
           
    public float thisgame_kills;
    public float thisgame_smallEnemyKills;
    public float thisgame_mediumEnemyKills;
    public float thisgame_bigEnemyKills;
    public float thisgame_glitchEnemyKills;
    public float thisgame_giantEnemyKills;
    #endregion
}