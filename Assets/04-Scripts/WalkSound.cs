using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    CharacterController controller;
    FPSController player;

    public AudioSource walk;
    public AudioSource run;
    public AudioSource slide;
    public AudioSource breath;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (controller.isGrounded == true && controller.velocity.magnitude > 2f)
        {
            if (Input.GetKey(KeyCode.LeftShift) && run.isPlaying == false)
            {
                walk.Stop();
                PlayRunSound();
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R) && slide.isPlaying == false)
            {
                run.Stop();
                PlaySlideSound();
            }

            if (walk.isPlaying == false)
            {
                PlayWalkSound();
            }
        }
    }

    public void PlayWalkSound()
    {
        //Debug.Log("WALKING");

        walk.volume = Random.Range(0.4f, 0.5f);
        walk.pitch = Random.Range(1.3f, 1.5f);
        walk.Play();
    }

    public void PlayRunSound()
    {
        //Debug.Log("RUNNING");

        run.volume = Random.Range(0.1f, 0.3f);
        run.pitch = Random.Range(1.4f, 1.6f);
        run.Play();

        //breath.PlayDelayed(0.3f);        //breath.PlayDelayed(0.3f);
    }

    public void PlaySlideSound()
    {
        //Debug.Log("SLIDING");

        slide.Play();
    }
}
