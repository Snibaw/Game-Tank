using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float lifes = 2;
    private bool isPlayer = false;
    private bool isInvincible = false;
    // Start is called before the first frame update

    void Start()
    {
        if(gameObject.tag == "Player")
        {
            isPlayer = true;
        }
    }
    public void TakeDamage(float damage)
    {
        if(isInvincible) return; // If the player is invincible, don't take damage
        if(isPlayer) // If the player is taking damage, make him invincible for 2 seconds
        {
            StartCoroutine(InvincibleTiming());
        }
        lifes -= damage;
        if(lifes <= 0)
        {
            Destroy(gameObject);
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
