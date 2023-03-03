using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player 
{
    public class PlayerShoot : MonoBehaviour
    {
        //To shoot
        public List<Transform> turretBarrels;
        public GameObject bulletPrefab;
        public GameObject bomb_prefab;
        [SerializeField] private float reloadDelay = 1f;
        private bool canShoot = true;
        private bool canBomb = true;
        private Collider2D tankCollider;
        private float currentDelayShoot = 0;
        private float currentDelayBomb = 0;
        
        // For the bullet
        [SerializeField] private string tag = "Enemy";
        [SerializeField] private bool canBounce = false;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float max_distance = 2f;
        [SerializeField] private float damage = 1f;
        [SerializeField] private int life_time = 50;
        [SerializeField] private float bombDelay = 3f;
        // Start is called before the first frame update
        void Start()
        {
            tankCollider = gameObject.GetComponentInChildren<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            ShootManagement();
            BombManagement();
            
        }
        private void ShootManagement() // Shoot
        {
            if(!canShoot)
            {
                currentDelayShoot -= Time.deltaTime;
            }
            if(currentDelayShoot <= 0)
            {
                canShoot = true;
            }
            if(Input.GetMouseButtonDown(0)) // Shoot a bullet for each barrels
            {
                if(canShoot)
                {
                    canShoot = false;
                    currentDelayShoot = reloadDelay;
                    foreach(var barrel in turretBarrels) // Spawn a bullet for each barrel
                    {            
                        Quaternion bullet_rotation = Quaternion.Euler(0, 0, barrel.rotation.eulerAngles.z + 90);
                        GameObject bullet = Instantiate(bulletPrefab, barrel.position, bullet_rotation);
                        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), tankCollider);
                        bullet.GetComponent<Bullet>().Initialise(damage, speed, max_distance,life_time ,canBounce, tag);
                    }
                }
            }
        }
        private void BombManagement() // Bomb
        {
            if(!canBomb)
            {
                currentDelayBomb -= Time.deltaTime;
            }
            if(currentDelayBomb <= 0)
            {
                canBomb = true;
            }
            if(Input.GetKeyDown(KeyCode.Space)) // Spawn a bomb
            {
                if(canBomb)
                {
                    canBomb = false;
                    currentDelayBomb = bombDelay;
                    Instantiate(bomb_prefab, transform.position, Quaternion.identity);
                }
            }
        }
    }

}
