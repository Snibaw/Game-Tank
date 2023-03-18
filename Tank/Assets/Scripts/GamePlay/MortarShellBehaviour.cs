using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarShellBehaviour : MonoBehaviour
{
    private GameObject playerObject;
    public GameObject mortarExplosion;
    private Vector2 playerPosition;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.Find("Player");
        playerPosition = playerObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MortarShoot()
    {
        
    }
}
