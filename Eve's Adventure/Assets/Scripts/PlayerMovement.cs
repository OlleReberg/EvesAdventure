using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody _rb;
    public float baseMoveSpeed = 10f;
    public float boostMoveSpeed = 15f;
    public float boostDuration = 10f;
    public Vector2 rotation;
    public float sensitivity = 10f;
    
    private bool isBoosting = false;
    private float boostTimer = 0f;
    
    

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check for boost input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBoosting = true;
            boostTimer = 0f;
        }

        rotation.x += Input.GetAxis("Mouse X") * sensitivity;
        rotation.y += Input.GetAxis("Mouse Y") * sensitivity;
        transform.localRotation = Quaternion.Euler(-rotation.y, rotation.x, 0);

        // Update boost timer
        if (isBoosting)
        {
            boostTimer += Time.deltaTime;

            // End boost if duration has been reached
            if (boostTimer >= boostDuration)
            {
                isBoosting = false;
                boostTimer = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        // Get movement input
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        float boostInput = Input.GetAxis("RocketBoost");

        // Calculate movement
        Vector3 movement = new Vector3(horInput, boostInput, vertInput);

        // Apply movement to Rigidbody
        _rb.AddForce(movement * (isBoosting ? boostMoveSpeed : baseMoveSpeed), ForceMode.Acceleration);
    }
}
