using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerObj;
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rotationSpeed;
    
    private Animator _animator;
    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 5f;
    [SerializeField] private float boostMoveSpeed = 15f;
    [SerializeField] private float boostDuration = 10f;


    [SerializeField] private float groundDrag;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    
    private bool isBoosting = false;
    private bool isRunning = false;
    private float boostTimer = 0f;
    private Vector3 moveDirection;
    
    // Get movement input
    float horInput; // get horizontal input
    float vertInput; // get vertical input
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get Rigidbody and assign it to variable
        _animator = GetComponent<Animator>(); //Get Animator and assign it to variable
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor within the game window
        //Cursor.visible = false; // make the cursor invisible
    }
    
    private void Update()
    {
        //Ground check
        isGrounded = Physics.Raycast(transform.position,
            Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        //Handle drag
        if (isGrounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        
        //rotate orientation
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;
        
        Vector3 inputDirection = orientation.forward * vertInput + orientation.right * horInput;
        
        MovePlayer();
        
        if (inputDirection != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward,
                inputDirection.normalized, Time.deltaTime * rotationSpeed);
        MyInput();
        //Boost();

        if (MathF.Abs(vertInput) > 0.01f)
        {
            
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

    private void Boost()
    {
        float boostInput = Input.GetAxis("RocketBoost"); // get boost input
        Vector3 movement = new Vector3(horInput, boostInput, vertInput);
        // Check for boost input
        if (MathF.Abs(rb.velocity.y) < 0.01f && Input.GetButtonDown("RocketBoost"))
        {
            isBoosting = true; // set the isBoosting variable to true
            boostTimer = 0f; // reset the boost timer
            _animator.SetBool("Boosting", true);
        }

        // Update boost timer
        if (isBoosting)
        {
            boostTimer += Time.deltaTime; // increment the boost timer
            rb.AddForce(movement * (isBoosting ? boostMoveSpeed : baseMoveSpeed), ForceMode.Acceleration);
            // End boost if duration has been reached
            if (boostTimer >= boostDuration)
            {
                isBoosting = false; // reset isBoosting to false
                _animator.SetBool("Boosting", false);
                boostTimer = 0f; // reset the boost timer
            }
        }
    }

    private void MyInput()
    {
        horInput = Input.GetAxisRaw("Horizontal"); // get horizontal input
        vertInput = Input.GetAxisRaw("Vertical"); // get vertical input
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * vertInput + orientation.right * horInput;
        rb.AddForce(moveDirection.normalized * baseMoveSpeed, ForceMode.Force);
    }
}
