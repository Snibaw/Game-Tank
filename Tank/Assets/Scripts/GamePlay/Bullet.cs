using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private string tag = "Player";
    private bool canBounce = false;
    private float speed = 10f;
    private float max_distance = 2f;
    private float damage = 1f;
    private float conquared_distance = 0f;
    private int maxBounces = 0;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Animator animator;
    // Boucing
    Vector2 lastVelocity;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        startPosition = transform.position;
        transform.Rotate(0, 0, 90);
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
        conquared_distance = Vector2.Distance(transform.position, startPosition);
        if(conquared_distance >= max_distance)
        {
            StartCoroutine(FadeAwayBeforeDestroy());
        }
    }
    private void OnCollisionEnter2D(Collision2D other) { // If the bullet hits an enemy, destroy the bullet and damage the enemy
        if(other.gameObject.tag == tag)
        {
            Debug.Log("Hit"+tag);
            other.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
            StartCoroutine(ExplodeBeforeDestroy());
        }
        else if(other.gameObject.tag == "Environnement" || other.gameObject.tag == "Obstacle") // If the bullet hits an obstacle, destroy the bullet or bounce if it cans
        {
            if(!canBounce || maxBounces <= 0)
            {
                StartCoroutine(ExplodeBeforeDestroy());
            }
            else
            {
                // Bounce once
                var speed = lastVelocity.magnitude;
                var direction = Vector2.Reflect(lastVelocity.normalized, other.contacts[0].normal);
                float rot_y = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
                transform.rotation = Quaternion.Euler(0, 0, rot_y-90); // Convert the angle to quaternion
                rb.velocity= direction * Mathf.Max(speed, 0f);
                maxBounces--;
            }
        }
        else if(other.gameObject.tag == "Bullet")
        {
            StartCoroutine(ExplodeBeforeDestroy());
        }
    }
    public void Initialise(float damage, float speed, float max_distance, int life_time, bool canBounce, string tag, int maxBounces = 0) // Initialise the bullet
    {
        this.damage = damage;
        this.speed = speed;
        this.max_distance = max_distance;
        this.canBounce = canBounce;
        this.tag = tag;
        this.maxBounces = maxBounces;
        StartCoroutine(DestroyBullet(life_time)); // Destroy the bullet according to the life time
    }
    IEnumerator DestroyBullet(int life_time)
    {

        yield return new WaitForSeconds(life_time);
        StartCoroutine(FadeAwayBeforeDestroy());
    }
    IEnumerator ExplodeBeforeDestroy()
    {
        Debug.Log("Explode");
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    IEnumerator FadeAwayBeforeDestroy()
    {
        Debug.Log("FadeAway");
        animator.SetTrigger("FadeAway");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}