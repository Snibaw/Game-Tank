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
    private int money;
    private int life;
    public TextMeshProUGUI TopText;
    public TextMeshProUGUI MidText;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI LifeText;
    public GameObject Panel;
    public Button NextLevelButton;
    public bool isPaused = false;
    public bool isEnd = false;
    
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        isEnd = false;
        playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        playerStats.LoadPlayer();
        playerStats.level = level;
        money = playerStats.money;
        life = playerStats.life;
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            LevelPause();
        }
    }
    private void LevelWin()
    {
        isEnd = true; // to prevent player from pressing escape after win
        isPaused = true; // to prevent player from rotation cannon after win
        TopText.text = "Level " + level + " Completed";
        OpenPanel();
        if(playerStats.hightScoreLevel < playerStats.level+1)
        {
            playerStats.hightScoreLevel = playerStats.level+1;
        }
        playerStats.SavePlayer();
        //Load win level screen
        
    }
    public void LevelLose()
    {
        isPaused = true; // to prevent player from rotation cannon after lose
        isEnd = true; // to prevent player from pressing escape after lose
        OpenPanel();
        if(playerStats.hightScoreLevel > level)
        {
            NextLevelButton.interactable = true;
        }
        else
        {
            NextLevelButton.interactable = false;
        }

    }
    public void LevelPause()
    {
        if(isEnd)
        {
            return;
        }
        isPaused = !isPaused;
        if(isPaused)
        {
            OpenPanel();
            TopText.text = "Pause";
            if(playerStats.hightScoreLevel > level)
            {
                NextLevelButton.interactable = true;
            }
            else
            {
                NextLevelButton.interactable = false;
            }
        }
        else
        {
            Time.timeScale = 1;
            Panel.SetActive(false);
        }
        
    }
    public void NextLevel()
    {
        int level2 = level+1;
        SceneManager.LoadScene("level"+level2);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("level" + level);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LevelSelector()
	{
		SceneManager.LoadScene("LevelSelect");
	}
    public void ResetSave()
    {
        playerStats.level = 1;
        playerStats.hightScoreLevel = 1;
        playerStats.SavePlayer();
    }
    private void OpenPanel()
    {
        Time.timeScale = 0;
        Panel.SetActive(true);
        MoneyText.text = money.ToString();
        LifeText.text = life.ToString();
        MidText.text = "Level " + level;
    }
}
