using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Security;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static bool gameActive = false; //Is the game paused.
    public static bool enemiesExist = false;
    public static WorldState worldState;
    public static bool startTransition = false;
    public static AudioMixer masterMixer; //Creates reference for the menu music
    public static Vector2Int currentRoom;
    public static GameObject currentWorld;
    public static GameObject wisp;
    public static Dictionary<string, KeyCode> keybind = new Dictionary<string, KeyCode> //Dictionary to store the keybinds.
    {
        { "MoveUp", KeyCode.W },
        { "MoveDown", KeyCode.S },
        { "MoveLeft", KeyCode.A },
        { "MoveRight", KeyCode.D },
        { "ShootUp", KeyCode.UpArrow },
        { "ShootDown", KeyCode.DownArrow },
        { "ShootLeft", KeyCode.LeftArrow },
        { "ShootRight", KeyCode.RightArrow },
        { "Melee", KeyCode.E },
        { "Heal", KeyCode.R },
        { "Parry", KeyCode.LeftShift },
        { "TrueSight", KeyCode.X },
        { "Interact", KeyCode.F },
        { "SwitchWeapon", KeyCode.Tab },
        { "Inventory", KeyCode.I },
        { "Pause", KeyCode.Escape }
    };

    public static bool maxPerks = false;

    [Header("References")]
    public HealthManager healthManager;
    public GoldManager goldManager;
    public GameObject overlay;
    public DungeonCamera dungeonCamera;
    public static GameObject dungeonRef;
    public static GameObject trueWorldRef;

    public void Awake()
    {
        masterMixer = Resources.Load("Music/Mixers/Master") as AudioMixer; //Loads the MasterMixer for renference.

        if (File.Exists(Application.persistentDataPath + "/settings.json")) //Checks if the file already exists and loads the file if it does.
        {
            SystemConfig.LoadSettings();
        }
        else //Else, creates the data for the new file.
        {
            SystemConfig.SaveSettings(); //Saves the new data as a new file "Settings".
        }

        overlay.SetActive(false);
    }

    public void Update()
    {
        CurrentWorld();
    }

    public void CurrentWorld()
    {
        switch (worldState)
        {
            case WorldState.Normal:
                switch (startTransition)
                {
                    case true:
                        ChangeWorld(false);
                        break;
                }
                break;
            case WorldState.Other:
                switch (startTransition)
                {
                    case true:
                        ChangeWorld(true);
                        break;
                }
                break;
        }
    }

    public void StartGame()
    {
        gameActive = true;
        currentWorld = Instantiate(Resources.Load("Prefabs/Utility/World") as GameObject, Vector3.zero, Quaternion.identity);
        overlay.SetActive(true);
        healthManager.SetUpHealth();
        dungeonCamera.OcclusionCulling();
        dungeonCamera.PathfindingCall();
    }

    public void LeaveGame()
    {
        gameActive = false;
        Destroy(currentWorld);
        Destroy(wisp);
        currentWorld = null;
        dungeonCamera.ResetCamera();
        healthManager.ClearPrevious();
        goldManager.ClearPrevious();
        overlay.SetActive(false);
    }

    public void ChangeWorld(bool isTrueWorld)
    {
        switch (isTrueWorld)
        {
            case true:
                trueWorldRef.SetActive(true);
                dungeonRef.SetActive(false);
                break;
            case false:
                dungeonRef.SetActive(true);
                trueWorldRef.SetActive(false);
                break;
        }

        startTransition = false;
    }
}

public enum WorldState
{ 
    Normal,
    Other
}