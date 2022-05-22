using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

//...............................................................................//
//...............................................................................//
//... This script is responsible for any effects done to the Title Screen UI. ...//
//...............................................................................//
//...............................................................................//

public class TitleScreenUI : MonoBehaviour
{
    GameObject titleF;
    [SerializeField] CanvasGroup letterF;  //Reference to the text that wil be fading in and out.
    [SerializeField] Image imageF;

    bool playFade;  //Allows the coroutine to loop when called from Awake.

    void Awake()
    {
        titleF = GameObject.FindGameObjectWithTag("LetterF");
        letterF = titleF.GetComponent<CanvasGroup>();
        imageF = titleF.GetComponent<Image>();
        playFade = true;

        StartCoroutine(LetterFade());
    }

    private void Update()
    {
        
    }

    //Randomizes the fade length and alpha strength
    IEnumerator LetterFade()
    {
        while (playFade)
        {
            float randAlpha = Random.Range(0.5f, 1.0f);
            float randWait = Random.Range(0.5f, 1f);

            //yield return new WaitForSeconds(0.1f);

            //Slowly fade in and out rather than jump to a new alpha number.
            if (randAlpha < letterF.alpha)
            {
                while (randAlpha < letterF.alpha)
                {
                    letterF.alpha -= 0.003f;
                    yield return null;
                }
            }
            if (randAlpha > letterF.alpha)
            {
                while (randAlpha > letterF.alpha)
                {
                    letterF.alpha += 0.003f;
                    yield return null;

                }
            }

            if (letterF.alpha >= 0.8f)
            {
                Color32 redColor32 = new Color(255, 0, 0, 255);
                imageF.color = redColor32;
                yield return new WaitForSeconds(randWait);
            }
            else
            {
                Color32 blackColor32 = new Color(0, 0, 0, 255);
                imageF.color = blackColor32;
            }


        }
        yield return null;
    }

}

