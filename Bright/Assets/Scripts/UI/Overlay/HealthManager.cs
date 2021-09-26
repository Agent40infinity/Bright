using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*---------------------------------/
 * Script created by Aiden Nathan.
 *---------------------------------*/

public class HealthManager : MonoBehaviour
{
    [Header("Position")]
    public Vector2 heartOrigin;
    public float heartOffset;

    [Header("Hearts")]
    public List<Animator> anim = new List<Animator>();
    public List<GameObject> hearts = new List<GameObject>();
    private float imagesPerHeart; //Defines how many Images there are per heart slot.

    [Header("Reference")]
    public PlayerHealth health;
    public GameObject heartParent;

    public void SetUpHealth()
    {
        ClearPrevious();

        health = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();

        for (int i = 0; i < health.maxHealth; i++)
        {
            Vector2 heartPos = new Vector2(heartOrigin.x + (heartOffset * i), heartOrigin.y);
            GameObject heart = Instantiate(heartParent, heartPos, Quaternion.identity);
            heart.transform.SetParent(transform, false);

            hearts.Add(heart);
            anim.Add(heart.GetComponent<Animator>());
        }

        CalculateHealth();
    }

    public void ClearPrevious()
    {
        health = null;

        for (int i = 0; i < hearts.Count; i++)
        {
            Destroy(hearts[i]);
        }

        anim.Clear();
        hearts.Clear();
    }

    public void CalculateHealth() //Calculates the health per slot.
    {
        imagesPerHeart = health.maxHealth / (float)(anim.Count * 2.5f); //0.5
    }

    public void Update() //Changes the image depending on how much health the player has.
    {
        DisplayHealth();
    }

    public void DisplayHealth()
    { 
        for (int i = 0; i < anim.Count; i++)
        {
            if (health.curHealth <= (imagesPerHeart * 2) * (i + 1))
            {
                anim[i].ResetTrigger("Recover");
                anim[i].SetTrigger("Lose");
            }
        }
    }

    public void RecoverHealth()
    {
        for (int i = 0; i < anim.Count; i++)
        {
            anim[i].ResetTrigger("Lose");
            anim[i].SetTrigger("Recover");
        }
    }
}