using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTargetScript : MonoBehaviour
{
    private float explosionTime ;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        explosionTime -= Time.deltaTime;
        if (explosionTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Initialise(float explosionTime)
    {
        this.explosionTime = explosionTime;
    }
}
