using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PauseMenu
{
    public class Pause : MonoBehaviour
    {
        #region Variables
        public PauseState pauseState = PauseState.Playing; //Checks whether or not the game is paused
        public GameObject pauseMenu;
        public GameObject options, main, mainBackground, overlay, background; //Creates reference for the pause menu
        public Settings optionsMenu;
        public Menu menu;
        public FadeController fade;
        #endregion

        #region General
        public void Update() //Ensures the pause menu can function
        {
            if (Input.GetKeyDown(GameManager.keybind["Pause"]) && GameManager.gameActive) //Show pause menu
            {
                switch (pauseState)
                {
                    case PauseState.Playing:
                        PauseG();
                        break;
                    case PauseState.Pause:
                        ResumeG();
                        break;
                }
            }
        }
        #endregion

        #region Pause
        public void ResumeG() //Trigger for resuming game and resume button
        {
            optionsMenu.ChangeBetween(0);
            OptionsCall(false);
            pauseMenu.SetActive(false);
            background.SetActive(false);
            Time.timeScale = 1f;
            pauseState = PauseState.Playing;
        }

        public void PauseG() //Trigger for pausing game
        {
            pauseMenu.SetActive(true);
            background.SetActive(true);
            Time.timeScale = 0f;
            pauseState = PauseState.Pause;
        }

        public void Menu() //Trigger for menu button
        {
            Debug.Log("Excuse me");
            StartCoroutine("ChangeToMain");
        }

        IEnumerator ChangeToMain()
        {
            Time.timeScale = 1f;
            fade.FadeOut();
            yield return new WaitForSeconds(2);
            pauseState = PauseState.Playing;
            pauseMenu.SetActive(false);
            main.SetActive(true);
            //overlay.SetActive(false);
            mainBackground.SetActive(true);
            background.SetActive(false);

            fade.FadeIn();
            menu.music.Play();
            GameManager.gameActive = false;

        }

        public void OptionsCall(bool toggle)
        {
            optionsMenu.ToggleOptions(toggle, LastMenuState.PauseMenu);
        }
        #endregion
    }

    public enum PauseState
    { 
        Playing,
        Pause
    }
}