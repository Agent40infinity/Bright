using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float movementSpeed;
    public int damage;
    public Vector2 shotDirection;

    void Update()
    {
        transform.Translate(shotDirection.normalized * movementSpeed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().DamagePlayer(damage);
            Explode();
        }
    }

    void Explode()
    {
        // implement particle pew pew later
        Destroy(gameObject);
    }
}
