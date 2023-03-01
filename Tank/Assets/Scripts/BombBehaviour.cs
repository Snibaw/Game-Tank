using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    
    private string avoidTag = "Undefined";
    private float explosionTime = 3f;
    private float explosionRadius = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionTime);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach(Collider2D collider in colliders)
        {
            if(collider.tag == avoidTag)
            {
                continue;
            }
            if(collider.tag == "Player" || collider.tag == "Enemy")
            {
                collider.GetComponentInParent<Health>().TakeDamage(1f);
            }
            if(collider.tag == "Obstacle")
            {
                Destroy(collider.gameObject);
            }
        }
        Destroy(gameObject);
    }
    public void SetAvoidTag(string tag)
    {
        this.avoidTag = tag;
    }
    public float GetExplosionRadius()
    {
        return explosionRadius;
    }
    public float GetExplosionTime()
    {
        return explosionTime;
    }
}
