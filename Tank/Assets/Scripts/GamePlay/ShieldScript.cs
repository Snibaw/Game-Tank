using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShieldScript : MonoBehaviour
{
    GameObject attachedTank;
    // Start is called before the first frame update
    void Start()
    {
        attachedTank = GetComponent<FixedJoint2D>().connectedBody.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(attachedTank == false)
        {
            Destroy(gameObject);
        }
    }
}
