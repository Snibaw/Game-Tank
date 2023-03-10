using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDeathSmoke : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySmoke());
    }

    IEnumerator DestroySmoke()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
