using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    GameManager GM;
    public AudioMixer audioMixer;
    public AudioSource introAudio;
    public string exposedParam;
    public float delayTime = 3f;

    private void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    public void HandleOnStateChange()
    {
        Debug.Log("OnStateChange!");
    }

    //public void OnGUI()
    //{
    //    // Menu layout
    //    GUI.BeginGroup(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 800));
    //    GUI.Box(new Rect(0, 0, 100, 200),"Menu");

    //    if (GUI.Button(new Rect(10, 40, 80, 30), "Start"))
    //    {
    //        StartGame();
    //    }
    //    if (GUI.Button(new Rect(10, 160, 80, 30), "Quit"))
    //    {
    //        Quit();
    //    }
    //    GUI.EndGroup();
    //}

    public void StartGame()
    {
        // Start game scene
        GM.SetGameState(GameState.LEVEL_ONE);
        Invoke("LoadLevel", delayTime);
        Debug.Log(GM.gameState);
    }

    public void Quit()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level01", LoadSceneMode.Single);
        introAudio.volume = 1;
    }

    public void AudioFade()
    {
        //float elapsedTime = 0;
        //float currentVolume = introAudio.volume;

        //while (elapsedTime < delayTime)
        //{
        //    elapsedTime += Time.deltaTime;
        //    introAudio.volume = Mathf.Lerp(currentVolume, 0, elapsedTime / delayTime);
        //}

        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, exposedParam, 3, 0));

    }
}
