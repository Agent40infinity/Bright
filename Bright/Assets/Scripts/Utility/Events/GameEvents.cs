using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [Header("General")]
    public List<GameObject> Objectives;

    public EventState eventState;

    public void Update()
    {   
        switch (eventState)
        {
            case EventState.Tutorial:
                TutorialEvent();
                break;
        }
    }

    public void TutorialEvent()
    {

    }
}

public enum EventState
{ 
    Tutorial,
    None
}
