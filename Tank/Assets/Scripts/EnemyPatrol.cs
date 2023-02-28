using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyPatrol : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float rotation_speed = 0.5f;
        [SerializeField] private float waitTimeBeforeMove = 3f;
        private GameObject Hull;
        public GameObject[] waypoints;
        private bool stopDelay;
        private bool waitBeforeMove = false;
        private EnemyTank enemyTank;
        private int index = 0;
        // Start is called before the first frame update
        void Start()
        {
            Hull = transform.GetChild(0).gameObject;
            enemyTank = gameObject.GetComponent<EnemyTank>();
        }

        // Update is called once per frame
        void Update()
        {
            Patrol();
            if(stopDelay)
            {
                FlipHullSprite();
            }
        }
        private void Patrol()
        {
            if(enemyTank.playerFound) waitBeforeMove = true;
            else if(waitBeforeMove) StartCoroutine(WaitBeforeMove());// Player just disappear
            
            if(stopDelay || waitBeforeMove) return;
            
            if(transform.position == waypoints[index].transform.position)
            {
                stopDelay= true;
                index = (index + 1) % waypoints.Length;
            }
            else transform.position = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, speed * Time.deltaTime);
        }
        private void FlipHullSprite() // Rotate the sprite according to the direction of the movement
        {
            Vector2 direction = waypoints[index].transform.position - transform.position;
            float rot_y = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
            var rotation_quaternion = Quaternion.Euler(0, 0, Mathf.Round(rot_y-90)); // Convert the angle to quaternion
            Hull.transform.rotation = Quaternion.Lerp(Hull.transform.rotation, rotation_quaternion, Time.deltaTime * rotation_speed); // Smoothly rotate the sprite
            if(Mathf.Round(Hull.transform.rotation.eulerAngles.z*10) == Mathf.Round(rotation_quaternion.eulerAngles.z*10))
            {
                stopDelay = false;
            }
        }
        private IEnumerator WaitBeforeMove()
        {
            yield return new WaitForSeconds(3f);
            waitBeforeMove = false;
        }
    }
}

