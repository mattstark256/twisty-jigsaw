using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPuzzleButton : MonoBehaviour
{
    [SerializeField]
    private Text puzzleTitle;
    [SerializeField]
    private Text puzzleStatus;
    [SerializeField]
    private Button button;

    private GameData gameData;
    private int sequenceIndex;
    private int puzzleIndex;


    public void Initialize(GameData _gameData, int _sequenceIndex, int _puzzleIndex)
    {
        gameData = _gameData;
        sequenceIndex = _sequenceIndex;
        puzzleIndex = _puzzleIndex;

        puzzleTitle.text = "Puzzle " + (puzzleIndex + 1);

        if (sequenceIndex == gameData.GetSaveData().GetSequencesCompleted() &&
            puzzleIndex > gameData.GetSaveData().GetPuzzlesCompleted())
        {
            puzzleStatus.text = "Locked";
            button.interactable = false;
        }
        else
        {
            puzzleStatus.enabled = false;
        }
    }


    public void SelectPuzzle()
    {
        gameData.GetPuzzleManager().LoadPuzzle(sequenceIndex, puzzleIndex);
        gameData.GetMenuManager().CloseMenu();
    }
}
