using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MenuScreen
{
    [SerializeField]
    private Toggle FPStoggle;

    private void Start()
    {
        FPStoggle.isOn = gameData.GetFramerateCounter().GetVisible();
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
        gameData.GetFramerateCounter().SetVisible(FPStoggle.isOn);
    }
}
