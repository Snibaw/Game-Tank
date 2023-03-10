using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyBomber : MonoBehaviour
    {
        private NavMeshAgent agent;
        private bool playerSeenOnce = false;
        private bool playerInView = false;
        private GameObject player;
        private GameObject Hull;
        private GameObject canon;
        public bool playerFound = false;
        [SerializeField] private float maxrange = 15f;
        [SerializeField] private float rotation_speed = 5f;
        private Vector3 lastPlayerPosition;
        private Rigidbody2D rb;
        private Vector3 lastPosition;
        public bool avoidingExplosion = false;
        private Vector3 bombPosition;
        public GameObject bomb_prefab;
        private float bombSafePosition = 0f;
        private float explosionTime = 0f;
        

        // Start is called before the first frame update
        void Start()
        {
            //Nav Mesh Agent for following player
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            // Other initialisation
            player = GameObject.FindGameObjectsWithTag("Player")[0];
            rb = GetComponent<Rigidbody2D>();
            canon = transform.GetChild(1).gameObject;
            Hull = transform.GetChild(0).gameObject;
            lastPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            SetDestination();
            RotateCanon();
            FlipHullSprite();
            DetectPlayer();
            AttackPlayer();
        }
        private void DetectPlayer()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, maxrange); // Raycast from the tank to the player
            if(hit.collider.tag == "Player") // Once we found the player, WE FOLLOW HIM UNTIL DEATH
            {
                playerFound = true;
                playerSeenOnce = true;
                lastPlayerPosition = player.transform.position;
            }
            else
            {
                playerFound = false;
            }
        }
        private void SetDestination()
        {
            if(avoidingExplosion)
            {// If the tank is avoiding an explosion, it will go to the bomb position + a vector avoiding the player to avoid collision with him
                agent.SetDestination(bombPosition + (transform.position - player.transform.position).normalized * 5 *bombSafePosition); // 5 = experimental value
            }
            else if(playerSeenOnce)
            {
                agent.SetDestination(player.transform.position);
            }
            else
            {
                agent.SetDestination(transform.position);
            }
        }
        private void RotateCanon() // Rotate the canon to the player with smooth rotation
        {
            Vector3 direction = lastPlayerPosition - canon.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //Smooth the rotation
            var rotation_quaternion = Quaternion.Euler(0,0,angle-90);
            canon.transform.rotation = Quaternion.Lerp(canon.transform.rotation, rotation_quaternion, Time.deltaTime * rotation_speed);
        }
        private void FlipHullSprite() // Rotate the sprite according to the direction of the movement
        {
            Vector2 direction = lastPosition - transform.position;
            lastPosition = transform.position;
            float rot_y = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
            var rotation_quaternion = Quaternion.Euler(0, 0, Mathf.Round(rot_y-90)); // Convert the angle to quaternion
            Hull.transform.rotation = Quaternion.Lerp(Hull.transform.rotation, rotation_quaternion, Time.deltaTime * rotation_speed); // Smoothly rotate the sprite
        }
        private void AttackPlayer()
        {
            if(avoidingExplosion)
            {
                return;
            }
            if(Vector3.Distance(transform.position, player.transform.position) <= 1.8f) // If the player is close enough, we spawn a bomb
            {
                // Spawn a bomb
                GameObject bomb = Instantiate(bomb_prefab, transform.position, Quaternion.identity);
                BombBehaviour bombBehaviour = bomb.GetComponent<BombBehaviour>();
                explosionTime = bombBehaviour.GetExplosionTime();
                bombSafePosition = 2; // Experimental value
                bombBehaviour.SetAvoidTag("Enemy");
                // Avoid the explosion of the bomb
                StartCoroutine(AvoidExplosion());
                bombPosition = transform.position;
            }
        }
        private IEnumerator AvoidExplosion()
        {
            avoidingExplosion = true;
            yield return new WaitForSeconds(explosionTime);
            avoidingExplosion = false;
        }
    }
}

