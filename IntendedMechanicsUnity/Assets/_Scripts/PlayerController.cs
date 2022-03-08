using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] Transform _playerCamera;
    [SerializeField] float _mouseSensitivity = 3.5f;
    [SerializeField] bool _lockCursor = true;
    [SerializeField] float _walkSpeed = 6.0f;
    [SerializeField] float _sprintIncrease = 3.0f;
    [SerializeField] float _jumpSpeed = 5.5f;
    [SerializeField] float _gravity = -13.0f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject interactUI;

    public static UnityEvent respawnPlayer;
    RaycastHit hit;
    bool interacting = false;
    public LayerMask interactableLayermask = 6;
    private GameObject respawnPoint;
    bool menuOpen = false;
    bool sprinting = false;
    private int numJumps = 1;
    private float _cameraPitch = 0.0f;
    float _velocityY = 0.0f;
    CharacterController controller = null;

    // Start is called before the first frame update
    void Awake()
    {
        hit = new RaycastHit();
        respawnPlayer = new UnityEvent();
        respawnPlayer.AddListener(RespawnPlayer);
        interactableLayermask = LayerMask.GetMask("Interactable");
        respawnPoint = GameObject.Find("RespawnPoint");
        controller = GetComponent<CharacterController>();
        if (_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        respawnPoint = GameObject.Find("RespawnPoint");
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseMenu");
        }
        DoRaycastLogic();
        UpdateMouseLook();
        UpdateMovement();
    }

    private void DoRaycastLogic()
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, 2, interactableLayermask))
        {
            interacting = true;
            interactUI.SetActive(true);
        } 
        else
        {
            interacting = false;
            interactUI.SetActive(false);
        }
    }

    void UpdateMouseLook()
    {
        if (_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            _cameraPitch -= targetMouseDelta.y * _mouseSensitivity;
            _cameraPitch = Mathf.Clamp(_cameraPitch, -85.0f, 85.0f);

            _playerCamera.localEulerAngles = Vector3.right * _cameraPitch; // I dont know how local Euler Angles works ------ research later
            transform.Rotate(Vector3.up * targetMouseDelta.x * _mouseSensitivity);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void UpdateMovement()
    {

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        /*inputDir.Normalize() would cause the controller to move at a static value of 1 when moving diagonally
         * While this is usually preferable, I am quirky and do not want this for my project
         */
        _velocityY += _gravity * Time.deltaTime;

        if (controller.isGrounded)
        {
            numJumps = numJumps < 1 ? 1 : numJumps;
            _velocityY = 0.0f;
        }

        if (Input.GetButtonDown("Jump") && numJumps > 0)
        {
            numJumps--;
            _velocityY = _jumpSpeed;
        }
        float moveSpeed = _walkSpeed;
        if (sprinting)
        {
            moveSpeed += _sprintIncrease;
        }
        Vector3 velocity = (transform.forward * targetDir.y + transform.right * targetDir.x) * moveSpeed + Vector3.up * _velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (transform.position.y < -10)
        {
            RespawnPlayer();
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            sprinting = true;
        if (ctx.canceled)
            sprinting = false;
    }

    public void OnEscape()
    {
        menuOpen = !menuOpen;
        ToggleCursor();
        pauseMenu.SetActive(menuOpen);
    }

    public void OnInteract()
    {
        if (interacting)
        {
            hit.collider.GetComponent<OpenDevLog>().Interact();
        }
    }

    public void RespawnPlayer()
    {
        transform.position = respawnPoint.transform.position;
    }

    public void ToggleCursor()
    {
        _lockCursor = !_lockCursor;
        Cursor.visible = !Cursor.visible;
    }
}
