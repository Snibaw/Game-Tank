using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int[] upgradeCost;
    public string[] upgradeText;
    public GameObject UpgradeDescription;
    public TextMeshProUGUI UpgradeDescriptionText;
    public Button buyButton;
    private int indexButtonSelected = -1;
    public TextMeshProUGUI moneyText;
    public Button[] upgradeButtons;
    
    // Player Prefs
    private int money = 0;
    public int[] Upgrades = new int[15];
    // Start is called before the first frame update
    void Start()
    {
        
        buyButton.interactable = false;
        GetPlayerPrefs();
        
        moneyText.text = money.ToString();
        for(int i = 0; i < upgradeButtons.Length; i++)
        {
            if(upgradeCost.Length > i) 
            {
                upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[i].ToString();
            }
            else
            {
                upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "Null";
                upgradeButtons[i].interactable = false;
            }
            if(i == 1)
            {
                upgradeCost[1] = 200*(int)Mathf.Pow(2, Upgrades[1]);
                upgradeButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = upgradeCost[1].ToString();
                continue;
            }
            if(Upgrades[i] == 1)
            {
                upgradeButtons[i].interactable = false;
            }
        }
    }
    public void SelectButton(int index)
    {
        indexButtonSelected = index;
        if(upgradeText.Length > index)
        {
            UpgradeDescriptionText.text = upgradeText[index];
            if(money >= upgradeCost[index])
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }
        }
        else
        {
            UpgradeDescriptionText.text = "No description";
            buyButton.interactable = false;
        }
        UpgradeDescription.SetActive(true);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void BuyUpgrade()
    {
        money -= upgradeCost[indexButtonSelected];
        SetPlayerPrefs();
        if(indexButtonSelected == 1)
        {
            Upgrades[1] += 1;
            upgradeCost[1] *= 2;
            upgradeButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = (200*(int)Mathf.Pow(2, Upgrades[1])).ToString();
            if(money >= upgradeCost[1])
            {
                buyButton.interactable = true;
            }
            else
            {
                buyButton.interactable = false;
            }
        }
        else
        {
            buyButton.interactable = false;
             upgradeButtons[indexButtonSelected].interactable = false;
            Upgrades[indexButtonSelected] = 1;
        }
        SetPlayerPrefs();
        GetPlayerPrefs();
        moneyText.text = money.ToString();
        
       
    }
    private void GetPlayerPrefs()
    {
        money = PlayerPrefs.GetInt("money",0);
        for(int i =0; i< Upgrades.Length; i++)
        {
            Upgrades[i] = PlayerPrefs.GetInt("Upgrades"+i,0);
        }
    }
    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("money", money);
        for(int i=0; i< Upgrades.Length; i++)
        {
            PlayerPrefs.SetInt("Upgrades"+i, Upgrades[i]);
        }
    }
    private void ResetSave()
    {
        PlayerPrefs.SetInt("money", 0);
        PlayerPrefs.SetInt("highscore", 1);
        PlayerPrefs.SetInt("highscoreHard", 1);
        for(int i = 0; i <= 50; i++)
        {
            PlayerPrefs.SetInt("nbr_stars"+i, 0);
            PlayerPrefs.SetInt("nbr_starsHard"+i, 0);
        }
        for(int i = 0; i <= 15; i++)
        {
            PlayerPrefs.SetInt("Upgrades"+i, 0);
        }
    }
    public void AddMoney()
    {
        money += 10000;
        SetPlayerPrefs();
        moneyText.text = money.ToString();
    }
}
