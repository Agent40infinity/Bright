using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour
{
    public float activateTime = 1;
    public float deactivateTime = 0.5f;
    public int damage = 1;

    public Collider2D col;

    public void Awake()
    {
        col.enabled = false;
    }

    public void Start()
    {
        StartCoroutine("Lifecycle");
    }

    public IEnumerator Lifecycle()
    {
        yield return new WaitForSeconds(activateTime);
        col.enabled = true;
        yield return new WaitForSeconds(deactivateTime);
        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                other.gameObject.GetComponent<PlayerHealth>().DamagePlayer(damage);
                break;
        }
    }
}
