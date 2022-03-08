using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject CreditsMenu;

    bool creditsOpen = false;

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void RestartGame()
    {
        GameManager.Instance.changeScene(0);
    }

    public void SwitchMenu()
    {
        creditsOpen = !creditsOpen;
        MainMenu.SetActive(!creditsOpen);
        CreditsMenu.SetActive(creditsOpen);
    }

}
