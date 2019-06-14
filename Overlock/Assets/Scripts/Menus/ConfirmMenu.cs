using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmMenu : MenuScreen
{
    public void Cancel()
    {
        gameData.GetMenuManager().OpenMainMenu();
    }

    public void Confirm()
    {
        gameData.GetSaveData().EraseSaveData();
        gameData.GetPuzzleManager().LoadPuzzle(0, 0);
        gameData.GetMenuManager().CloseMenu();
    }
}
