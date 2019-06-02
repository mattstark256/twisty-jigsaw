using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData : MonoBehaviour
{
    private void Update()
    {
        // For testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            EraseSaveData();
        }
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
}
