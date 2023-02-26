using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float lifes = 2;
    // Start is called before the first frame update

    public void TakeDamage(float damage)
    {
        lifes -= damage;
        if(lifes <= 0)
        {
            Destroy(gameObject);
        }
    }
}
