using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    
    private string avoidTag = "Undefined";
    private float explosionTime = 3f;
    private Tilemap tileMapObstacles;
    private Tilemap tileMapNoCollider;
    // Start is called before the first frame update
    void Start()
    {
        tileMapObstacles = GameObject.Find("Obstacles").GetComponent<Tilemap>();
        tileMapNoCollider = GameObject.Find("ObstaclesNoCollider").GetComponent<Tilemap>();
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Explode() // Explode after 3 seconds
    {
        yield return new WaitForSeconds(explosionTime);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach(Collider2D collider in colliders)
        {
            if(collider.tag == avoidTag) // If the collider is an enemy and the bomb is an enemy bomb, don't damage the enemy
            {
                continue;
            }
            if(collider.tag == "Player" || collider.tag == "Enemy")
            {
                collider.GetComponentInParent<Health>().TakeDamage(1f);
            }
        }
        foreach (var p in new BoundsInt(-1, -1, 0, 3, 3, 1).allPositionsWithin) // Destroy the bomb and the tiles around it
        {
            tileMapObstacles.SetTile(tileMapObstacles.WorldToCell(transform.position) + p, null);
            tileMapNoCollider.SetTile(tileMapNoCollider.WorldToCell(transform.position) + p, null);
        }
        Destroy(gameObject);
    }
    public void SetAvoidTag(string tag)
    {
        this.avoidTag = tag;
    }
    public float GetExplosionTime()
    {
        return explosionTime;
    }
}
