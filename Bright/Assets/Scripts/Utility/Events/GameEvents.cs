using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    [Header("General")]
    public List<GameObject> Objectives;
    public bool[] Checks;

    public EventState eventState;
    public TutorialState tutorialState;
    public OneTimeState oneTimeState;

    [Header("Reference")]
    public Dialogue dialogueRef;

    public void Start()
    {
        SelectEvent();
    }

    public void SelectEvent()
    {
        switch (eventState)
        {
            case EventState.Tutorial:
                Checks = new bool[System.Enum.GetValues(typeof(TutorialState)).Length];
                break;
        }
    }

    public void Update()
    {
        Event();
    }

    public void Event()
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
        KeyCode[] keybinds = null;
        List<string> dialogue = new List<string>();
        GameObject objective = null;
        int index = (int)tutorialState;

        switch (tutorialState)
        {
            case TutorialState.Move:
                keybinds = new KeyCode[] { GameManager.keybind["MoveUp"], GameManager.keybind["MoveDown"], GameManager.keybind["MoveLeft"], GameManager.keybind["MoveRight"] };
                dialogue.Add("Press " + GameManager.keybind["MoveUp"].ToString() + ", " + GameManager.keybind["MoveLeft"].ToString() + ", " + GameManager.keybind["MoveDown"].ToString() + ", or " + GameManager.keybind["MoveRight"].ToString() + " to Move");
                break;
            case TutorialState.Shoot:
                keybinds = new KeyCode[] { GameManager.keybind["ShootUp"], GameManager.keybind["ShootDown"], GameManager.keybind["ShootLeft"], GameManager.keybind["ShootRight"] };
                break;
            case TutorialState.Switch:
                keybinds = new KeyCode[] { GameManager.keybind["SwitchWeapon"] };
                break;
            case TutorialState.Melee:
                keybinds = new KeyCode[] { GameManager.keybind["Melee"] };
                break;
            case TutorialState.SwitchWorlds:
                keybinds = new KeyCode[] { GameManager.keybind["TrueSight"] };
                break;
            case TutorialState.Interact:
                keybinds = new KeyCode[] { GameManager.keybind["Interact"] };
                break;
        }

        switch (oneTimeState)
        {
            case OneTimeState.None:
                PushObjective(objective, dialogue);
                break;
        }

        SwitchStateCheck(index, keybinds);
    }
                
    public void PushObjective(GameObject objective, List<string> dialogue)
    {
        /*dialogueRef.dialogue = dialogue;
        dialogueRef.dialogueState = DialogueState.Load;

        oneTimeState = OneTimeState.Finnished;*/
    }

    public void SwitchStateCheck(int index, KeyCode[] keybinds)
    {
        if (!Checks[index])
        {
            Checks[index] = KeybindCheck(keybinds);
        }
        else
        {
            NextState(index);
        }
    }

    public bool KeybindCheck(KeyCode[] keycode)
    {
        if (keycode == null)
        {
            return true;
        }

        for (int i = 0; i < keycode.Length; i++)
        {
            if (Input.GetKeyDown(keycode[i]))
            {
                return true;
            }
        }

        return false;
    }

    public void NextState(int index)
    {
        int newIndex = index + 1;

        if (newIndex != System.Enum.GetValues(typeof(TutorialState)).Length)
        {
            oneTimeState = OneTimeState.None;
            tutorialState = (TutorialState)System.Enum.GetValues(typeof(TutorialState)).GetValue(newIndex);
        }
        else
        {
            eventState = EventState.None;
            Debug.Log("My tutorial worked when I feel brain damaged fuckers, suck a dick I'm a God");
        }
    }
}

public enum EventState
{
    Tutorial,
    None
}

public enum TutorialState
{ 
    Move,
    Shoot,
    Switch,
    Melee,
    SwitchWorlds,
    Interact,
}

public enum OneTimeState
{ 
    Finnished,
    None
}