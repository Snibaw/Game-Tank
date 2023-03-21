using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mortar_Enemy : MonoBehaviour
{
    private int activeExplosions = 0;
    private float timerReload;
    private Vector3 playerOldPosition;
    private Vector3 playerNewPosition;
    public GameObject mortarExplosion;
    public GameObject mortarTarget;
    public GameObject mortarExplosion2;
    AudioSource boomSound;
    [SerializeField] public GameObject player;
    [SerializeField] private float reloadTime = 5f;
    [SerializeField] private float explosionTime = 1f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private AudioClip blastOffSound;
    [SerializeField] private AudioClip landSound;




    private float timerExplosion;
    void Start()
    {
        timerReload = reloadTime ;
        timerExplosion = 100f;
        boomSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timerReload -= Time.deltaTime;
        timerExplosion -= Time.deltaTime;
        if (timerReload <= 0)
        {
            timerExplosion = explosionTime;
            timerReload = reloadTime;
            playerOldPosition = player.transform.position;
            GameObject target = Instantiate(mortarTarget, playerOldPosition, Quaternion.identity);
            target.GetComponent<MortarTargetScript>().Initialise(explosionTime);
            Instantiate(mortarExplosion, transform.position, Quaternion.identity);
            activeExplosions++;
            boomSound.PlayOneShot(blastOffSound);
        }
        if(timerExplosion <= 0 && activeExplosions > 0)
        {
            Instantiate(mortarExplosion2, playerOldPosition, Quaternion.identity);
            activeExplosions--;
            playerNewPosition = player.transform.position;
            boomSound.PlayOneShot(landSound);
            if(Math.Sqrt(Math.Pow(playerOldPosition.x - playerNewPosition.x, 2) + Math.Pow(playerOldPosition.y - playerNewPosition.y, 2)) < explosionRadius)
            {
                Debug.Log("Player hit");
                player.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }
    
    
   
}
