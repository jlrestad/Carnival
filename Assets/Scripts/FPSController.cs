using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Effects;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    CharacterController characterController;

    public WeaponEquip weaponEquip;

    [Header("SPEEDS")]
    public float walkSpeed = 7.0f;
    public float runSpeed = 10.0f;
    public float jumpSpeed = 8.0f;
    public float lookSpeed = 2.5f;
    public float gravity = 20.0f;
    public float slideSpeed = 2.0f;
    public float pushPower = 10.0f;

    [Header("TIMING")]
    public float slideTime = 2;
    //public float slideTimeCounter;

    [Header("HEIGHT")]
    [SerializeField] float slideHeight;
    [SerializeField] float crouchHeight;
    [SerializeField] float slideAngle = 20f;
    float originalHeight;
    float heightPos;

    [Header("CAMERA")]
    public Camera playerCamera;
    float originalCamHeight;
    public float lookXLimit = 45.0f;
    
    Vector3 moveDirection = Vector3.zero; //set to 0
    float rotationX = 0.0f;

    [Header("BOOLS")]
    //[HideInInspector]
    public bool canMove = true;

    [HideInInspector] public bool run, jump, slide, crouch, useFlashlight, dontUseFlashlight;
    [HideInInspector] public bool slidingAllowed = true;
    public bool isGrounded, isJumping, isRunning, isSliding, isCrouching, isUp, flashlightOn, canThrow = true;

    [Header("BOSS COMPONENTS")]
    public GameObject tent;
    public GameObject boss;
    public int cardCount; //Used to verify that 3 cards have been won before boss can be fought.

    [Space(15)]
    [HideInInspector] public GameObject flashlightHold;

    Transform capsule;

    public void OnValidate()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        weaponEquip = GetComponent<WeaponEquip>(); 
        characterController = GetComponent<CharacterController>();
        capsule = GetComponentInChildren<Transform>();

        //Get and set the original settings of player
        originalHeight = characterController.height;
        originalCamHeight = playerCamera.transform.position.y;
        heightPos = characterController.transform.position.y;

        // LOCK CURSOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        canMove = true;
        isUp = true;
    }

    void Update()
    {
        //Activate the boss
        if (cardCount == 3)
        {
            tent.SetActive(false);
            boss.SetActive(true);
        }

        //
        //Controls
        run = Input.GetAxis("LtTrigger") > 0 || Input.GetButton("Run");
        jump = Input.GetButtonDown("Jump");
        slide = Input.GetButtonDown("Slide");
        crouch = Input.GetButtonDown("Crouch");
        useFlashlight = Input.GetAxis("Flashlight1") > 0 && !flashlightOn || Input.GetButtonDown("Flashlight2") && !flashlightOn;
        dontUseFlashlight = Input.GetAxis("Flashlight1") > 0 && flashlightOn || Input.GetButtonDown("Flashlight2") && flashlightOn;
 
        //
        //States
        isGrounded = characterController.isGrounded;
        isJumping = jump && characterController.isGrounded;
        isRunning = run && !isJumping && characterController.isGrounded;
        isSliding = slide && isRunning;
        isCrouching = crouch && isUp;

        //
        //Flashlight
        if (useFlashlight)
        {
            flashlightHold.SetActive(true);
            flashlightOn = true;
        }

        //If holding the flashlight and button is pressed again put the flashlight away
        if (dontUseFlashlight)
        {
            flashlightHold.SetActive(false);
            flashlightOn = false;
        }

        // Player is grounded -- recalculate the move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        // If canMove is true and isRunning is true, then speed is runSpeed, else speed is walkSpeed.
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxisRaw("Vertical") : 0;
        float curSpeedZ = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxisRaw("Horizontal") : 0;

        // Change the speed if player is sliding.
        if (isSliding)
        {
            curSpeedX = slideSpeed * Input.GetAxisRaw("Vertical");
            curSpeedZ = slideSpeed * Input.GetAxisRaw("Horizontal");
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
        if (characterController.enabled == true)
        {
            characterController.Move(moveDirection * Time.deltaTime);
        }

        // Player and camera rotation
        if (canMove)
        {
            //rotate at the lookSpeed
            rotationX += -Input.GetAxisRaw("Mouse Y") * lookSpeed;
            rotationX += Input.GetAxisRaw("LookUpAndDown") * lookSpeed;
            //stop rotate at the min and max degree limit
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            //have camera follow the rotation
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxisRaw("Mouse X") * lookSpeed, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxisRaw("LookLeftAndRight") * lookSpeed, 0);
        }

        // Sliding
        if (slidingAllowed && isSliding)
        {
            StartCoroutine(Slide());
        }

        if (Input.GetButtonUp("Slide") && !slidingAllowed)
        {
            StartCoroutine(Slide());
        }
        if (Input.GetButtonUp("Slide") && !slidingAllowed)
        {
            slidingAllowed = true;
        }

        // Crouching
        if (isCrouching)
        {
            Crouch();
        }
        else if (crouch && !isUp)
        {
            StandUp();
        }
    }

    // Limit the amount of time until slide is allowed
    IEnumerator CanSlide() 
    {
        yield return new WaitForSeconds(2f);        
        slidingAllowed = true;
    }

    //Used to control Joystick trigger from the ability to spam fire.
    void GetTriggerUse()
    {
        if (Input.GetAxis("RtTrigger") > 0)
        {
            canThrow = false;
        }
        else
        {
            canThrow = true;
        }
    }

    // PUSHES RIDIDBODIES THAT PLAYER RUNS INTO
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // No rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        //Keep from pushing objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        //Stop moving if running into a static object
        // (for sliding) I think need to use colliders -- if collider enter belongs to static.... will try it later
        if (hit.gameObject.isStatic == true)
        {
            characterController.Move(moveDirection * Time.deltaTime * 0);
        }

        // Calculate push direction from move direction,
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply the push
        body.velocity = pushDir * pushPower;
    }   

    private void StandUp()
    {
        isUp = true;

        characterController.height = originalHeight;
    }

    private void Crouch()
    {
        isUp = false;

        characterController.height = crouchHeight;
    }

    IEnumerator Slide()
    {
        isUp = false;

        yield return new WaitForFixedUpdate();

        characterController.height = slideHeight; //being character height down to immitate sliding
        characterController.Move(moveDirection * Time.deltaTime * slideSpeed); //move in the direction player slid at slide speed
        //capsule = transform; //get the transform of FPSPlayer object

        Transform faceForward = capsule.transform;
        //Tilt to the side during slide
        capsule.transform.Rotate(0f, 0f, slideAngle, Space.Self); //works...

        yield return new WaitForSeconds(slideTime);
      
        isUp = true;

        //Return to upright position after slide
        //capsule.transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up); //not working....

        //capsule.transform.Rotate(0f, 0f, transform.rotation.z - 10f, Space.Self); //not working well....
        capsule.transform.Rotate(0f, 0f, -slideAngle, Space.Self); 

        characterController.height = originalHeight; //set character height back to normal
    }
}
