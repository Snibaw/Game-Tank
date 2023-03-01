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
    private Rigidbody2D rb;
    private Vector2 startPosition;
    // Boucing
    Vector2 lastVelocity;
    // Start is called before the first frame update
    void Start()
    {
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
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == tag)
        {
            Debug.Log("Hit"+tag);
            other.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Environnement" || other.gameObject.tag == "Obstacle")
        {
            if(!canBounce)
            {
                Destroy(gameObject);
            }
            else
            {
                // Bounce once
                var speed = lastVelocity.magnitude;
                var direction = Vector2.Reflect(lastVelocity.normalized, other.contacts[0].normal);
                float rot_y = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle of the direction in y axis
                transform.rotation = Quaternion.Euler(0, 0, rot_y-90); // Convert the angle to quaternion
                rb.velocity= direction * Mathf.Max(speed, 0f);
                canBounce = false;
            }
        }
    }
    public void Initialise(float damage, float speed, float max_distance, int life_time, bool canBounce, string tag)
    {
        this.damage = damage;
        this.speed = speed;
        this.max_distance = max_distance;
        this.canBounce = canBounce;
        this.tag = tag;
        StartCoroutine(DestroyBullet(life_time)); // Destroy the bullet according to the life time
    }
    IEnumerator DestroyBullet(int life_time)
    {
        yield return new WaitForSeconds(life_time);
        Destroy(gameObject);
    }
}
