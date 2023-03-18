using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player 
{
    public class PlayerShoot : MonoBehaviour
    {
        //To shoot
        public Transform barrel;
        public List<Transform> dobleBarrel;
        public GameObject bulletPrefab;
        public GameObject bomb_prefab;
        [SerializeField] private float reloadDelay = 1f;
        private bool canShoot = true;
        private bool canBomb = true;

        private Collider2D tankCollider;
        private float currentDelayShoot = 0;
        private float currentDelayBomb = 0;
        private PlayerStats playerStats;
        
        // For the bullet
        [SerializeField] private string tag = "Enemy";
        [SerializeField] private bool canBounce = false;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float max_distance = 2f;
        [SerializeField] private float damage = 1f;
        [SerializeField] private int life_time = 50;
        [SerializeField] private float bombDelay = 3f;
        // For Upgrade
        private int maxBounces =  0;
        private bool isDobleShot = false;
        private bool isDobleBarrel = false;
        private bool isTripleShot = false;
        private float tripleShotSpread = 0f;
        private List<float> tripleShotSpreadList = new List<float> {0f, 45f, -45f};
        private float tripleShotTiming = 0.1f;
        private float bombRadius = 2f;
        private int bombDamage = 1;
        
        private LevelManager levelManager;
        // Start is called before the first frame update
        void Start()
        {
            tankCollider = gameObject.GetComponentInChildren<Collider2D>();
            levelManager = GameObject.FindGameObjectsWithTag("LevelManager")[0].GetComponent<LevelManager>();
            
            playerStats = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>();
            InitUpgrade();
        }

        // Update is called once per frame
        void Update()
        {
            if(levelManager.isPaused)
            {
                return;
            }
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
                    if(isTripleShot)
                    {
                        StartCoroutine(TripleShot());
                    }
                    else if(isDobleShot)
                    {
                        StartCoroutine(DobleShot());
                    }    
                    else
                    {
                        ShootABullet();
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
                    GameObject bomb = Instantiate(bomb_prefab, transform.position, Quaternion.identity);
                    bomb.GetComponent<BombBehaviour>().Initialise(bombRadius, bombDamage);
                }
            }
        }
        private void ShootABullet()
        {
            if(!isDobleBarrel || isTripleShot)
            {
                Quaternion bullet_rotation = Quaternion.Euler(0, 0, barrel.rotation.eulerAngles.z + 90 + tripleShotSpread);
                GameObject bullet = Instantiate(bulletPrefab, barrel.position, bullet_rotation);
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), tankCollider);
                bullet.GetComponent<Bullet>().Initialise(damage, speed, max_distance,life_time ,canBounce, tag, maxBounces);
            }
            else
            {
                for(int i =0 ; i < 2; i++)
                {
                    Quaternion bullet_rotation = Quaternion.Euler(0, 0, barrel.rotation.eulerAngles.z + 90 + tripleShotSpread);
                    GameObject bullet = Instantiate(bulletPrefab, dobleBarrel[i].position, bullet_rotation);
                    Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), tankCollider);
                    bullet.GetComponent<Bullet>().Initialise(damage, speed, max_distance,life_time ,canBounce, tag, maxBounces);
                }
                
            }
        }
        private IEnumerator DobleShot()
        {
            ShootABullet();
            yield return new WaitForSeconds(0.05f);
            ShootABullet();
        }
        
        private IEnumerator TripleShot()
        {
            foreach(float tripleShotSpreadTempo in tripleShotSpreadList)
            {
                tripleShotSpread  = tripleShotSpreadTempo;
                if(isDobleShot)
                {
                    StartCoroutine(DobleShot());
                }
                else
                {
                    ShootABullet();
                }
                yield return new WaitForSeconds(tripleShotTiming);
                tripleShotTiming = 0.05f;
            }
            tripleShotTiming = 0.1f;
            tripleShotSpread = 0f;
        }
        private void InitUpgrade()
        {
            playerStats.LoadPlayer();
            if(playerStats.Upgrades[0] == 1)
            {
                speed = speed * 1.5f;
            }
            if(playerStats.Upgrades[1] >= 1)
            {
                canBounce = true;
                maxBounces = playerStats.Upgrades[1];
            }
            if(playerStats.Upgrades[2] == 1)
            {
                isDobleShot = true;
            }
            if(playerStats.Upgrades[3] == 1)
            {
                isDobleBarrel = true;
            }
            if(playerStats.Upgrades[4] == 1)
            {
                isTripleShot = true;
            }
            if(playerStats.Upgrades[5] == 1)
            {
                reloadDelay = reloadDelay * 0.5f;
            }
            if(playerStats.Upgrades[6] == 1)
            {
                // Make bullet explode
            }
            if(playerStats.Upgrades[7] == 1)
            {
                damage = damage * 2f;
            }
            if(playerStats.Upgrades[8] == 1)
            {
                bombRadius = 3f;
            }
            if(playerStats.Upgrades[9] == 1)
            {
                bombDelay = bombDelay * 0.5f;
            }
            if(playerStats.Upgrades[10] == 1)
            {
                bombDamage = 2;
            }
        }
    }

}
