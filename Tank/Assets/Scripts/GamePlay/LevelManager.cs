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
    private int level;
    private int money;
    public TextMeshProUGUI TopText;
    public TextMeshProUGUI MidText;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI LifeText;
    public GameObject Panel;
    public Button NextLevelButton;
    public bool isPaused = false;
    public bool isEnd = false;
    private GameObject[] lifeUI;
    private Health playerHealth;
    private Scene scene;
    public GameObject[] stars;
    
    public GameObject settingsWindow;
    private string levelNameText;
    private GameObject[] enemies;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("DifficultyLevel.difficultyLevel: " + DifficultyLevel.difficultyLevel);
        isPaused = false;
        isEnd = false;
        scene = SceneManager.GetActiveScene();
        level = int.Parse(scene.name.Substring(5));
        playerHealth = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Health>();
        playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        //ResetSave();
        LoadPlayerStats();
        playerStats.level = level;
        lifeUI = GameObject.FindGameObjectsWithTag("LifeUI");
        
        if(DifficultyLevel.difficultyLevel == 0) // Mode Easy
        {
            levelNameText = "Level " + level;
        }
        else // Mode Hard
        {
            levelNameText = "Level " + level + " Hard";
            playerHealth.lifes = 1;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies)
            {
                Health enemyHealth = enemy.GetComponent<Health>();
                if(enemyHealth != null)
                {
                    enemyHealth.lifes = enemyHealth.lifes +1;
                }
            }
        }
        
        UpdateStarUI();
        UpdateLifeUI();
        Debug.Log("playerStats.level: " + playerStats.level+ "playerStats.hightScoreLevel: " + playerStats.hightScoreLevel);
        Debug.Log("money: " + money);
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
        TopText.text = levelNameText + " Completed";
        int lifesAfterWin = (int)playerHealth.lifes;
        if(playerStats.nbr_stars[level-1] < lifesAfterWin)
        {
            playerStats.nbr_stars[level-1] = lifesAfterWin;
        }
        if(playerStats.hightScoreLevel < playerStats.level+1)
        {
            playerStats.hightScoreLevel = playerStats.level+1;
        }
        playerStats.SavePlayer();
        
        
        
        LoadPlayerStats();
        if(playerStats.hightScoreLevel > level)
        {
            NextLevelButton.interactable = true;
        }
        else
        {
            NextLevelButton.interactable = false;
        }
        UpdateStarUI();
        OpenPanel();
        
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
            CloseSettingsWindow();
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
    public void Shop()
	{
		SceneManager.LoadScene("Shop");
	}
    public void ResetSave()
    {
        playerStats.level = 1;
        playerStats.hightScoreLevel = 1;
        playerStats.money = 0;
        playerStats.nbr_stars = new int[50];
        playerStats.Upgrades = new int[15];
        playerStats.SavePlayer();
    }
    public void SettingsButton()
	{
		settingsWindow.SetActive(value: true);
	}

	public void CloseSettingsWindow()
	{
		settingsWindow.SetActive(value: false);
	}
    private void OpenPanel()
    {
        Time.timeScale = 0;
        Panel.SetActive(true);
        MoneyText.text = money.ToString();
        LifeText.text = playerHealth.lifes.ToString();
        MidText.text = levelNameText;
    }
    public void UpdateLifeUI()
    {
        for(int i = 0; i < lifeUI.Length; i++)
        {
            if(i < playerHealth.lifes)
            {
                lifeUI[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                lifeUI[i].GetComponent<Image>().color = Color.black;
            }
        }
    }
    private void UpdateStarUI()
    {
        for(int i = 0; i < stars.Length; i++)
        {
            if(i < playerStats.nbr_stars[level-1])
            {
                stars[i].SetActive(true);
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
    }
    public void OneEnemyDie()
    {
        numberOfEnemies--;
        if(playerStats.nbr_stars[level-1] == 0)
        {
            playerStats.money += 10;
        }
    }
    private void LoadPlayerStats()
    {
        playerStats.LoadPlayer();
        money = playerStats.money;
    }
}
