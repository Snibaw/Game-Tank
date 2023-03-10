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
    public Tile[] obstacles;
    private Vector3Int tilePosition;
    private Vector3Int tileTreeBottomLeftPosition; // the position of the bottom left tile of the tree
    private Vector3Int tileObstacleBottomPosition; // the position of the bottom tile of the obstacle
    private Vector3Int tileObstacleTopPosition; // the position of the bottom tile of the tree
    private Animator bombAnimator;
    private bool bottomObstacleDestroyed = false;
    private bool topObstacleDestroyed = false;
    
    /**
     * This script is used to destroy the bomb and the tiles around it
     * It also damages the enemies and the player
     * It also destroys the obstacles
     */
    // Start is called before the first frame update
    void Start()
    {
        bombAnimator = GetComponent<Animator>();
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
        yield return new WaitForSeconds(explosionTime-0.5f);
        bombAnimator.SetTrigger("exploding");
        yield return new WaitForSeconds(0.3f);
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
            if(collider.tag == "Obstacle")
            {
                Destroy(collider.gameObject);
            }
        }
        bottomObstacleDestroyed = false;
        topObstacleDestroyed = false;
        foreach (var p in new BoundsInt(-1, -1, 0, 3, 3, 1).allPositionsWithin) // Destroy the bomb and the tiles around it
        {
            tilePosition = tileMapObstacles.WorldToCell(transform.position) + p;
            
            if(isTileTree(tileMapObstacles.GetTile(tilePosition), tileMapNoCollider.GetTile(tilePosition))) continue;
            //if(isTileObstacle(tileMapObstacles.GetTile(tilePosition), tileMapNoCollider.GetTile(tilePosition))) continue;
            if(isTileObstacle(tilePosition)) // Destroy the head of the obstacle
            {
                tileMapNoCollider.SetTile(tilePosition+new Vector3Int(0,1,0), null);
                // tileObstacleTopPosition = tilePosition+new Vector3Int(0,1,0);
                // topObstacleDestroyed = true;
            }
            // else if(tileMapObstacles.GetTile(tilePosition) == obstacles[0]) // We are destroying the bottom of the obstacle
            // {
            //     tileObstacleBottomPosition = tilePosition;
            //     bottomObstacleDestroyed = true;
            // }
            // else if(tileMapNoCollider.GetTile(tilePosition) == obstacles[obstacles.Length-1]) // We are destroying the top of the obstacle
            // {
            //     tileObstacleTopPosition = tilePosition;
            //     topObstacleDestroyed = true;
            // }
            tileMapObstacles.SetTile(tilePosition, null);
            tileMapNoCollider.SetTile(tilePosition, null);
            
        }
        // We recrete a smaller obstacle with pieces of the destroyed obstacle
        // if(bottomObstacleDestroyed)
        // {
        //     for(int i=0; i<6;i++)
        //     {
        //         if(tileMapObstacles.GetTile(tileObstacleBottomPosition) == null)
        //         {
        //             tileObstacleBottomPosition += new Vector3Int(0, 1, 0);
        //         }
        //         else
        //         {
        //             tileMapObstacles.SetTile(tileObstacleBottomPosition, obstacles[0]);
        //             break;
        //         }
        //     }
        // } 
        // if(topObstacleDestroyed)
        // {
        //     Debug.Log("Top Obstacle Destroyed");
        //     for(int i=0; i<6;i++)
        //     {
        //         if(tileMapObstacles.GetTile(tileObstacleTopPosition) == null)
        //         {
        //             tileObstacleTopPosition -= new Vector3Int(0, 1, 0);
        //         }
        //         else
        //         {
        //             Debug.Log("2");
        //             tileMapObstacles.SetTile(tileObstacleTopPosition, null);
        //             tileMapNoCollider.SetTile(tileObstacleTopPosition, obstacles[obstacles.Length-1]);
        //             break;
        //         }
        //     }
        // }  
        yield return new WaitForSeconds(0.3f);
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
    private bool isTileObstacle(Vector3Int tilePosition) // Find if the tile is part of the obstacle
    {
        Tile tile = tileMapObstacles.GetTile<Tile>(tilePosition);
        for(int i = 0; i < obstacles.Length; i++)
        {
            if(tile == obstacles[i])
            {
                return true;
            }
        }
        return false;
    }
    /*private bool isTileObstacle(TileBase tileObstacle,TileBase tileNoCollider) // Find if the tile is part of the obstacle
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            if (tileObstacle == obstacles[i] || tileNoCollider == obstacles[i])
            {
                Debug.Log("Tile Position : " + tilePosition);
                tileObstacleBottomPosition = FindBottomTilePosition(tilePosition, i); // We need to find the bottom of the obstacle
                DestroyObstacles(tileObstacleBottomPosition);
                return true;
                break;
            }
        }
        return false;
    }
    private Vector3Int FindBottomTilePosition(Vector3Int tilePosition, int i) // Find the bottom tile of the obstacle
    {
        if (i == 0) // This is the bottom tile
        {
            return tilePosition;   
        }
        else // Try to find the bottom tile
        {
            Tile tile = tileMapObstacles.GetTile<Tile>(tilePosition + new Vector3Int(0, -1, 0));
            while (tile != obstacles[0])
            {
                tilePosition += new Vector3Int(0, -1, 0);
                tile = tileMapObstacles.GetTile<Tile>(tilePosition + new Vector3Int(0, -1, 0));
            }
            return tilePosition;
        }
    }
    private void DestroyObstacles(Vector3Int tileBottomPosition) // Destroy the obstacle at the given position
    {
        Tile tile;
        tileMapObstacles.SetTile(tileBottomPosition + new Vector3Int(0,-1,0), null); // First tile = bottom of obstacle
        do
        {
            tileMapObstacles.SetTile(tileBottomPosition, null);
            tileBottomPosition += new Vector3Int(0, 1, 0);
            tile = tileMapNoCollider.GetTile<Tile>(tileBottomPosition);
        } while(tile != obstacles[obstacles.Length-1]);
        tileMapNoCollider.SetTile(tileBottomPosition, null); // Last tile = top of obstacle
    }*/
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
