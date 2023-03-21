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
    public Button[] levelNormalButtons;
    public Button[] levelHardButtons;
    private PlayerStats playerStats;
    private GameObject[] button_Stars;
    private int[] nbr_stars = new int[50];
    private int[] nbr_starsHard = new int[50];
    private int highscore;
    private int highscoreHard;
    private void Start()
    {
        //Panel
        ChangePanel(0);        
        DifficultyLevel.difficultyLevel = 0;
        
        //PlayerStats
        playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        playerStats.LoadPlayer();
        highscore = playerStats.hightScoreLevel;
        highscoreHard = playerStats.highScoreLevelHard;
        for (int i = 0; i < 50; i++)
        {
            nbr_stars[i] = playerStats.nbr_stars[i];
            nbr_starsHard[i] = playerStats.nbr_starsHard[i];
        }
        
        //Lock Button
        InitButtons();
    }
    public void LoadLevelPassed(string levelN)
    {
        SceneManager.LoadScene(levelN);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void UpdateStarsUI(int i, int difficultyLevel)
    {
        if(difficultyLevel == 0)
        {
            button_Stars = levelNormalButtons[i].GetComponent<StarContainer>().GetStar();
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
        else
        {
            button_Stars = levelHardButtons[i].GetComponent<StarContainer>().GetStar();
            for (int j = 0; j < button_Stars.Length; j++)
            {
                if (j < nbr_starsHard[i])
                {
                    button_Stars[j].GetComponent<Image>().color = Color.white;
                }
                else
                {
                    button_Stars[j].GetComponent<Image>().color = Color.black;
                }
            }
        }

    }
    public void ChangePanel(int indexPanel)
    {
        DifficultyLevel.difficultyLevel = indexPanel;
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
    private void InitButtons()
    {
        for (int i = 0; i < levelNormalButtons.Length; i++)
		{
            levelNormalButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
			if (i + 1 > highscore)
			{
                levelNormalButtons[i].GetComponent<Image>().sprite = LockButtonImage;
                levelNormalButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                levelNormalButtons[i].interactable = false;
			}
            //Update Stars
            UpdateStarsUI(i,0);
		}
        for (int i = 0; i < levelHardButtons.Length; i++)
		{
            levelHardButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = (i + 1).ToString();
			if (i + 1 > highscoreHard)
			{
                levelHardButtons[i].GetComponent<Image>().sprite = LockButtonImage;
                levelHardButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                levelHardButtons[i].interactable = false;
			}
            //Update Stars
            UpdateStarsUI(i,1);
		}
    }
}
