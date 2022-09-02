using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayCarnivalMusic : MonoBehaviour
{
    public Menu menu;
    [SerializeField] bool fadingIn;


    private void Start()
    {
        menu = GameObject.FindObjectOfType<Menu>();
        fadingIn = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered Trigger Zone");
            //menu.PlaySceneMusic();
            if (fadingIn)
            {
                StartCoroutine(FadeIn(menu.sceneMusic, 0.1f, 1));
            }

            //* Fade out could be triggered by play and then the music would stay off inside the carnival. So won't use it for now.
        }
    }

    //Fade in the carnival music
    public IEnumerator FadeIn(AudioSource audioSource, float speed, float maxVolume)
    {
        //fadingIn = true;

        audioSource.enabled = true;
        audioSource.volume = 0; //Turn down the audio volume.
        float audioVolume = audioSource.volume;

        while (audioSource.volume < maxVolume)
        {
            audioVolume += speed;
            audioSource.volume = audioVolume;
            yield return new WaitForSeconds(0.3f);
        }

        if (audioSource.volume == maxVolume)
        {
            fadingIn = false;
        }
    }

    public IEnumerator FadeOut(AudioSource audioSource, float speed)
    {
        float audioVolume = audioSource.volume;

        while (audioSource.volume >= speed)
        {
            audioVolume -= speed;
            audioSource.volume = audioVolume;
            yield return new WaitForSeconds(0.3f);
        }

        if (audioSource.volume == speed)
        {
            fadingIn = true;
        }
    }
}
