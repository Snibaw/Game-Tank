using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 0;
    public int money = 0;
    public int hightScoreLevel = 0;
    public int[] nbr_stars = new int[50];
    public int[] Upgrades = new int[10];
    
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        
        level = data.level;
        money = data.money;
        hightScoreLevel = data.hightScoreLevel;
        for (int i = 0; i < 50; i++)
        {
            nbr_stars[i] = data.nbr_stars[i];
        }
        for (int i = 0; i < 10; i++)
        {
            Upgrades[i] = data.Upgrades[i];
        }
    }
}
