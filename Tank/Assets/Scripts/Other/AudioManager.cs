using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool isMainMenu = false;
    public AudioClip[] musicMainMenu;
    public AudioClip[] musicGamePlay;
    private AudioClip[] audioClips;
    private AudioSource audioSource;
    private int index = 0;
    public AudioMixerGroup soundEffectMixer;
    
    public static AudioManager instance;
    private void Awake() 
    {
        if(instance != null)
        {
            if(instance.isMainMenu == this.isMainMenu)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Destroy(instance.gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
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
    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource audioSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        audioSource.clip = clip; // define the clip
        audioSource.outputAudioMixerGroup = soundEffectMixer;
        audioSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return audioSource; // return the AudioSource reference
    }
}
