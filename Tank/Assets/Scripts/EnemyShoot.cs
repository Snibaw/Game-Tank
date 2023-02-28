using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyShoot : MonoBehaviour
    {
        //To shoot
        public Transform[] barrels;
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
        //Type of enemy
        [SerializeField] private float timeBetweenDobleShots = 0.2f;
        private float tripleShotSpread = 0f;
        [SerializeField] private bool isDobleShot = false;
        
        [SerializeField] private bool isTripleShot = false;
        [SerializeField] private bool isLaser = false;
        
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
                if(isDobleShot)
                {
                    StartCoroutine(DobleShot());
                }
                if(isTripleShot)
                {
                    StartCoroutine(TripleShot());
                }
                else
                {
                    InstantiateBullet(isLaser);  
                } 
            }
        }
        private void InstantiateBullet(bool isLaser = false)
        {
            foreach (Transform single_barrel in barrels)
            {
                Quaternion bullet_rotation = Quaternion.Euler(0, 0, single_barrel.rotation.eulerAngles.z + 90+ tripleShotSpread);
                GameObject bullet = Instantiate(bulletPrefab, single_barrel.position, bullet_rotation);
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), tankCollider);
                if(isLaser)
                {
                    bullet.GetComponent<Laser>().Initialise(bullet_damage,life_time, tag);
                }
                else
                {
                    bullet.GetComponent<Bullet>().Initialise(bullet_damage, bullet_speed, max_distance,life_time, canBounce, tag);
                }
            }
        }
        IEnumerator DobleShot()
        {
            InstantiateBullet(isLaser);
            yield return new WaitForSeconds(timeBetweenDobleShots);
            InstantiateBullet(isLaser);
        }
        private IEnumerator TripleShot()
        {
            tripleShotSpread = 0f;
            InstantiateBullet();
            yield return new WaitForSeconds(0.05f);
            tripleShotSpread = 45f;
            InstantiateBullet();
            tripleShotSpread = -45f;
            InstantiateBullet();
        }
        
    }
}
