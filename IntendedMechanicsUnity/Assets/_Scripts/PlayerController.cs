using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Set in Inspector")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField] private AnimationCurve curve;


    private Vector3 startingPoint;
    private Vector3 targetPoint;
    float timeSinceLastMovement = 0f;
    RaycastHit hit;
    string direction = "right";

    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.localPosition;
        targetPoint = startingPoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeSinceLastMovement += Time.deltaTime;
        transform.localPosition = Vector3.Lerp(startingPoint, targetPoint, curve.Evaluate(7 * timeSinceLastMovement));
        if (timeSinceLastMovement > 1f / 7f)
        {
            if (checkForInput())
            {
                if (!checkForBounds())
                {
                    Debug.Log(direction);
                    doRaycastStuff();
                }
            }
        }
    }

    private void doRaycastStuff()
    {
        Vector3 rayDirection = new Vector3();
        if (direction == "up")
        {
            rayDirection = Vector3.up;
        }
        if (direction == "down")
        {
            rayDirection = Vector3.down;
        }
        if (direction == "right")
        {
            rayDirection = Vector3.right;
        }
        if (direction == "left")
        {
            rayDirection = Vector3.left;
        }


        //Check raycast. If object in front, handle different. If wall object, set targetpoint to startpoint

        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        Debug.Log(startingPoint + " " + transform.position);

        if (Physics.Raycast(transform.position, transform.TransformDirection(rayDirection), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, targetPoint * hit.distance, Color.yellow);
            if (hit.distance <= 1)
            {
                Debug.Log("Did Hit");
                Debug.Log(hit.collider);
                Debug.Log(hit.point);

                //LOGIC FOR DETECTING OBJECTS IN FRONT OF YOU
                if (hit.collider.CompareTag("Collider"))
                {
                    targetPoint = startingPoint;
                }
            }
        }
    }

    bool checkForInput()
    {
        timeSinceLastMovement = 0;
        startingPoint = targetPoint;
        if (Input.GetKey(KeyCode.A))
        {
            targetPoint.x -= 1;
            direction = "left";
        }
        else if (Input.GetKey(KeyCode.D))
        {
            targetPoint.x += 1;
            direction = "right";
        }
        else if (Input.GetKey(KeyCode.W))
        {
            targetPoint.y += 1;
            direction = "up";
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetPoint.y -= 1;
            direction = "down";
        }
        else
        {
            timeSinceLastMovement = 1;
            return false;
        }
        return true;
    }

    bool checkForBounds()
    {
        if (targetPoint.x > maxBounds.x)
        {
            targetPoint.x = maxBounds.x;
            return true;
        }
        if (targetPoint.x < minBounds.x)
        {
            targetPoint.x = minBounds.x;
            return true;
        }
        if (targetPoint.y > maxBounds.y)
        {
            targetPoint.y = maxBounds.y;
            return true;
        }
        if (targetPoint.y < minBounds.y)
        {
            targetPoint.y = minBounds.y;
            return true;
        }
        return false;
    }
}
