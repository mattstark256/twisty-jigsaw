using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MenuScreen
{
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject continueButton;
    [SerializeField]
    private GameObject selectPuzzleButton;
    [SerializeField]
    private GameObject newGameButton;
    [SerializeField]
    private GameObject settingsButton;


    public override void Initialize(GameData _gameData)
    {
        base.Initialize(_gameData);

        startButton.SetActive(false);
        continueButton.SetActive(false);
        selectPuzzleButton.SetActive(false);
        newGameButton.SetActive(false);

        if (!gameData.GetPuzzleManager().InteractionsAvailable() &&
            !gameData.GetSaveData().SaveDataExists())
        { startButton.SetActive(true); }

        if (gameData.GetPuzzleManager().InteractionsAvailable() ||
            gameData.GetSaveData().SaveDataExists() &&
            gameData.GetSaveData().GetSequencesCompleted() < gameData.GetSequenceSequence().GetSequenceCount())
        { continueButton.SetActive(true); }

        if (gameData.GetSaveData().SaveDataExists())
        { selectPuzzleButton.SetActive(true); }

        if (gameData.GetSaveData().SaveDataExists())
        { newGameButton.SetActive(true); }
    }


    public void StartGame()
    {
        gameData.GetPuzzleManager().LoadPuzzle(0, 0);
        gameData.GetMenuManager().CloseMenu();
    }


    public void Continue()
    {
        if (!gameData.GetPuzzleManager().InteractionsAvailable())
        {
            gameData.GetPuzzleManager().LoadPuzzle(gameData.GetSaveData().GetSequencesCompleted(), gameData.GetSaveData().GetPuzzlesCompleted());
        }
        gameData.GetMenuManager().CloseMenu();
    }


    public void SelectPuzzle()
    {
        gameData.GetMenuManager().OpenSequenceSelect();
    }


    public void StartNewGame()
    {
        gameData.GetSaveData().EraseSaveData();
        gameData.GetPuzzleManager().LoadPuzzle(0, 0);
        gameData.GetMenuManager().CloseMenu();
    }
}
