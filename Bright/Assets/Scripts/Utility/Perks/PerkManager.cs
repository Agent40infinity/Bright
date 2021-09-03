using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public Perk perkInfo;
    public Player player;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        PerkStart();
    }

    public void PerkStart()
    {
        if (player.perkList.Count != System.Enum.GetValues(typeof(PerkType)).Length)
        {
            perkInfo = PerkData.AddPerk(PerkSelection());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PerkType PerkSelection()
    {
        

        int chance = Random.Range(0, System.Enum.GetValues(typeof(PerkType)).Length);
        PerkType temp = (PerkType)System.Enum.GetValues(typeof(PerkType)).GetValue(chance);

        for (int i = 0; i < player.perkList.Count; i++)
        {
            if (player.perkList[i].PerkType == temp)
            {
                PerkSelection();
            }
        }

        return temp;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                player.perkList.Add(perkInfo);
                Debug.Log("Enum Length: " + System.Enum.GetValues(typeof(PerkType)).Length + " | Perk Length: " + player.perkList.Count);
                Destroy(gameObject);
                break;
        }
    }
}