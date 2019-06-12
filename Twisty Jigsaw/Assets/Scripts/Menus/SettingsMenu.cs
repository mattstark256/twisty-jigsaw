using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MenuScreen
{
    [SerializeField]
    private Toggle fpsToggle;
    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private GameObject devTools;


    private void Start()
    {
        fpsToggle.isOn = gameData.GetFramerateCounter().GetEnabled();

        volumeSlider.value = gameData.GetVolumeController().GetEffectsVolume();
    }


    public void GoBack()
    {
        gameData.GetMenuManager().OpenMainMenu();
    }


    public void UnlockAll()
    {
        gameData.GetSaveData().UnlockAll();
    }


    public void EraseSaveData()
    {
        gameData.GetSaveData().EraseSaveData();
    }


    public void ToggleFramerateCounter()
    {
        gameData.GetFramerateCounter().SetEnabled(fpsToggle.isOn);
    }


    public void UpdateEffectsVolume()
    {
        gameData.GetVolumeController().SetEffectsVolume(volumeSlider.value);
    }


    public void ToggleDevTools()
    {
        devTools.SetActive(!devTools.activeSelf);
    }
}
