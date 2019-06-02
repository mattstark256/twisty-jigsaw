using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// It's a sequence of sequences. Got a better name?


[CreateAssetMenu(fileName = "New Sequence Sequence", menuName = "ScriptableObjects/Sequence Sequence", order = 1)]
public class SequenceSequence : ScriptableObject
{
    [SerializeField]
    private List<PuzzleSequence> sequences;

    public PuzzleSequence GetSequence(int index)
    {
        return sequences[index];
    }


    public bool IsLastSequence(int index)
    {
        return (index == sequences.Count - 1);
    }


    public int GetSequenceCount()
    {
        return sequences.Count;
    }


    // I don't think I'll need this
    //public int GetSequenceIndex(PuzzleSequence sequence)
    //{
    //    return sequences.FindIndex(l => l == sequence);
    //}
}
