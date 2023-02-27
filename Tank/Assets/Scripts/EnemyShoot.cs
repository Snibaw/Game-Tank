using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyShoot : MonoBehaviour
    {
        //To shoot
        
        public Transform barrel;
        public GameObject bulletPrefab;
        [SerializeField] private float reloadDelay = 3f;
        private bool canShoot = true;
        private Collider2D tankCollider;
        private float currentDelay = 0;
        
        // For the bullet
        [SerializeField] private string tag = "Player";
        [SerializeField] private bool canBounce = false;
        [SerializeField] private float bullet_speed = 5f;
        [SerializeField] private float max_distance = 2f;
        [SerializeField] private float bullet_damage = 1f;
        [SerializeField] private int life_time = 50;
        
        // Start is called before the first frame update
        void Start()
        {
            tankCollider = gameObject.GetComponentInChildren<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if(!canShoot)
            {
                currentDelay -= Time.deltaTime;
            }
            if(currentDelay <= 0)
            {
                canShoot = true;
            }
        }
        public void ShootThePlayer()
        {
            if(canShoot)
            {
                canShoot = false;
                currentDelay = reloadDelay;          
                Quaternion bullet_rotation = Quaternion.Euler(0, 0, barrel.rotation.eulerAngles.z + 90);
                GameObject bullet = Instantiate(bulletPrefab, barrel.position, bullet_rotation);
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), tankCollider);
                bullet.GetComponent<Bullet>().Initialise(bullet_damage, bullet_speed, max_distance,life_time, canBounce, tag);
            }
        }
        
    }
}
