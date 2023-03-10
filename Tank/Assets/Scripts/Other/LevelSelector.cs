using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Sprite LockButtonImage;
    public Button[] levelButtons;
    private PlayerStats playerStats;
    private void Start()
    {
        playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        playerStats.LoadPlayer();
        int highscore = playerStats.hightScoreLevel;
        for (int i = 0; i < levelButtons.Length; i++)
		{
			if (i + 1 > highscore)
			{
                levelButtons[i].GetComponent<Image>().sprite = LockButtonImage;
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                levelButtons[i].interactable = false;
			}
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
}
