using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private float horizontal;
        private float vertical;
        [SerializeField] private float speed = 2.5f;
        [SerializeField] private float rotation_speed = 10f;
        public GameObject Canon;
        private GameObject Barrel;
        private SpriteRenderer canonSprite;
        public GameObject Hull;
        
        private LevelManager levelManager;

        private FixedJoystick moveJoystick;
        private FixedJoystick aimJoystick;
        private Image aimJoystickImage;
        private Vector2 moveVelocity;
        private Vector2 aimVelocity;
        private Rigidbody2D rb;
        private PlayerShoot playerShoot;
        private LineRenderer lineRenderer;
        private Vector2 aimDir;
        public bool autoAim = false;
        public bool aimAssist = true;
        private bool followAimAssist = false;
        public float aimAssistStrength = 0.5f;
        private Vector2 directionAimAsssit = Vector2.zero;
        
        // Start is called before the first frame update
        void Start()
        {
            canonSprite = Canon.GetComponent<SpriteRenderer>();
            levelManager = GameObject.FindGameObjectsWithTag("LevelManager")[0].GetComponent<LevelManager>();
            moveJoystick = GameObject.Find("MoveJoystick").GetComponent<FixedJoystick>();
            aimJoystick = GameObject.Find("AimJoystick").GetComponent<FixedJoystick>();
            aimJoystickImage = aimJoystick.transform.GetChild(0).GetComponent<Image>();
            rb = GetComponent<Rigidbody2D>();
            playerShoot = GetComponent<PlayerShoot>();
            lineRenderer = GetComponent<LineRenderer>();
            Barrel = Canon.transform.GetChild(0).gameObject;

            autoAim = PlayerPrefs.GetInt("AutoAim", 0) == 1;
            aimAssist = PlayerPrefs.GetInt("AimAssist", 1) == 1;
            aimAssistStrength = PlayerPrefs.GetFloat("AimAssistStrength", 0.5f);
        }

        // Update is called once per frame
        void Update()
        {
            if(levelManager.isPaused)
            {
                return;
            }
            //Movement with ZQSD
            // horizontal = Input.GetAxis("Horizontal");
            // vertical = Input.GetAxis("Vertical");

            

            //Aim assist if no joystick input
            if(aimJoystick.Horizontal == 0 && aimJoystick.Vertical == 0 && autoAim) AutoShoot();
            if(aimAssist) AimAssist();
            //Rotate canon with mouse
            RotateCanon();
            CreateVisibleLine();
            ShootIfJoystickMoved();

            
        }
        private void CreateVisibleLine()
        {
            //Make a visible line in game going in front of the canon
            Vector2 rayDirection;
            if(aimJoystick.Horizontal!=0 || aimJoystick.Vertical!=0 || !autoAim)
            {
                if(followAimAssist) rayDirection = directionAimAsssit;
                else rayDirection = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);
            }
            else
            {
                //The direction is the front of the barrel
                rayDirection = new Vector2(Barrel.transform.position.x, Barrel.transform.position.y) - new Vector2(Canon.transform.position.x, Canon.transform.position.y);
                
            }
             //Create a Raycast, if it hits something, draw a line to it
            RaycastHit2D hit = Physics2D.Raycast(Barrel.transform.position, rayDirection, playerShoot.max_distance);

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, new Vector3(Barrel.transform.position.x, Barrel.transform.position.y, -1));
            if(hit.collider != null)
            {
                lineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, -1));
                //Change color to red
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    lineRenderer.startColor = Color.red;
                    lineRenderer.endColor = Color.red;
                    if(aimJoystick.Horizontal == 0 && aimJoystick.Vertical == 0 && autoAim)
                    {
                        playerShoot.TryToShoot();
                    }
                }
                else
                {
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                }
            }
            else
            {
                lineRenderer.SetPosition(1, new Vector3(Barrel.transform.position.x, Barrel.transform.position.y, -1) + new Vector3(aimVelocity.x, aimVelocity.y, 0).normalized * playerShoot.max_distance);
                //Change color to white
                lineRenderer.startColor = Color.black;
                lineRenderer.endColor = Color.black;
            }


        }
        private void ShootIfJoystickMoved()
        {
            if(Mathf.Pow(aimJoystick.Horizontal,2)+Mathf.Pow(aimJoystick.Vertical,2) >= 0.8f)
            {
                // Make the canon this color : #FF9E9E
                canonSprite.color =  new Color(1f, 0.6196079f, 0.6196079f, 1f);
                aimJoystickImage.color = Color.red;
                playerShoot.TryToShoot();
            }
            else
            {
                canonSprite.color = Color.white;
                aimJoystickImage.color = Color.white;
            }
        }
        private void JoystickMovement()
        {
            moveVelocity = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical).normalized * speed;
            Vector2 moveInput = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);
            Vector2 moveDir = moveInput.normalized*speed;
            rb.velocity = new Vector2(moveDir.x, moveDir.y);
        }
        private void FixedUpdate() 
        {
            JoystickMovement();
            FlipHullSprite();
            // if(horizontal != 0 && vertical != 0)
            // {
            //     horizontal /= (float)System.Math.Sqrt(System.Math.Pow(horizontal, 2) + System.Math.Pow(vertical, 2));
            //     vertical /= (float)System.Math.Sqrt(System.Math.Pow(horizontal, 2) + System.Math.Pow(vertical, 2));
            // }
            // Transform input into movement
            // transform.Translate(Vector2.right * horizontal * speed * Time.deltaTime);
            // transform.Translate(Vector2.up * vertical * speed * Time.deltaTime);
        }
        private void FlipHullSprite() // Rotate the sprite according to the direction of the movement
        {
            Vector2 direction = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical).normalized;
            if(direction.x != 0 || direction.y != 0)
            {
                float rot_y = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
                var rotation_quaternion = Quaternion.Euler(0, 0, rot_y-90); // Convert the angle to quaternion
                Hull.transform.rotation = Quaternion.Lerp(Hull.transform.rotation, rotation_quaternion, Time.deltaTime * rotation_speed); // Smoothly rotate the sprite
            }
        }
        private void RotateCanon()
        {
            // Vector3 mouse_pos = Input.mousePosition; // Get the mouse position
            // mouse_pos.z = -20;
            // Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position); // Get the position of the player
            // mouse_pos.x = mouse_pos.x - object_pos.x;
            // mouse_pos.y = mouse_pos.y - object_pos.y;
            // float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
            // Canon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90)); // Rotate the canon (no smoothing)

            aimVelocity = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);
            Vector2 aimInput = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);
            Vector2 aimDir = aimInput.normalized;
            if(aimDir.x != 0 || aimDir.y != 0)
            {
                float rot_y;
                if(followAimAssist) rot_y = Mathf.Atan2(directionAimAsssit.y, directionAimAsssit.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
                else rot_y = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
                var rotation_quaternion = Quaternion.Euler(0, 0, rot_y-90); // Convert the angle to quaternion
                Canon.transform.rotation = Quaternion.Lerp(Canon.transform.rotation, rotation_quaternion, Time.deltaTime * rotation_speed); // Smoothly rotate the sprite
            }   
        }
        private void AutoShoot()
        {
            //Make the canon aim at the closest enemy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float min_distance = Mathf.Infinity;
            GameObject closest_enemy = null;
            foreach(GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(enemy.transform.position, Canon.transform.position);
                if(distance < min_distance)
                {
                    min_distance = distance;
                    closest_enemy = enemy;
                }
            }
            if(closest_enemy != null)
            {
                Vector2 direction = closest_enemy.transform.position - Canon.transform.position;
                float rot_y = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
                var rotation_quaternion = Quaternion.Euler(0, 0, rot_y-90); // Convert the angle to quaternion
                Canon.transform.rotation = Quaternion.Lerp(Canon.transform.rotation, rotation_quaternion, Time.deltaTime * rotation_speed); // Smoothly rotate the sprite
            }
        }
        private void AimAssist()
        {
            // If the canon is close to the ennemy direction, make it aim at the enemy
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            bool enemy_in_range = false;
            foreach(GameObject enemy in enemies)
            {
                // Get the direction between where the player is aiming with the joystick and the enemy
                Vector2 direction = enemy.transform.position - Canon.transform.position+new Vector3(aimVelocity.x, aimVelocity.y, 0).normalized;
                float rot_y = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
                var rotation_quaternion = Quaternion.Euler(0, 0, rot_y-90); // Convert the angle to quaternion
                // Create a raycast and check if it hits an enemy
                

                if(Quaternion.Angle(Canon.transform.rotation, rotation_quaternion) < aimAssistStrength)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Canon.transform.position, direction, playerShoot.max_distance);
                    if(hit.collider != null && hit.collider.gameObject.tag == "Enemy")
                    {
                        enemy_in_range = true;
                        followAimAssist = true;
                        directionAimAsssit = enemy.transform.position - Canon.transform.position;
                    }
                        
                }
            }
            if(!enemy_in_range)
                followAimAssist = false;
        }
    }

}
