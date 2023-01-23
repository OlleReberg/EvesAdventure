using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _animator;
    public float baseMoveSpeed = 5f;
    public float boostMoveSpeed = 15f;
    public float boostDuration = 10f;
    public Vector2 rotation;
    public float sensitivity = 10f;
    public float smoothing = 5f;

    private bool isBoosting = false;
    private bool isRunning = false;
    private float boostTimer = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>(); // Get Rigidbody and assign it to variable
        _animator = GetComponent<Animator>(); //Get Animator and assign it to variable
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor within the game window
        Cursor.visible = false; // make the cursor invisible
    }

    private void Update()
    {
        // rotation.x += Input.GetAxis("Mouse X") * sensitivity; // get horizontal mouse input and multiply by sensitivity
         //rotation.y += Input.GetAxis("Mouse Y") * sensitivity; // get vertical mouse input and multiply by sensitivity
        // //TODO: Make rotation based on camera rather than player, alt based on player head movement
         //rotation.y = Mathf.Clamp(rotation.y, -50f, 50f); // limit the rotation on the y-axis
         //transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(-rotation.y, rotation.x, 0), Time.deltaTime * smoothing); // smoothly rotate the player based on the rotation vector
    }

    private void FixedUpdate()
    {
        // Get movement input
        float horInput = Input.GetAxis("Horizontal"); // get horizontal input
        float vertInput = Input.GetAxis("Vertical"); // get vertical input
        float boostInput = Input.GetAxis("RocketBoost"); // get boost input
        Vector3 movement = new Vector3(horInput, boostInput, vertInput);
        
        // Check for boost input
        if (MathF.Abs(_rb.velocity.y) < 0.01f && Input.GetButtonDown("RocketBoost"))
        {
            isBoosting = true; // set the isBoosting variable to true
            boostTimer = 0f; // reset the boost timer
            _animator.SetBool("Boosting", true);
        }
        
        
        // Update boost timer
        if (isBoosting)
        {
            boostTimer += Time.deltaTime; // increment the boost timer
            _rb.AddForce(movement * (isBoosting ? boostMoveSpeed : baseMoveSpeed), ForceMode.Acceleration);
            // End boost if duration has been reached
            if (boostTimer >= boostDuration)
            {
                isBoosting = false; // reset isBoosting to false
                _animator.SetBool("Boosting", false);
                boostTimer = 0f; // reset the boost timer
            }
        }
        

        if (MathF.Abs(vertInput) > 0.01f)
        {
            //Move character
            transform.rotation = Quaternion.LookRotation(new Vector3(-vertInput, 0f, 0f));
            _rb.MovePosition(_rb.position - transform.forward * (baseMoveSpeed * Time.fixedDeltaTime));
            
            //Start run animation
            if (!isRunning)
            {
                isRunning = true;
                _animator.SetBool("Running", true);
            }
            else if (isRunning)
            {
                isRunning = false;
                _animator.SetBool("Running", false);
            }
        }
    }
}
