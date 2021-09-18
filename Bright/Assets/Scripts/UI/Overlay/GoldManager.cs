using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public static int gold;

    public TextMeshProUGUI textRef;

    public void Update()
    {
        DisplayGold();

        if (Input.GetKeyDown(KeyCode.P))
        {
            gold++;
        }
    }

    public void DisplayGold()
    {
        textRef.text = gold.ToString();
    }

    public void ClearPrevious()
    {
        gold = 0;
    }
}
