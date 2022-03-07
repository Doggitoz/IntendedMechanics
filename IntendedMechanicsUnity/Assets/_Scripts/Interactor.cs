using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public LayerMask interactableLayermask = 6;
    [SerializeField] private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = transform.gameObject.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, 2, interactableLayermask))
        {
            Debug.Log(hit.collider.name);
        }
    }
}
