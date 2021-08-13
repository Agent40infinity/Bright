using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        { "Interact", KeyCode.F },
        { "SwitchWeapon", KeyCode.Tab },
        { "Pause", KeyCode.Escape },
        { "Inventory", KeyCode.I },

    };
}
