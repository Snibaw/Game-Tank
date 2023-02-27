using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private string tag = "Player";
    private float damage = 1f;
    private int life_time = 50;
    private Transform startPosition;
    private Collider2D laserCollider;
    // Boucing
    Vector2 lastVelocity;
    // Start is called before the first frame update
    void Awake()
    {
        transform.Rotate(0, 0, 90);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == tag)
        {
            Debug.Log("Hit"+tag);
            other.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
        }
    }
    public void Initialise(float damage, int life_time, string tag)
    {
        this.damage = damage;
        this.tag = tag;
        this.life_time = life_time;
        StartCoroutine(DestroyBullet(life_time)); // Destroy the bullet according to the life time
    }
    IEnumerator DestroyBullet(int life_time)
    {
        yield return new WaitForSeconds(life_time);
        Destroy(gameObject);
    }
}
