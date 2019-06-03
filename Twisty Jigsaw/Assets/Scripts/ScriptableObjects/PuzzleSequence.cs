using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Puzzle Sequence", menuName = "ScriptableObjects/Puzzle Sequence", order = 1)]
public class PuzzleSequence : ScriptableObject
{
    [SerializeField]
    protected ColorPalette colorPalette;
    [SerializeField]
    private List<Puzzle> puzzles;


    public Puzzle GetPuzzle(int index)
    {
        return puzzles[index];
    }


    public bool IsLastPuzzle(int index)
    {
        return (index == puzzles.Count - 1);
    }


    public int GetPuzzleCount()
    {
        return puzzles.Count;
    }


    // The foreground and background colors flip between consecutive puzzles
    public Color GetColor(int colorIndex, int puzzleIndex)
    {
        return (puzzleIndex % 2 == 0) ?
            (colorIndex == 0) ?
                colorPalette.foregroundColor :
                colorPalette.backgroundColor :
            (colorIndex == 0) ?
                colorPalette.backgroundColor :
                colorPalette.foregroundColor;
    }


    // I don't think I'll need this
    //public int GetPuzzleIndex(PuzzleSO puzzleSO)
    //{
    //    return puzzles.FindIndex(l => l == puzzleSO);
    //}

    // I don't think I'll need this
    //public int GetPuzzleIndex(OverlapPuzzle puzzle)
    //{
    //    return puzzles.FindIndex(p => p.prefab == puzzle);
    //}
}
