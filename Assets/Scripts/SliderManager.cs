using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SliderManager : MonoBehaviour
{
    public static SliderManager Instance;

    [Header ("SLIDERS")]
    public UnityEngine.UI.Slider volumeSlider;
    public UnityEngine.UI.Slider sensitivitySlider;
    public UnityEngine.UI.Slider brightnessSlider;

    [Header ("SENSITIVITY")]
    public float sensitivityValue = 50;  //Holds the slider value so that FPSController can use it.

    [Header("BRIGHTNESS")]
    Light sceneLight;
    public float brightnessValue = 0.5f;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the sliders
        //volumeSlider = GameObject.Find("VolumeSlider").GetComponent<UnityEngine.UI.Slider>();
        //sensitivitySlider = GameObject.Find("SensitivitySlider").GetComponent<UnityEngine.UI.Slider>();
        //brightnessSlider = GameObject.Find("BrightnessSlider").GetComponent<UnityEngine.UI.Slider>();
        //LoadVolume();
        //LoadSensitivity();
        //LoadBrightness();

        //Set scene light
        //sceneLight = Menu.Instance.sceneLight;

        //
        // VOLUME INPUT
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.6f);
            LoadVolume();
        }
        else
        {
            LoadVolume();
        }

        //
        // SENSITIVITY INPUT
        if (PlayerPrefs.HasKey("sensitivityValue"))
        {
            PlayerPrefs.SetFloat("sensitivityValue", 100.0f);
            LoadSensitivity();
        }
        else
        {
            LoadSensitivity();
        }

        //
        //BRIGHTNESS INPUT
        if (PlayerPrefs.HasKey("brightnessValue"))
        {
            PlayerPrefs.SetFloat("brightnessValue", 0.5f);
            LoadBrightness();
        }
        else
        {
            LoadBrightness();
        }
    }

    //
    // VOLUME
    public void AdjustVolume()
    {
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
        //PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");  //Make a new float in PlayerPrefs called musicVolume
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);  //Save the slider value in musicVolume
    }

    //
    // SENSITIVITY
    public void AdjustSensitivity()
    {
        sensitivityValue = sensitivitySlider.value;  //Set the value to the slider value
        //FPSController.Instance.lookSpeed = sensitivityValue;
        SaveSensitivity();
    }

    public void LoadSensitivity()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivityValue");
    }

    private void SaveSensitivity()
    {
        PlayerPrefs.SetFloat("sensitivityValue", sensitivitySlider.value);
    }

    //
    // BRIGHTNESS
    //Set the brightness value in Menu to be called when the scene loads
    public void SetSceneBrightness()
    {
        Menu.Instance.brightnessValue = brightnessValue;
    }

    public void AdjustBrightness()
    {
        brightnessValue = brightnessSlider.value;
        SaveBrightness();
    }

    public void LoadBrightness()
    {
        brightnessSlider.value = PlayerPrefs.GetFloat("brightnessValue");
    }

    private void SaveBrightness()
    {
        PlayerPrefs.SetFloat("brightnessValue", brightnessSlider.value);
    }
}
