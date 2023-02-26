using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyTank : MonoBehaviour
    {
        private EnemyShoot enemyShoot;
        private bool playerInView = false;
        public GameObject player;
        public GameObject canon;
        [SerializeField] private float timeToRotate = 2f;
        [SerializeField] private float maxrange = 15f;
        [SerializeField] private float rotation_speed = 5f;
        // Start is called before the first frame update
        void Start()
        {
            enemyShoot = GetComponent<EnemyShoot>();
        }

        // Update is called once per frame
        void Update()
        {
            DetectPlayer();
        }
        private void DetectPlayer()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, maxrange); // Raycast from the tank to the player
            if(hit.collider.tag == "Player")
            {
                Shoot(hit.collider.gameObject);
            }
            else
            {
                playerInView = false;
            }
        }
        private void Shoot(GameObject player)
        {
            RotateCanon(player); // Rotate the canon to the player
            StartCoroutine(WaitBeforeShoot()); // Shoot the player
        }
        private void RotateCanon(GameObject player) // Rotate the canon to the player with smooth rotation
        {
            Vector3 direction = player.transform.position - canon.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //Smooth the rotation
            var rotation_quaternion = Quaternion.Euler(0,0,angle-90);
            canon.transform.rotation = Quaternion.Lerp(canon.transform.rotation, rotation_quaternion, Time.deltaTime * rotation_speed);
        }
        IEnumerator WaitBeforeShoot()
        {
            if(!playerInView)
            {
                yield return new WaitForSeconds(timeToRotate);
                playerInView = true;
            }
            enemyShoot.ShootThePlayer();
        }
    }
}

