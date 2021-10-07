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
                dialogue.Add("Press " + Settings.CheckSpecial(GameManager.keybind["MoveUp"]) + ", " + Settings.CheckSpecial(GameManager.keybind["MoveLeft"]) + ", " + Settings.CheckSpecial(GameManager.keybind["MoveDown"]) + ", or " + Settings.CheckSpecial(GameManager.keybind["MoveRight"]) + " to Move");
                break;
            case TutorialState.Shoot:
                keybinds = new KeyCode[] { GameManager.keybind["ShootUp"], GameManager.keybind["ShootDown"], GameManager.keybind["ShootLeft"], GameManager.keybind["ShootRight"] };
                dialogue.Add("Press " + Settings.CheckSpecial(GameManager.keybind["ShootUp"]) + ", " + Settings.CheckSpecial(GameManager.keybind["ShootLeft"]) + ", " + Settings.CheckSpecial(GameManager.keybind["ShootDown"]) + ", or " + Settings.CheckSpecial(GameManager.keybind["ShootRight"]) + " to Shoot");
                break;
            case TutorialState.Switch:
                keybinds = new KeyCode[] { GameManager.keybind["SwitchWeapon"] };
                dialogue.Add("Press " + Settings.CheckSpecial(GameManager.keybind["SwitchWeapon"]) + " to Switch Weapons");
                break;
            case TutorialState.Melee:
                keybinds = new KeyCode[] { GameManager.keybind["Melee"] };
                dialogue.Add("Press " + Settings.CheckSpecial(GameManager.keybind["Melee"]) + " to perform a Close Range Attack");
                break;
            case TutorialState.SwitchWorlds:
                keybinds = new KeyCode[] { GameManager.keybind["TrueSight"] };
                dialogue.Add("Press " + Settings.CheckSpecial(GameManager.keybind["TrueSight"]) + " to enter the 'True World'");
                break;
            case TutorialState.Interact:
                keybinds = new KeyCode[] { GameManager.keybind["Interact"] };
                dialogue.Add("Press " + Settings.CheckSpecial(GameManager.keybind["Interact"]) + " to Interact");
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
        dialogueRef.dialogue = dialogue;
        dialogueRef.dialogueState = DialogueState.Load;

        oneTimeState = OneTimeState.Finnished;
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

        dialogueRef.ClearDialogue();

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