using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShieldScript : MonoBehaviour
{
    public GameObject attachedTank;
    Vector3 delay;
    // Start is called before the first frame update
    void Start()
    {
        delay = transform.position - attachedTank.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = attachedTank.transform.position + delay;
    }
}
