using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    public Pause pause;

    public void Menu() //Trigger for menu button
    {
        pause.Menu();
        StartCoroutine("DisableDeath");
    }

    public IEnumerator DisableDeath()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
