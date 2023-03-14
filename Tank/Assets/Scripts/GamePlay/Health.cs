using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float lifes = 2;
    private bool isPlayer = false;
    private bool isInvincible = false;
    private LevelManager levelManager;
    public GameObject deathSmoke;
    // Start is called before the first frame update

    void Start()
    {
        levelManager = GameObject.FindGameObjectsWithTag("LevelManager")[0].GetComponent<LevelManager>();
        if(gameObject.tag == "Player")
        {
            isPlayer = true;
        }
        else if(gameObject.tag == "Enemy")
        {
            levelManager.numberOfEnemies++;
        }
    }
    public void TakeDamage(float damage)
    {
        if(isInvincible) return; // If the player is invincible, don't take damage
        lifes -= damage;
        if(lifes <= 0)
        {
            if(isPlayer)
            {
                // Game Over
                levelManager.LevelLose();
            }
            else if(gameObject.tag == "Enemy")
            {
                levelManager.OneEnemyDie();
                Instantiate(deathSmoke, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        if(isPlayer) // If the player is taking damage, make him invincible for 2 seconds
        {
            levelManager.UpdateLifeUI();
            StartCoroutine(InvincibleTiming());
        }
    }
    IEnumerator InvincibleTiming() // Flash the player when he is invincible after taking a shot
    {
        isInvincible = true;
        for(int i = 0; i <= 2;i++) // Invincible for 2 second
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1,0,0,1f);
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1,1f);
            yield return new WaitForSeconds(0.5f);
        }
        isInvincible = false;
        
    }
}
