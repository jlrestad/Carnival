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
    public float slideTime = 2;
    //public float slideTimeCounter;

    [Header("HEIGHT")]
    public float slideHeight;
    public float crouchHeight;
    float originalHeight;

    [Header("CAMERA")]
    public Camera playerCamera;
    public float lookXLimit = 45.0f;
    

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero; //set to 0
    float rotationX = 0.0f;
    GameObject weapon;

    [Header("BOOLS")]
    //[HideInInspector]
    public bool canMove = true;
    public bool weaponIsEquipped;

    bool run, jump, slide, crouch;
    public bool isGrounded, isJumping, isRunning, isSliding, isCrouching, isUp;
    public bool slidingAllowed = true;

    public void Start()
    {
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();

        originalHeight = characterController.height;

        canMove = true;
        isUp = true;
    }

    public void Update()
    {
        //weaponIsEquipped = weapon.GetComponent<WeaponEquip>().isEquipped;

        //Controls
        run = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetButton("Jump");
        slide = Input.GetKey(KeyCode.R);
        crouch = Input.GetKeyDown(KeyCode.LeftControl);

        //States
        isGrounded = characterController.isGrounded;
        isJumping = jump && characterController.isGrounded;
        isRunning = run && !isJumping && characterController.isGrounded;
        isSliding = slide && isRunning;
        isCrouching = crouch && !isUp;

        // Sliding
        if (slidingAllowed && isSliding)
        {
            // slide once
            Slide();
            slidingAllowed = false;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            // allow for sliding again
            slidingAllowed = true;
        }
     

        // Crouching
        if (crouch && isUp)
        {
            Crouch();
        }
        else if (isCrouching)
        {
            GoUp();
        }
    }
    void FixedUpdate()
    {
        // Player is grounded -- recalculate the move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        //If canMove is true and isRunning is true, then speed is runSpeed, else speed is walkSpeed.
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedZ = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        if (isSliding)
        {
            curSpeedX = slideSpeed * Input.GetAxis("Vertical");
            curSpeedZ = slideSpeed * Input.GetAxis("Horizontal");
        }

        float moveDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedZ);

        // Jumping
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

        // Move the player
        characterController.Move(moveDirection * Time.deltaTime);

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
        isUp = false;
        
        characterController.height = slideHeight;
        characterController.Move(moveDirection * Time.deltaTime/* * slideSpeed*/);

        playerCamera.transform.position = new Vector3(transform.position.x, characterController.height, transform.position.z);
        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, -10.0f);

        StartCoroutine(DoneSliding());
    }

    private void GoUp()
    {
        isUp = true;

        characterController.height = originalHeight;

        playerCamera.transform.position = new Vector3(transform.position.x, characterController.height, transform.position.z);
    }

    IEnumerator DoneSliding()
    {
        yield return new WaitForSeconds(slideTime);

        isUp = true;

        characterController.height = originalHeight;

        //float rotX = transform.rotation.x;
        //float rotY = transform.rotation.y;

        playerCamera.transform.position = new Vector3(transform.position.x, characterController.height, transform.position.z);
        //transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    private void Crouch()
    {
        isUp = false;

        characterController.height = crouchHeight;

        playerCamera.transform.position = new Vector3(transform.position.x, characterController.height, transform.position.z);
    }
}
