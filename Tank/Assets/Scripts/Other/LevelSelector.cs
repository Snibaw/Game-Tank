using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public GameObject[] Panel;
    public Button[] ButtonPanel;
    public Sprite LockButtonImage;
    public Button[] levelButtons;
    private PlayerStats playerStats;
    private GameObject[] button_Stars;
    private int[] nbr_stars = new int[50];
    private int activePanel = 0;
    private void Start()
    {
        //Panel
        ChangePanel(0);        
        
        //PlayerStats
        playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        playerStats.LoadPlayer();
        int highscore = playerStats.hightScoreLevel;
        for (int i = 0; i < 50; i++)
        {
            nbr_stars[i] = playerStats.nbr_stars[i];
        }
        
        //Lock Button
        for (int i = 0; i < levelButtons.Length; i++)
		{
            levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
			if (i + 1 > highscore)
			{
                levelButtons[i].GetComponent<Image>().sprite = LockButtonImage;
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                levelButtons[i].interactable = false;
			}
            //Update Stars
            UpdateStarsUI(i);
		}
    }
    public void LoadLevelPassed(string levelN)
    {
        SceneManager.LoadScene(levelN);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void UpdateStarsUI(int i)
    {
        button_Stars = levelButtons[i].GetComponent<StarContainer>().GetStar();
        for (int j = 0; j < button_Stars.Length; j++)
        {
            if (j < nbr_stars[i])
            {
                button_Stars[j].GetComponent<Image>().color = Color.white;
            }
            else
            {
                button_Stars[j].GetComponent<Image>().color = Color.black;
            }
        }
    }
    public void ChangePanel(int indexPanel)
    {
        activePanel = indexPanel;
        for(int i = 0; i < Panel.Length; i++)
        {
            if (i == indexPanel)
            {
                Panel[i].SetActive(true);
                ButtonPanel[i].interactable = false;
            }
            else
            {
                Panel[i].SetActive(false);
                ButtonPanel[i].interactable = true;
            }
        }
    }
}
