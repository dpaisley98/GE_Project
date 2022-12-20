using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 10.0f;

    void Update()
    {
        // Use the Input Manager to get the mouse input for rotation
        float mouseInput = Input.GetAxis("Mouse X");

        // Use the mouse input to rotate the camera
        transform.rotation *= Quaternion.Euler(0, mouseInput * rotationSpeed, 0);

        // Shoot a raycast out of the camera
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log("Hit object: " + hit.transform.name);
        }
    }
}




