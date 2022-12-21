using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // The speed at which the player moves
    public float moveSpeed = 10.0f;

    // The sensitivity of the mouse for looking around
    public float mouseSensitivity = 5.0f;

    // The vertical range of the player's rotation
    public float pitchMin = -80.0f;
    public float pitchMax = 80.0f;

    // The camera object
    public Camera camera;

    // The vertical rotation of the player
    private float pitch = 0.0f;

    // The horizontal rotation of the player
    private float yaw = 0.0f;

    // The rigidbody component
    private Rigidbody rigidbody;

    // The force of gravity
    public float gravity = -9.81f;

    // The jump force
    public float jumpForce = 5.0f;

    // The current velocity of the player
    private Vector3 velocity;

    public LayerMask groundLayerMask;


    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the rigidbody component
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Rotate the player based on mouse input
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Clamp the pitch between the min and max values
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Rotate the transform of the player
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        // Move the player based on WASD input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        rigidbody.AddForce(moveDirection * moveSpeed, ForceMode.VelocityChange);

        // Apply gravity to the player's velocity
        velocity.y += gravity * Time.deltaTime;

        // Jump if the player presses the space bar and is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            velocity.y = jumpForce;
        }

        // Move the player based on their velocity
        rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);

        // Update the camera's position and rotation
        camera.transform.position = transform.position;
        camera.transform.rotation = transform.rotation;
    }

    bool IsGrounded()
    {
        // Check if the player is touching the ground
        return Physics.Raycast(transform.position, -Vector3.up, 0.1f, groundLayerMask);
    }
}