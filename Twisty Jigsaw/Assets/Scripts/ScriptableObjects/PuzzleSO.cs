using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Puzzle", menuName = "ScriptableObjects/Puzzle", order = 1)]
public class PuzzleSO : ScriptableObject
{
    public Puzzle prefab;
}
