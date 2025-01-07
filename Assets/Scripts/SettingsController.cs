using System.Collections;
using System.Collections.Generic;
using TMPro;
// using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsManager : MonoBehaviour
{

    public GameObject settings;
    public UIDocument settingsUI;
    public VisualElement settingsRoot;
    public GameObject mainMenu;

    public GameObject pauseMenu;
    public int volumePercent;
    public int brightnessPercent;
    public int torchRadiusPercent;
    public int sensitivityPercent;

    public string previousMenu;

    // Start is called before the first frame update
    void Start()
    {
        sensitivityPercent = 50;
        volumePercent = 50;
        brightnessPercent = 0;
        torchRadiusPercent = 50;

        settings = GameObject.Find("Settings");
        mainMenu = GameObject.Find("MainMenu");
        pauseMenu = GameObject.Find("Inventory");

        settingsUI = settings.GetComponent<UIDocument>();
        settingsRoot = settingsUI.rootVisualElement;
        settingsRoot.style.display = DisplayStyle.None;


    }

    void Awake()
    {
        settings = GameObject.Find("Settings");
        mainMenu = GameObject.Find("MainMenu");

        settingsUI = settings.GetComponent<UIDocument>();
        settingsRoot = settingsUI.rootVisualElement;
        settingsRoot.style.display = DisplayStyle.None;

        //Volume
        Slider volumeSlider = settingsRoot.Q<Slider>("VolumeSlider");
        volumeSlider.RegisterValueChangedCallback(evt =>
        {
            volumePercent = (int)evt.newValue;
            volumeSlider.label = "Volume: " + volumePercent + "%";
        }
        );
        //Brightness
        Slider brightnessSlider = settingsRoot.Q<Slider>("BrightnessSlider");
        brightnessSlider.RegisterValueChangedCallback(evt =>
        {
            brightnessPercent = (int)evt.newValue;
            brightnessSlider.label = "Brightness: " + brightnessPercent  + "%";
        }
        );
        //Torch Radius
        Slider torchRadiusSlider = settingsRoot.Q<Slider>("TorchRadiusSlider");
        torchRadiusSlider.RegisterValueChangedCallback(evt =>
        {
            torchRadiusPercent = (int)evt.newValue;
            torchRadiusSlider.label = "Torch Radius: " + torchRadiusPercent + "%";
        }
        );
        //Sensitivity
        Slider sensitivitySlider = settingsRoot.Q<Slider>("SensitivitySlider");
        sensitivitySlider.RegisterValueChangedCallback(evt =>
        {
            sensitivityPercent = (int)evt.newValue;
            sensitivitySlider.label = "Sensitivity: " + sensitivityPercent + "%";
        }
        );
        //Close Menu
        Button closeButton = settingsRoot.Q<Button>("CloseButton");
        closeButton.RegisterCallback<ClickEvent>(evt => {
            Debug.Log("Close Button Clicked");
            settingsRoot.style.display = DisplayStyle.None;
            if (previousMenu == "MainMenu")
            {
                settingsRoot.style.display = DisplayStyle.None;
                mainMenu.GetComponent<UIDocument>().rootVisualElement.style.display = DisplayStyle.Flex;
            }else if (previousMenu == "Inventory")
            {
                settingsRoot.style.display = DisplayStyle.None;
                pauseMenu.GetComponent<InventoryController>().guiNeedsUpdating = true;
                pauseMenu.SetActive(true);
            }
        });
    }


    // Update is called once per frame
    void OnGUI()
    {
        if(settingsUI.rootVisualElement.style.display == DisplayStyle.None)
        {
            return;
        }
        settingsUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void ApplySettings()
    {
        Debug.Log("Volume: " + volumePercent);
        Debug.Log("Brightness: " + brightnessPercent);
        Debug.Log("Torch Radius: " + torchRadiusPercent);
        Debug.Log("Sensitivity: " + sensitivityPercent);
    }
}
