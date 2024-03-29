using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyTank : MonoBehaviour
    {
        private EnemyShoot enemyShoot;
        private bool playerInView = false;
        private GameObject player;
        public GameObject canon;
        public bool playerFound = false;
        private bool lastPlayerFound = false;
        [SerializeField] private float timeToRotate = 2f;
        [SerializeField] private float maxrange = 15f;
        [SerializeField] private float rotation_speed = 5f;
        

        // Start is called before the first frame update
        void Start()
        {
            enemyShoot = GetComponent<EnemyShoot>();
            player = GameObject.FindGameObjectsWithTag("Player")[0];

        }

        // Update is called once per frame
        void Update()
        {
            DetectPlayer();
        }
        private void DetectPlayer()
        {

            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position); // Raycast from the tank to the player
            // If distance < maxrange
            if(Vector2.Distance(transform.position, player.transform.position) < maxrange)
            {
                if(hit.collider.tag == "Player")
                {
                    playerFound = true;
                    if(playerFound != lastPlayerFound)
                    {
                        RotateCanon(player.transform.position);
                        StartCoroutine(WaitBeforeShoot());
                    }
                    else
                    {
                        Shoot(player.transform.position);
                    }
                }
                else
                {
                    playerFound = false;
                    lastPlayerFound = false;
                }
            }
            
        }
        public void Shoot(Vector3 playerPosition)
        {
            RotateCanon(playerPosition); // Rotate the canon to the player
            enemyShoot.ShootThePlayer(); // Shoot the player
        }
        public void RotateCanon(Vector3 playerPosition) // Rotate the canon to the player with smooth rotation
        {
            Vector3 direction = playerPosition - canon.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //Smooth the rotation
            var rotation_quaternion = Quaternion.Euler(0,0,angle-90);
            canon.transform.rotation = Quaternion.Lerp(canon.transform.rotation, rotation_quaternion, Time.deltaTime * rotation_speed);
        }
        public IEnumerator WaitBeforeShoot()
        {
            yield return new WaitForSeconds(timeToRotate);
            lastPlayerFound = true;
            enemyShoot.ShootThePlayer();
        }
    }
}
