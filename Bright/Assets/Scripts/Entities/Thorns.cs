using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour
{
    public int activateTime = 1;
    public int damage = 2;

    public Collider2D col;

    public void Awake()
    {
        col.enabled = false;
    }

    public void Start()
    {
        StartCoroutine("StartAttack");
    }

    public IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(activateTime);
        col.enabled = true;
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
