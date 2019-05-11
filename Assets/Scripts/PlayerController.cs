using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Config
    public float movementSpeed = 2f;

    // Inputs
    private Vector3 moveInput;
    private Vector3 moveVelocity;

    // Components
    private Camera mainCamera;
    private Rigidbody player;

    // Use this for initialization
    void Start () {
        // Grab the Rigidbody and set it as the "player"
        player = GetComponent<Rigidbody>();
        // Locate the camera object
        mainCamera = FindObjectOfType<Camera>();
    }
    
    // Update is called once per frame
    void Update () {
        UpdatePlayerMovement();
        UpdatePlayerRotation();
    }

    void FixedUpdate()
    {
        // Update the player's movement
        player.velocity = moveVelocity;
    }

    void UpdatePlayerMovement()
    {
        // ** -- MOVEMENT -- **
        // Grab the input of the player, leaving the Y value as 0f
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        // Multiply that value by our speed
        moveVelocity = moveInput * movementSpeed;
    }

    void UpdatePlayerRotation()
    {
        // ** -- ROTATION -- **
        // Create a ray from the camera's position to the mouse position
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        // Create a mathematical ray for the camera's ray to hit
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        // Store the length of the ray
        float rayLength;
        // The rayLength value is defined below, the distance from the groundPlane and the camera
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            // Create a vector 3 based on the point in which the camera's ray intersects with the plane
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            // Fix the player's y position so they don't rotate down/up
            pointToLook.y = transform.position.y;
            // Set the player's transform to look at the intersect
            transform.LookAt(pointToLook);
        }
    }

}
