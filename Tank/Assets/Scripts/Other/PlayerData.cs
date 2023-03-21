using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int level;
    public int money;
    public int hightScoreLevel;
    public int highScoreLevelHard;
    public int[] nbr_stars = new int[50];
    public int[] nbr_starsHard = new int[50];
    public int[] Upgrades = new int[15];
    public PlayerData(PlayerStats playerStats)
    {
        level = playerStats.level;
        money = playerStats.money;
        hightScoreLevel = playerStats.hightScoreLevel;
        highScoreLevelHard = playerStats.highScoreLevelHard;
        for(int i = 0; i < 50; i++)
        {
            nbr_stars[i] = playerStats.nbr_stars[i];
            nbr_starsHard[i] = playerStats.nbr_starsHard[i];
        }
        for (int i = 0; i < 15; i++)
        {
            Upgrades[i] = playerStats.Upgrades[i];
        }
    }
}
