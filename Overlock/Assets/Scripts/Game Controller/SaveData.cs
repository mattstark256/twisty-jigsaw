using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameData))]
public class SaveData : MonoBehaviour
{
    private GameData gameData;


    private void Awake()
    {
        gameData = GetComponent<GameData>();
    }


    public void SequenceCompleted(int sequenceIndex)
    {
        if (sequenceIndex + 1 > PlayerPrefs.GetInt("Sequences completed"))
        {
            PlayerPrefs.SetInt("Sequences completed", sequenceIndex + 1);
            PlayerPrefs.SetInt("Puzzles completed", 0);
        }
    }


    public void PuzzleCompleted(int puzzleIndex)
    {
        if (puzzleIndex + 1 > PlayerPrefs.GetInt("Puzzles completed"))
        {
            PlayerPrefs.SetInt("Puzzles completed", puzzleIndex + 1);
        }
    }


    public bool SaveDataExists()
    {
        return
            PlayerPrefs.GetInt("Sequences completed") != 0 ||
            PlayerPrefs.GetInt("Puzzles completed") != 0;
    }


    public void EraseSaveData()
    {
        PlayerPrefs.DeleteKey("Sequences completed");
        PlayerPrefs.DeleteKey("Puzzles completed");
    }


    public int GetSequencesCompleted()
    {
        return PlayerPrefs.GetInt("Sequences completed");
    }


    public int GetPuzzlesCompleted()
    {
        return PlayerPrefs.GetInt("Puzzles completed");
    }


    public void UnlockAll()
    {
        PlayerPrefs.SetInt("Sequences completed", gameData.GetSequenceSequence().GetSequenceCount());
    }
}
