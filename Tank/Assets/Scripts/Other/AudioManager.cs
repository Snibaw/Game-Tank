using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool isMainMenu = false;
    public AudioClip[] musicMainMenu;
    public AudioClip[] musicGamePlay;
    private AudioClip[] audioClips;
    private AudioSource audioSource;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(isMainMenu)
        {
            audioClips = musicMainMenu;
        }
        else
        {
            audioClips = musicGamePlay;
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClips[index];
        audioSource.Play();   
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            playNextSong();
        }   
    }
    private void playNextSong()
    {
        index = (index + 1) % audioClips.Length;
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
