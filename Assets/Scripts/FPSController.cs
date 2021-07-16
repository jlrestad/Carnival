using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    [Header("SPEEDS")]
    public float walkSpeed = 7.0f;
    public float runSpeed = 10.0f;
    public float jumpSpeed = 8.0f;
    public float lookSpeed = 2.5f;
    public float gravity = 20.0f;
    public float slideSpeed = 2.0f;

    [Header("TIMING")]
    public float slideTime = 2.0f;
    public float slideTimeCounter;

    [Header("SLIDING")]
    public float reducedHeight;
    float originalHeight;

    [Header("CAMERA")]
    public Camera playerCamera;
    public float lookXLimit = 45.0f;
    Vector3 originalCamPos;
    

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero; //set to 0
    float rotationX = 0.0f;

    //[HideInInspector]
    public bool canMove = true;

    bool run, jump, slide;
    public bool isGrounded, isJumping, isRunning, isSliding, isUp;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        originalHeight = characterController.height;
        originalCamPos = playerCamera.transform.position;
        slideTimeCounter = slideTime;

        canMove = true;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        //Controls
        run = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetButton("Jump");
        slide = Input.GetKey(KeyCode.R);

        //States
        isGrounded = characterController.isGrounded;
        isJumping = jump && characterController.isGrounded;
        isRunning = run && !isJumping && characterController.isGrounded;
        isSliding = slide && isRunning;

        // Player is grounded -- recalculate the move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        //If canMove is true and isRunning is true, then speed is runSpeed, else speed is walkSpeed.
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedZ = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float moveDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedZ);

        if (isJumping)
        {
            moveDirection.y = jumpSpeed;
            isJumping = true;
        }
        else
        {
            moveDirection.y = moveDirectionY;
            isJumping = false;
        }

        // Apply gravity for jumping.
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        if (isSliding)
        {
            Slide();
        }
        else if (!isSliding && !isUp)
        {
            GoUp();
        }


        // Player and camera rotation
        if (canMove)
        {
            //rotate at the lookSpeed
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            //stop rotate at the min and max degree limit
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            //have camera follow the rotation
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void Slide()
    {
        Debug.Log("Sliding");

        isUp = false;
        
        while (slideTimeCounter > 0.0f)
        {
            characterController.height = reducedHeight;
            characterController.Move(slideSpeed * Time.deltaTime * moveDirection);

            playerCamera.transform.position = new Vector3(transform.position.x, characterController.height, transform.position.z);
            slideTimeCounter -= 1.0f;
        }

        //GoUp();
        slideTimeCounter = slideTime;


        //if (!isSliding)
        //{
        //    GoUp();
        //}
    }

    private void GoUp()
    {
        Debug.Log("Go up");

        isUp = true;

        //isSliding = false;
        //slideTimeCounter = slideTime;

        characterController.height = originalHeight;
        //characterController.Move(moveDirection * Time.deltaTime);

        playerCamera.transform.position = new Vector3(transform.position.x, characterController.height, transform.position.z);
    }
}
