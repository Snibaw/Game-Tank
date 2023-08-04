using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombButton : MonoBehaviour
{
    private Player.PlayerShoot playerShoot;
    // Start is called before the first frame update
    void Start()
    {
        playerShoot = GameObject.Find("Player").GetComponent<Player.PlayerShoot>();
    }

    public void SpawnBomb()
    {
        playerShoot.TryToBomb();
    }
}
