using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerkData
{
    public static Perk AddPerk(PerkType perk)
    {
        string name = "";
        PerkType perkType = perk;

        string path = "Sprites/Perks/" + perkType.ToString();

        Perk temp = new Perk
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
    Test1 = 20,
    Test2 = 5,
    Test3 = 15,
    Test4 = 25,
    Test5 = 35,
}
