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
    private List<Vector3Int> tileObstacleVerticalPosition; // the position of the tile appartening to a vertical obstacle
    private List<Vector3Int> tileObstacleHorizontalPosition; // the position of the tile appartening to a horizontal obstacle
    private Animator bombAnimator;
    public AudioClip bombExplosion;
    private float bombRadius = 2f;
    private int bombDamage = 1;
    
    /**
     * This script is used to destroy the bomb and the tiles around it
     * It also damages the enemies and the player
     * It also destroys the obstacles
     */
    // Start is called before the first frame update
    void Start()
    {
        tileObstacleHorizontalPosition = new List<Vector3Int>();
        tileObstacleVerticalPosition = new List<Vector3Int>();
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
        yield return new WaitForSeconds(0.2f);
        AudioManager.instance.PlayClipAt(bombExplosion, transform.position);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bombRadius);
        foreach(Collider2D collider in colliders)
        {
            if(collider.tag == avoidTag) // If the collider is an enemy and the bomb is an enemy bomb, don't damage the enemy
            {
                continue;
            }
            if(collider.tag == "Player" || collider.tag == "Enemy")
            {
                collider.GetComponentInParent<Health>().TakeDamage(bombDamage);
            }
            if(collider.tag == "Obstacle")
            {
                Destroy(collider.gameObject);
            }
        }
        foreach (var p in new BoundsInt(-1, -1, 0, 3, 3, 1).allPositionsWithin) // Destroy the bomb and the tiles around it
        {
            tilePosition = tileMapObstacles.WorldToCell(transform.position) + p;
            
            if(isTileTree(tileMapObstacles.GetTile(tilePosition), tileMapNoCollider.GetTile(tilePosition))) continue;
            
            // Important case: If it's a 2x1 obstacle, we need to destroy the 2 tiles
            if(tileMapObstacles.GetTile(tilePosition) == obstacles[0] && tileMapNoCollider.GetTile(tilePosition+new Vector3Int(0,1,0)) == obstacles[obstacles.Length-1])
            {
                tileMapNoCollider.SetTile(tilePosition + new Vector3Int(0,1,0), null);
            }           
            
            if(isTileObstacle(tilePosition)) // We destroy part of an obstacle
            {
                Debug.Log("Tile obstacle: " + tilePosition);
                // We search if it's a vertical or horizontal obstacle
                if(isTileObstacle(tilePosition+new Vector3Int(0,1,0)) || isTileObstacle(tilePosition+new Vector3Int(0,-1,0))) // Vertical
                {
                    tileObstacleVerticalPosition.Add(tilePosition);
                }
            }
            tileMapObstacles.SetTile(tilePosition, null);
            tileMapNoCollider.SetTile(tilePosition, null);
            
        }
        //We recrete a smaller obstacle with pieces of the destroyed obstacle
        RecreateVerticalObstacle();
        yield return new WaitForSeconds(0.4f);
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
    private void RecreateVerticalObstacle()
    {
        Vector3Int tileGoingUp;
        for(int j = 0; j < tileObstacleVerticalPosition.Count; j++) // For each tiles destroyed
        {
            tileGoingUp = tileObstacleVerticalPosition[j];
            for(int i=0; i<3;i++) // We search the top part of the obstacle
            {
                if(tileMapNoCollider.GetTile(tileGoingUp) == obstacles[obstacles.Length-1]) // We found a top tile
                {
                    tileMapObstacles.SetTile(tileGoingUp + new Vector3Int(0,-1,0), obstacles[0]);
                    break;
                }
                else if(tileMapObstacles.GetTile(tileGoingUp) == null) // If the tile is empty we continue to search
                {
                    tileGoingUp += new Vector3Int(0, 1, 0);
                }
                else // We found a tile that is not empty
                {
                    tileMapObstacles.SetTile(tileGoingUp, obstacles[0]); // If the tile is an obstacle we create the obstacle by placing a bottom tile
                    break;
                }
            }
            for(int i=0; i<3;i++) // We search the bottom part of the obstacle
            {
                if(tileMapObstacles.GetTile(tileObstacleVerticalPosition[j]) == null) // If the tile is empty we continue to search
                {
                    tileObstacleVerticalPosition[j] += new Vector3Int(0, -1, 0);
                }
                else // We found a tile that is not empty
                {
                    tileMapNoCollider.SetTile(tileObstacleVerticalPosition[j]+ new Vector3Int(0,1,0), obstacles[obstacles.Length-1]); // If the tile is an obstacle we create the obstacle by placing a bottom tile on top of it
                    break;
                }
            }
        }
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
    public void Initialise(float bombRadius, int bombDamage)
    {
        this.bombRadius = bombRadius;
        this.bombDamage = bombDamage;
    }
}
