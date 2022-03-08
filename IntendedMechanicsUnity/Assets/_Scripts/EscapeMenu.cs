using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private Button QuitButton;

    // Start is called before the first frame update
    void Start()
    {
        MenuUI.SetActive(false);
    }

    public void MainMenu()
    {
        GameManager.Instance.changeScene(0);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
