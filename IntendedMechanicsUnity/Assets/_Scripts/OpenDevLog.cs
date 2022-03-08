using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;

public class OpenDevLog : MonoBehaviour
{

    [SerializeField] private Canvas DevLogUI;

    private bool isActive = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        isActive = !isActive;
        DevLogUI.gameObject.SetActive(isActive);
        if (isActive == false)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().ToggleCursor();
        }
    }
}
