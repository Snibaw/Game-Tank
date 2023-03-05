using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private PlayerStats playerStats;
    public int numberOfEnemies;
    public int level;
    public TextMeshProUGUI TopText;
    public GameObject Panel;
    public Button NextLevelButton;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        playerStats.LoadPlayer();
        Debug.Log(playerStats.level);
        Debug.Log(playerStats.hightScoreLevel);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(numberOfEnemies == 0)
        {
            LevelWin();
        }
    }
    private void LevelWin()
    {
        Time.timeScale = 0;
        playerStats.level = level + 1;
        if(playerStats.hightScoreLevel < playerStats.level)
        {
            playerStats.hightScoreLevel = playerStats.level;
        }
        playerStats.SavePlayer();
        //Load win level screen
        Panel.SetActive(true);
        TopText.text = "Level " + level + " Completed";
    }
    public void LevelLose()
    {
        Time.timeScale = 0;
        //Load lose level screen
        Panel.SetActive(true);
        TopText.text = "Level " + level + " Failed";
        if(playerStats.hightScoreLevel > level)
        {
            NextLevelButton.interactable = true;
        }
        else
        {
            NextLevelButton.interactable = false;
        }

    }
    public void NextLevel()
    {
        SceneManager.LoadScene("level"+level+1);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("level" + level);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
	{
		Application.Quit();
	}
    public void ResetSave()
    {
        playerStats.level = 0;
        playerStats.hightScoreLevel = 0;
        playerStats.SavePlayer();
    }
}
