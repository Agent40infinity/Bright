using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk : MonoBehaviour
{
    private string perkName;
    private Sprite portrait;
    private PerkType perkType;

    public string Name
    {
        get { return perkName; }
        set { perkName = value; }
    }

    public Sprite Portrait
    {
        get { return portrait; }
        set { portrait = value; }
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
