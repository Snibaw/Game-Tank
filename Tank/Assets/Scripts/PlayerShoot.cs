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
        [SerializeField] private float reloadDelay = 1f;
        private bool canShoot = true;
        private Collider2D tankCollider;
        private float currentDelay = 0;
        
        // For the bullet
        [SerializeField] private string tag = "Enemy";
        [SerializeField] private bool canBounce = false;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float max_distance = 2f;
        [SerializeField] private float damage = 1f;
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
            if(Input.GetMouseButtonDown(0))
            {
                if(canShoot)
                {
                    canShoot = false;
                    currentDelay = reloadDelay;
                    foreach(var barrel in turretBarrels)
                    {            
                        Quaternion bullet_rotation = Quaternion.Euler(0, 0, barrel.rotation.eulerAngles.z + 90);
                        GameObject bullet = Instantiate(bulletPrefab, barrel.position, bullet_rotation);
                        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), tankCollider);
                        bullet.GetComponent<Bullet>().Initialise(damage, speed, max_distance, canBounce, tag);
                    }
                }
            }
        }
    }

}
