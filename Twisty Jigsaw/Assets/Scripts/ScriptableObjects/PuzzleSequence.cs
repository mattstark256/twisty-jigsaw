using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Puzzle Sequence", menuName = "ScriptableObjects/Puzzle Sequence", order = 1)]
public class PuzzleSequence : ScriptableObject
{
    [SerializeField]
    private List<PuzzleSO> puzzles;


    public PuzzleSO GetPuzzle(int index)
    {
        return puzzles[index];
    }


    public bool IsLastPuzzle(int index)
    {
        return (index == puzzles.Count - 1);
    }


    public int GetPuzzleIndex(PuzzleSO puzzleSO)
    {
        return puzzles.FindIndex(l => l == puzzleSO);
    }


    public int GetPuzzleIndex(OverlapPuzzle puzzle)
    {
        return puzzles.FindIndex(p => p.prefab == puzzle);
    }
}
