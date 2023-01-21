using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody _rb;
    public float baseMoveSpeed = 10f;
    public float boostMoveSpeed = 15f;
    public float boostDuration = 10f;
    public Vector2 rotation;
    public float sensitivity = 10f;
    public float smoothing = 5f;

    private bool isBoosting = false;
    private float boostTimer = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Get the Rigidbody component from the object and assign it to the _rb variable
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor within the game window
        Cursor.visible = false; // make the cursor invisible
    }

    private void Update()
    {
        // Check for boost input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBoosting = true; // set the isBoosting variable to true
            boostTimer = 0f; // reset the boost timer
        }

        rotation.x += Input.GetAxis("Mouse X") * sensitivity; // get horizontal mouse input and multiply by sensitivity
        rotation.y += Input.GetAxis("Mouse Y") * sensitivity; // get vertical mouse input and multiply by sensitivity
        rotation.y = Mathf.Clamp(rotation.y, -50f, 50f); // limit the rotation on the y-axis
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(-rotation.y, rotation.x, 0), Time.deltaTime * smoothing); // smoothly rotate the player based on the rotation vector

        // Update boost timer
        if (isBoosting)
        {
            boostTimer += Time.deltaTime; // increment the boost timer

            // End boost if duration has been reached
            if (boostTimer >= boostDuration)
            {
                isBoosting = false; // reset isBoosting to false
                boostTimer = 0f; // reset the boost timer
            }
        }
    }

    private void FixedUpdate()
    {
        // Get movement input
        float horInput = Input.GetAxis("Horizontal"); // get horizontal input
        float vertInput = Input.GetAxis("Vertical"); // get vertical input
        float boostInput = Input.GetAxis("RocketBoost"); // get boost input
        
        Vector3 movement = new Vector3(horInput, boostInput, vertInput); // Calculate movement

        // Apply movement to Rigidbody
        _rb.AddForce(movement * (isBoosting ? boostMoveSpeed : baseMoveSpeed), ForceMode.Acceleration);
    }
}
