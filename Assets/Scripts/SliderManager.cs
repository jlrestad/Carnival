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
    public float sensitivityValue;  //Holds the slider value so that FPSController can use it.

    [Header("BRIGHTNESS")]
    Light sceneLight;
    public float brightnessValue;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set the sliders
        volumeSlider = GameObject.Find("VolumeSlider").GetComponent<UnityEngine.UI.Slider>();
        sensitivitySlider = GameObject.Find("SensitivitySlider").GetComponent<UnityEngine.UI.Slider>();
        brightnessSlider = GameObject.Find("BrightnessSlider").GetComponent<UnityEngine.UI.Slider>();

        //Set scene light
        //sceneLight = Menu.Instance.sceneLight;

        //
        // VOLUME INPUT
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
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
            PlayerPrefs.SetFloat("sensitivityValue", 50);
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
    }

    private void LoadVolume()
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
        //FPSController.Instance.lookSpeed = mouseSlider.value;       
        sensitivityValue = sensitivitySlider.value;  //Set the value to the slider value
        SaveSensitivity();
    }

    private void LoadSensitivity()
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

    private void LoadBrightness()
    {
        brightnessSlider.value = PlayerPrefs.GetFloat("brightnessValue");
    }

    private void SaveBrightness()
    {
        PlayerPrefs.SetFloat("brightnessValue", brightnessSlider.value);
    }
}
