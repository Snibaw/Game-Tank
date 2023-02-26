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
        conquared_distance = Vector2.Distance(transform.position, startPosition);
        if(conquared_distance >= max_distance)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == tag)
        {
            Debug.Log("Hit"+tag);
            other.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Environnement")
        {
            if(!canBounce)
            {
                Destroy(gameObject);
            }
        }
    }
    public void Initialise(float damage, float speed, float max_distance, bool canBounce, string tag)
    {
        this.damage = damage;
        this.speed = speed;
        this.max_distance = max_distance;
        this.canBounce = canBounce;
        this.tag = tag;
    }
}
