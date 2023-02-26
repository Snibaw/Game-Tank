using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float max_distance = 2f;
    [SerializeField] private float damage = 1f;
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
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit"+other.gameObject.name);
            // other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
