using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkData : MonoBehaviour
{
    public static Perk AddPerk(string perkName)
    {
        string name = "";
        PerkType perkType = PerkType.None;

        string path = "Sprites/Perks/" + perkType.ToString();

        Perk temp = new Perk()
        {
            Name = name,
            PerkType = perkType,
            Portrait = Resources.Load<Sprite>(path),
        };

        return temp;
    }
}

public enum PerkType
{ 
    None
}
