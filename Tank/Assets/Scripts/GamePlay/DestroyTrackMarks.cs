using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrackMarks : MonoBehaviour
{
    // Start is called before the first frame update
    public float destroyTime = 2f;
    void Start()
    {
        StartCoroutine(DestroyTrack());
    }
    private IEnumerator DestroyTrack()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
