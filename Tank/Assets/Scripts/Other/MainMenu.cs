using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public int levelToLoad;

	public GameObject settingsWindow;

	public void StartGame()
	{
		SceneManager.LoadScene(levelToLoad);
	}


	public void SettingsButton()
	{
		settingsWindow.SetActive(value: true);
	}

	public void CloseSettingsWindow()
	{
		settingsWindow.SetActive(value: false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void LoadCreditsScene()
	{
		SceneManager.LoadScene("Credits");
	}
}
