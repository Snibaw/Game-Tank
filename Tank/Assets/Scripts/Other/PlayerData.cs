using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int level;
    public int money;
    public int hightScoreLevel;
    public PlayerData(PlayerStats playerStats)
    {
        level = playerStats.level;
        money = playerStats.money;
        hightScoreLevel = playerStats.hightScoreLevel;
    }
}
