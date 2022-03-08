using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{

    public static GameManager instance;

    public float mouseSensitivity = 3.5f;

    // Start is called before the first frame update

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
    }

    public void changeScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
