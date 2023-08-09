using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    //private PlayerStats playerStats;
    
    // PlayerPrefs Stats
    private int nbr_stars = 0;
    private int money = 0;
    private int highscore = 0;

    // Other
    public int numberOfEnemies;
    private int level;
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
    
    private GameObject settingsWindow;
    private string levelNameText;
    private GameObject[] enemies;
    private PauseButton pauseButton;
    
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        isEnd = false;
        scene = SceneManager.GetActiveScene();
        level = int.Parse(scene.name.Substring(5));
        playerHealth = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Health>();
        settingsWindow = GameObject.Find("SettingsWindow");
        settingsWindow.SetActive(false);
        pauseButton = GameObject.Find("PauseButton").GetComponent<PauseButton>();
        //playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
        //ResetSave();
        GetPlayerPrefs();
        
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
            foreach(GameObject enemy in enemies) // Add 1 hp to every enemies
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
        isEnd = true; // to prevent player from pressing escape after win
        isPaused = true; // to prevent player from rotation cannon after win
        TopText.text = levelNameText + " Completed";
        
        int lifesAfterWin = (int)playerHealth.lifes;
        if(nbr_stars < lifesAfterWin)
        {
            nbr_stars = lifesAfterWin;
        }
        if(highscore < level+1)
        {
            highscore = level+1;
        }
        
        SetPlayerPrefs();
        
        GetPlayerPrefs();
        
        if(highscore > level)
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
        if(highscore > level)
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
            pauseButton.ChangeImage(1);
            OpenPanel();
            TopText.text = "Pause";
            if(highscore > level)
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
            pauseButton.ChangeImage(0);
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
        if(DifficultyLevel.difficultyLevel == 0) // Mode Easy
        {
            for(int i = 0; i < stars.Length; i++)
            {
                if(i < nbr_stars)
                {
                    stars[i].SetActive(true);
                }
                else
                {
                    stars[i].SetActive(false);
                }
            }
        }
        else // Mode Hard
        {
            if(nbr_stars == 1)
            {
                stars[0].SetActive(true);
                stars[1].SetActive(true);
                stars[2].SetActive(true);
            }
            else
            {
                stars[0].SetActive(false);
                stars[1].SetActive(false);
                stars[2].SetActive(false);
            }
        }
        

    }
    public void OneEnemyDie()
    {
        numberOfEnemies--;
        if(DifficultyLevel.difficultyLevel == 0)
        {
            if(nbr_stars == 0)
            {
                money += 10;
            }
        }
        else if(DifficultyLevel.difficultyLevel == 1)
        {
            if(nbr_stars == 0)
            {
                money += 15;
            }
        }

    }
    private void GetPlayerPrefs()
    {
        money = PlayerPrefs.GetInt("money",0);
        if(DifficultyLevel.difficultyLevel == 0)
        {
            nbr_stars = PlayerPrefs.GetInt("nbr_stars"+level,0);
            highscore = PlayerPrefs.GetInt("highscore",1);
        }
        else if(DifficultyLevel.difficultyLevel == 1)
        {
            nbr_stars = PlayerPrefs.GetInt("nbr_starsHard"+level,0);
            highscore = PlayerPrefs.GetInt("highscoreHard",1);
        }       
    }
    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("money",money);
        if(DifficultyLevel.difficultyLevel == 0)
        {
            PlayerPrefs.SetInt("nbr_stars"+level,nbr_stars);
            PlayerPrefs.SetInt("highscore",highscore);
        }
        else if(DifficultyLevel.difficultyLevel == 1)
        {
            PlayerPrefs.SetInt("nbr_starsHard"+level,nbr_stars);
            PlayerPrefs.SetInt("highscoreHard",highscore);
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
}
