using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int level;
    public int money;
    public int hightScoreLevel;
    public int[] nbr_stars = new int[50];
    public int[] Upgrades = new int[10];
    public PlayerData(PlayerStats playerStats)
    {
        level = playerStats.level;
        money = playerStats.money;
        hightScoreLevel = playerStats.hightScoreLevel;
    
        for(int i = 0; i < 50; i++)
        {
            nbr_stars[i] = playerStats.nbr_stars[i];
        }
        for (int i = 0; i < 10; i++)
        {
            Upgrades[i] = playerStats.Upgrades[i];
        }
    }
}
