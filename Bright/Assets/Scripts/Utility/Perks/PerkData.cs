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
    Test1,
    Test2,
    Test3
}
