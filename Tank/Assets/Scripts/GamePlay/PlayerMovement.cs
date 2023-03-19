using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private float horizontal;
        private float vertical;
        [SerializeField] private float speed = 2.5f;
        [SerializeField] private float rotation_speed = 10f;
        public GameObject Canon;
        public GameObject Hull;
        
        private LevelManager levelManager;
        
        
        // Start is called before the first frame update
        void Start()
        {
            levelManager = GameObject.FindGameObjectsWithTag("LevelManager")[0].GetComponent<LevelManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if(levelManager.isPaused)
            {
                return;
            }
            //Movement with ZQSD
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            FlipHullSprite();
            
            //Rotate canon with mouse
            RotateCanon();
            
        }
        private void FixedUpdate() 
        {
            if(horizontal != 0 && vertical != 0)
            {
                horizontal /= 1.414f;
                vertical /= 1.414f;
            }
            // Transform input into movement
            transform.Translate(Vector2.right * horizontal * speed * Time.deltaTime);
            transform.Translate(Vector2.up * vertical * speed * Time.deltaTime);
        }
        private void FlipHullSprite() // Rotate the sprite according to the direction of the movement
        {
            Vector2 direction = new Vector2(horizontal, vertical).normalized;
            if(direction.x != 0 || direction.y != 0)
            {
                float rot_y = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
                var rotation_quaternion = Quaternion.Euler(0, 0, rot_y-90); // Convert the angle to quaternion
                Hull.transform.rotation = Quaternion.Lerp(Hull.transform.rotation, rotation_quaternion, Time.deltaTime * rotation_speed); // Smoothly rotate the sprite
            }
        }
        private void RotateCanon()
        {
            Vector3 mouse_pos = Input.mousePosition; // Get the mouse position
            mouse_pos.z = -20;
            Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position); // Get the position of the player
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
            Canon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90)); // Rotate the canon (no smoothing)
        }
    }

}
