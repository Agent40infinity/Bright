using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public GameObject end, main, mainBackground;

    public Menu menu;
    public FadeController fade;
    public GameObject gameEvent;

    public GameManager gameManager;

    public void Menu() //Trigger for menu button
    {
        StartCoroutine("ChangeToMain");
    }

    IEnumerator ChangeToMain()
    {
        if (GameObject.FindWithTag("GameEvent"))
        {
            gameEvent = GameObject.FindWithTag("GameEvent");
            gameEvent.SetActive(false);
            gameEvent = null;
        }

        fade.FadeOut();
        yield return new WaitForSeconds(2);
        end.SetActive(false);
        Debug.Log("Heh?");
        main.SetActive(true);
        mainBackground.SetActive(true);

        fade.FadeIn();
        menu.music.Play();
        gameManager.LeaveGame();
    }
}
