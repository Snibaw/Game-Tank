using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private LevelManager levelManager;
    public Sprite[] Sprites;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void LevelPause()
    {
        levelManager.LevelPause();
    }
    public void ChangeImage(int i)
    {
        gameObject.GetComponent<Image>().sprite = Sprites[i];
    }
}
