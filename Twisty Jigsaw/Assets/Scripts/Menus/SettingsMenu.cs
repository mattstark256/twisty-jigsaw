using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MenuScreen
{
    public void GoBack()
    {
        gameData.GetMenuManager().OpenMainMenu();
    }
}
