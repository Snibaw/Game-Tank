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
    private GameObject[] upgradeButtons;
    // Start is called before the first frame update
    void Start()
    {
        buyButton.interactable = false;
        playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        playerStats.LoadPlayer();
    
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
