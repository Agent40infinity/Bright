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
            Icon = Resources.Load<Sprite>(path),
        };

        return temp;
    }
}

public enum PerkType
{ 
    Adrenaline = 20,
    ShootingStar = 5,
    GlassCannon = 15,
    SoulHeart = 15,
    Parasitic = 5,
    Triplets = 5,
    MoneyBags = 15,
    Juggernaut = 10,
    FollowTheLeader = 5,
    WellFed = 10,
    SpiritsWrath = 10,
    Blinded = 15
}