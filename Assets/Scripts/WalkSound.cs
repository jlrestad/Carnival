using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    CharacterController controller;
    public AudioSource walk;
    public AudioSource run;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (controller.isGrounded == true && controller.velocity.magnitude > 2f && run.isPlaying == false && walk.isPlaying == false)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("RUNNING");

                run.volume = Random.Range(0.5f, .8f);
                run.pitch = Random.Range(.2f, .8f);
                run.Play();
            }
            Debug.Log("WALKING");

            walk.volume = Random.Range(0.2f, 0.5f);
            walk.pitch = Random.Range(0.8f, 1.2f);
            walk.Play();
        }
    }

}
