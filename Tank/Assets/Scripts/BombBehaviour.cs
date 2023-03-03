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
    public Tile[] Tree;
    public Tile headOfObstacle;
    private Vector3Int tilePosition;
    private Vector3Int tileTreeBottomLeftPosition; // the position of the bottom left tile of the tree
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
            tilePosition = tileMapObstacles.WorldToCell(transform.position) + p;
            
            if(isTileTree(tileMapObstacles.GetTile(tilePosition), tileMapNoCollider.GetTile(tilePosition))) continue;
            
            tileMapObstacles.SetTile(tilePosition, null);
            tileMapNoCollider.SetTile(tilePosition, null);
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
    private bool isTileTree(TileBase tileObstacle,TileBase tileNoCollider) // Find if the tile is part of the tree
    {
        for (int i = 0; i < Tree.Length; i++)
        {
            if (tileObstacle == Tree[i] || tileNoCollider == Tree[i])
            {
                tileTreeBottomLeftPosition = tilePosition - new Vector3Int(-i%3, -i/3, 0);
                DestroyTree(tileTreeBottomLeftPosition);
                return true;
                break;
            }
        }
        return false;
    }
    private void DestroyTree(Vector3Int tilePosition) // Destroy the tree at the given position
    {
        Debug.Log("Tile Position : " + tilePosition);
        for (int height = 0; height <3; height++)
        {
            for (int width = 0; width < 2; width++) 
            {
                if(height == 0) // Destroy the base of the tree
                {
                    tileMapObstacles.SetTile(tilePosition + new Vector3Int(width, 0, 0), null);
                }
                else // Destroy the leaves of the tree
                {
                    tileMapNoCollider.SetTile(tilePosition + new Vector3Int(width, height, 0), null);
                }
            }
        }
    }
}
