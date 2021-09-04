using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject perk;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(perk, transform);
        }
    }
}
