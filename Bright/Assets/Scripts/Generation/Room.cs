using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public string type;
    public int scaling;
    public int varient;

    public Room(string typeInput, int varientInput)
    {
        type = typeInput;
        varient = varientInput;
    }
}