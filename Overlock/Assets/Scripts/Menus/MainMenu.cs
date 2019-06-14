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


    public override void Initialize(GameData _gameData)
    {
        base.Initialize(_gameData);

        if (gameData.GetPuzzleManager().GetPuzzleState() != PuzzleState.notLoaded ||
            gameData.GetSaveData().SaveDataExists())
        { startButton.SetActive(false); }
        
        if (gameData.GetPuzzleManager().GetPuzzleState() == PuzzleState.notLoaded &&
            (!gameData.GetSaveData().SaveDataExists() ||
            gameData.GetSaveData().GetSequencesCompleted() == gameData.GetSequenceSequence().GetSequenceCount()))
        { continueButton.SetActive(false); }

        if (!gameData.GetSaveData().SaveDataExists())
        { selectPuzzleButton.SetActive(false); }

        if (!gameData.GetSaveData().SaveDataExists())
        { newGameButton.SetActive(false); }
    }


    public void StartGame()
    {
        gameData.GetPuzzleManager().LoadPuzzle(0, 0);
        gameData.GetMenuManager().CloseMenu();
    }


    public void Continue()
    {
        if (gameData.GetPuzzleManager().GetPuzzleState() == PuzzleState.notLoaded)
        {
            // Load the most recent puzzle
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
        gameData.GetMenuManager().OpenConfirmMenu();
    }


    public void Settings()
    {
        gameData.GetMenuManager().OpenSettingsMenu();
    }


    public void Quit()
    {
        Application.Quit();
    }
}
