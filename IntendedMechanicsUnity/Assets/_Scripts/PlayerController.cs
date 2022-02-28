using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] Transform _playerCamera;
    [SerializeField] float _mouseSensitivity = 3.5f;
    [SerializeField] bool _lockCursor = true;
    [SerializeField] float _walkSpeed = 6.0f;
    [SerializeField] float _gravity = -13.0f;

    private float _cameraPitch = 0.0f;
    float _velocityY = 0.0f;
    CharacterController controller = null;

    // Start is called before the first frame update
    void Start()
    {
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
        UpdateMouseLook();
        UpdateMovement();
    }
    
    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _cameraPitch -= targetMouseDelta.y * _mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, -85.0f, 85.0f);

        _playerCamera.localEulerAngles = Vector3.right * _cameraPitch; // I dont know how local Euler Angles works ------ research later
        transform.Rotate(Vector3.up * targetMouseDelta.x * _mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        /*inputDir.Normalize() would cause the controller to move at a static value of 1 when moving diagonally
         * While this is usually preferable, I am quirky and do not want this for my project
         */

        if (controller.isGrounded)
            _velocityY = 0.0f;

        _velocityY += _gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * targetDir.y + transform.right * targetDir.x) * _walkSpeed + Vector3.up * _velocityY;

        controller.Move(velocity * Time.deltaTime);
    }
}
