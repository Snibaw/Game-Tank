using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireTrackSpawner : MonoBehaviour
{
    public GameObject tireTrackPrefab;
    [SerializeField] private float tireTrackSpawnTimer = 0.5f;
    private float timer = 0f;
    private GameObject Parent;
    private Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        timer = tireTrackSpawnTimer;
        Parent = this.gameObject.transform.parent.gameObject;
        lastPosition = Parent.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        timer-= Time.deltaTime;
        if(lastPosition != Parent.transform.position)
        {
            lastPosition = Parent.transform.position;
            if(timer <= 0)
            {
                timer = tireTrackSpawnTimer;
                SpawnTireTrack();
            }
        }
        
    }
    void SpawnTireTrack()
    {
        Instantiate(tireTrackPrefab, transform.position, transform.rotation);
    }
}
