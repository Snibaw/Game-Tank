using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 0;
    public int money = 0;
    public int hightScoreLevel = 0;
    
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
    }
}
