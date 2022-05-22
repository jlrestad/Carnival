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
    [SerializeField] CanvasGroup letterF;  //Reference to the text that wil be fading in and out.

    bool playFade;  //Allows the coroutine to loop when called from Awake.

    void Awake()
    {
        letterF = GameObject.FindGameObjectWithTag("LetterF").GetComponent<CanvasGroup>();
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
            float randAlpha = Random.Range(0.3f, 1.0f);
            float randWait = Random.Range(0.3f, 0.5f);

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
        }
        yield return null;
    }

}

