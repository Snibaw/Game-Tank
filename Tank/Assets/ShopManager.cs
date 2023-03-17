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
    private PlayerStats playerStats;
    public TextMeshProUGUI moneyText;
    public Button[] upgradeButtons;
    // Start is called before the first frame update
    void Start()
    {
        
        buyButton.interactable = false;
        playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        playerStats.LoadPlayer();
        
        moneyText.text = playerStats.money.ToString();
        
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
            if(playerStats.Upgrades[i] == 1)
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
            if(playerStats.money >= upgradeCost[index])
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
        buyButton.interactable = false;
        playerStats.money -= upgradeCost[indexButtonSelected];
        playerStats.Upgrades[indexButtonSelected] = 1;
        playerStats.SavePlayer();
        playerStats.LoadPlayer();
        
        moneyText.text = playerStats.money.ToString();
        
        upgradeButtons[indexButtonSelected].interactable = false;
    }
    //Test function
    public void SavePlayer()
    {
        playerStats.SavePlayer();
    }
}
