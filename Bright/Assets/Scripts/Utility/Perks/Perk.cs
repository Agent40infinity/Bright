using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk
{
    private string perkName;
    private Sprite icon;
    private PerkType perkType;

    public string Name
    {
        get { return perkName; }
        set { perkName = value; }
    }

    public Sprite Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    public PerkType PerkType
    {
        get { return perkType; }
        set { perkType = value; }
    }

    public void ItemCreation()
    { 
        
    }
}
