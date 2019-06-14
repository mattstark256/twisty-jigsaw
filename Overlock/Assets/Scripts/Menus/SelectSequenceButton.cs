using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSequenceButton : MonoBehaviour
{
    [SerializeField]
    private Text sequenceTitle;
    [SerializeField]
    private Text sequenceStatus;
    [SerializeField]
    private Button button;

    private GameData gameData;
    private int sequenceIndex;

    public void Initialize(GameData _gameData, int _sequenceIndex)
    {
        gameData = _gameData;
        sequenceIndex = _sequenceIndex;

        sequenceTitle.text = "Sequence " + (sequenceIndex + 1);

        int completedSequences = gameData.GetSaveData().GetSequencesCompleted();
        if (sequenceIndex < completedSequences)
        {
            int puzzlesToComplete = gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetPuzzleCount();
            sequenceStatus.text = puzzlesToComplete + "/" + puzzlesToComplete;

        }
        else if (sequenceIndex == completedSequences)
        {
            int puzzlesToComplete = gameData.GetSequenceSequence().GetSequence(sequenceIndex).GetPuzzleCount();
            int completedPuzzles = gameData.GetSaveData().GetPuzzlesCompleted();
            sequenceStatus.text = completedPuzzles + "/" + puzzlesToComplete;
        }
        else
        {
            sequenceStatus.text = "Locked";
            button.interactable = false;
        }
    }


    public void SelectSequence()
    {
        gameData.GetMenuManager().OpenPuzzleSelect(sequenceIndex);
    }
}
