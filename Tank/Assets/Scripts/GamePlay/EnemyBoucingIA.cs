using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace Enemy
{
    public class EnemyBoucingIA : MonoBehaviour
    {
        public int maxBounces = 2;
        public float maxDistance = 100f;
        private Vector2 original_direction;
        private Vector2 direction;
        [SerializeField] private float step = 0.05f;
        [SerializeField] private float maxAngle = 0.95f;
        private bool goingUp = true;
        private EnemyTank enemyTank;
        private bool last_playerFound = false; // Know if the player just has been found or not
        
        private RaycastHit2D last_hit;

        // Here we will store all the points that we want the ray to bounce off
        private List<Vector2> pointsOfReflection = new List<Vector2>();
        private GameObject player;
        private void Start() 
        {
            // The direction the ray will be cast at
            // Initially it's the direction from the enemy to the player
            player = GameObject.FindGameObjectsWithTag("Player")[0];
            original_direction = (player.transform.position - transform.position).normalized;
            direction = original_direction;
            
            enemyTank = GetComponent<EnemyTank>();
        }
        private void Update()
        {
            TravelPossibleReflexions();
            if (pointsOfReflection.Count > 0)
            {
                // Draw lines between points of reflection
                for (int i = 0; i < pointsOfReflection.Count - 1; i++)
                {
                    Debug.DrawLine(pointsOfReflection[i], pointsOfReflection[i + 1], Color.blue);
                }
            }
        }
        private bool DetectIfPlayerInDirection(Vector2 direction)
        {
                pointsOfReflection.Clear();
                // Add the enemy position as the first point for the ray
                pointsOfReflection.Add(transform.position);
                int bounceCount = 0;

                
                // Calculate all the bounces
                while (bounceCount < maxBounces)
                {
                    // Cast the ray towards that direction we want
                    // Move the source a tiny bit (0.1f) so it doesn't start from inside the collider, cause the could lead to problems
                    RaycastHit2D hit = Physics2D.Raycast(pointsOfReflection[pointsOfReflection.Count - 1] + direction * 0.01f, direction, maxDistance);
                    // If we hit anything
                    if (hit.collider != null)
                    {
                        if(bounceCount==0) // If it's the first bounce, we store the hit
                        {
                            last_hit = hit;
                        }
                        pointsOfReflection.Add(hit.point);
                        // Find the direction the reflection should be, based on the collider we hit
                        Vector2 reflection = Vector2.Reflect(direction, hit.normal);                    
                        direction = reflection.normalized;
                        bounceCount++;
                        if(hit.collider.gameObject.tag=="Player")
                        {
                            return true;
                        }
                    }
                    else
                        // If the raycast didn't hit anything, break out of the loop
                        break;
                }

            // Draw lines between all the points we found earlier
            if (pointsOfReflection.Count > 0)
            {
                // Draw lines between points of reflection
                for (int i = 0; i < pointsOfReflection.Count - 1; i++)
                {
                    Debug.DrawLine(pointsOfReflection[i], pointsOfReflection[i + 1], Color.blue);
                }
            }
            return false;
        }
        private void TravelPossibleReflexions()
        {
            if(!enemyTank.playerFound) // If the player is found, no need to check for reflexions
            {
                if(last_playerFound) // The player was in sight but he ran behind a wall
                {
                    // We need to reactualise the direction of the player
                    original_direction = (player.transform.position - transform.position).normalized;
                    direction = original_direction;
                    last_playerFound = false;
                }
                if(goingUp) // First phase : we go from the original direction to y = maxAngle x = 0
                {
                    if(!DetectIfPlayerInDirection(direction) && direction.y < maxAngle) // If we don't detect the player and we are not at the max angle
                    {
                        direction = new Vector2(direction.x,direction.y+step).normalized; // We go up
                    }
                    else if(DetectIfPlayerInDirection(direction)) // We found the player !
                    {
                        enemyTank.RotateCanon(last_hit.point);
                        StartCoroutine(enemyTank.WaitBeforeShoot());
                    }
                    else // We reach max angle, time to go down
                    {
                        goingUp = false;
                        direction = original_direction; // We start from the player position
                    }
                }
                if(!goingUp)
                {
                    if(!DetectIfPlayerInDirection(direction) && direction.y > -maxAngle)
                    {
                        direction = new Vector2(direction.x,direction.y-step).normalized;
                    }
                    else if(DetectIfPlayerInDirection(direction)) // We found the player !
                    {
                        enemyTank.RotateCanon(last_hit.point);
                        StartCoroutine(enemyTank.WaitBeforeShoot());
                    }
                    else // We reach max angle, the end of both phases
                    {
                        goingUp = true;
                        Debug.Log(gameObject.name + " : Player not found through reflexions");
                    }
                }
            }
            else
            {
                last_playerFound = true;
            }
        }
    }
}
