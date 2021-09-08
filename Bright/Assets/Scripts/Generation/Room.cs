using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public string type;
    public int difficulty;
    public int variant;
    public GameObject room;

    public Room(string typeInput, int difficultyInput, int variantInput)
    {
        type = typeInput;
        difficulty = difficultyInput;
        variant = variantInput;
    }

    public Room(string typeInput)
    {
        type = typeInput;
    }
}