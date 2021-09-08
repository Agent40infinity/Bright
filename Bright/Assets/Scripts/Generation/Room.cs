﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    [Header("Attributes")]
    public string type;
    public int difficulty;
    public int variant;

    [Header("Active Slot")]
    public GameObject room;
    public VisitedState visitedState = VisitedState.Empty;

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

public enum VisitedState
{ 
    Empty,
    Occupied,
    Clear
}