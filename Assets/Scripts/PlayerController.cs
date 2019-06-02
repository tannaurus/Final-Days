using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // Config
    public float movementSpeed = 2f;
    public float lookSpeed = 2f;

    // Inputs
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Vector3 movement;

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
        UpdatePlayerRotation();
    }

    void FixedUpdate()
    {
        // Update the player's movement
        player.velocity = moveVelocity;
        UpdatePlayerMovement();
    }

    void UpdatePlayerMovement()
    {
        // ** -- MOVEMENT -- **        
        // Grab the input of the player, leaving the Y value as 0f
        movement.Set(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        // Normalize that captured movement, calculating speed and time.
        movement = movement.normalized * movementSpeed * Time.deltaTime;

        player.MovePosition(transform.position + (transform.forward * movement.z) + (transform.right * movement.x));

        //moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        //// Multiply that value by our speed
        //moveVelocity = moveInput * movementSpeed;
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg * -1;
    }

    //void UpdatePlayerRotation()
    //{
    //    //Get the Screen positions of the object
    //    Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

    //    //Get the Screen position of the mouse
    //    Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

    //    //Get the angle between the points
    //    float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

    //    //Ta Daaa
    //    transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

    //}

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
            // Set the player's transform to look at the intersect
            // transform.LookAt(pointToLook);
            Vector3 direction = pointToLook - transform.position;
            Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
            Quaternion calculatedRotation = Quaternion.Lerp(transform.rotation, toRotation, lookSpeed * Time.time);
            calculatedRotation.x = 0;
            calculatedRotation.z = 0;
            transform.rotation = calculatedRotation;
        }
    }

}
