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
            PerkType perk = PerkDropTable();

            switch (PerkValid(perk))
            {
                case true:
                    perkInfo = PerkData.AddPerk(perk);
                    break;
                case false:
                    PerkStart();
                    break;
            }
        }
        else
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }   
        }
    }

    public bool PerkValid(PerkType perk)
    {   
        for (int i = 0; i < player.perkList.Count; i++)
        {
            if (player.perkList[i].PerkType == perk)
            {
                return false;
            }
        }

        return true;
    }

    public PerkType PerkDropTable()
    {
        List<int> perkChances = new List<int>();
        int typeLength = System.Enum.GetValues(typeof(PerkType)).Length;
        int index = 0;

        for (int i = 0; i < typeLength; i++)
        {
            PerkType temp = (PerkType)System.Enum.GetValues(typeof(PerkType)).GetValue(i);
            perkChances.Add((int)temp);
        }

        int selection = (int)Random.Range(0, AddChance(perkChances.Count - 1, perkChances) + 1);

        if (selection <= AddChance(0, perkChances))
        {
            index = 0;
        }

        for (int i = 1; i < typeLength; i++)
        {
            if (selection <= AddChance(i, perkChances) && selection > AddChance(i - 1, perkChances))
            {
                index = i;
            }
        }

        return (PerkType)System.Enum.GetValues(typeof(PerkType)).GetValue(index);
    }

    public float AddChance(int index, List<int> chance)
    {
        float temp = 0;

        for (int i = 0; i < index + 1; i++)
        {
            temp += chance[i];
        }

        return temp;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                player.perkList.Add(perkInfo);
                Debug.Log(perkInfo.PerkType + " Picked up | Enum Length: " + System.Enum.GetValues(typeof(PerkType)).Length + " | Perk Length: " + player.perkList.Count);
                if (gameObject != null)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }
}