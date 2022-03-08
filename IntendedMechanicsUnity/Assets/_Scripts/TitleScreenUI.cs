using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private Slider slider;
    [SerializeField] private Text sensText;

    bool settingsOpen = false;
    float mouseSensitivity = 3.5f;

    // Start is called before the first frame update
    void Awake()
    {
        mouseSensitivity = 3.5f;
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity");
        } 
        else
        {
            PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
        }
        GameManager.Instance.mouseSensitivity = mouseSensitivity;
        slider.GetComponent<Slider>().value = Mathf.Clamp(mouseSensitivity, 0.1f, 7);
        sensText.text = "" + mouseSensitivity;
    }

    private void Update()
    {
        if (mouseSensitivity != PlayerPrefs.GetFloat("Sensitivity"))
        {
            PlayerPrefs.SetFloat("Sensitivity", mouseSensitivity);
        }
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public void StartGame()
    {
        GameManager.Instance.changeScene(1);
    }

    public void SwitchMenu()
    {
        settingsOpen = !settingsOpen;
        MainMenu.SetActive(!settingsOpen);
        SettingsMenu.SetActive(settingsOpen);
    }

    public void UpdateSensitivty()
    {
        GameManager.Instance.mouseSensitivity = slider.GetComponent<Slider>().value;
        sensText.text = "" + GameManager.Instance.mouseSensitivity;
    }
}
