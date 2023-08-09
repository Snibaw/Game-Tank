using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{

	public GameObject settingsWindow;
	public GameObject AutoAimButton;
	public GameObject AimAssistButton;
	public Slider aimAssistStrengthSlider;
	public TMP_Text aimAssistStrengthText;
	public Sprite[] tickSprites;
	

	private void Start()
	{
		settingsWindow.SetActive(false);

		AutoAimButton.GetComponent<Image>().sprite = tickSprites[PlayerPrefs.GetInt("AutoAim", 0)];
		AimAssistButton.GetComponent<Image>().sprite = tickSprites[PlayerPrefs.GetInt("AimAssist", 1)];
		aimAssistStrengthSlider.value = PlayerPrefs.GetFloat("AimAssistStrength", 0.5f);
		aimAssistStrengthText.text = aimAssistStrengthSlider.value.ToString("0.0");

	}
	public void TickAutoAim()
	{
		PlayerPrefs.SetInt("AutoAim", 1 - PlayerPrefs.GetInt("AutoAim", 0));
		AutoAimButton.GetComponent<Image>().sprite = tickSprites[PlayerPrefs.GetInt("AutoAim", 0)];
	}
	public void TickAimAssist()
	{
		PlayerPrefs.SetInt("AimAssist", 1 - PlayerPrefs.GetInt("AimAssist", 1));
		AimAssistButton.GetComponent<Image>().sprite = tickSprites[PlayerPrefs.GetInt("AimAssist", 1)];
	}
	public void SetAimAssistStrength()
	{
		PlayerPrefs.SetFloat("AimAssistStrength", aimAssistStrengthSlider.value);
		aimAssistStrengthText.text = aimAssistStrengthSlider.value.ToString("0.0");
	}

	public void StartGame()
	{
		SceneManager.LoadScene("LevelSelect");
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
	public void LoadShop()
	{
		SceneManager.LoadScene("Shop");
	}
}
