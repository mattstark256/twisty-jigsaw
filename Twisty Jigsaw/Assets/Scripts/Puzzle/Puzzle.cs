using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Puzzles are interactive and can be solved. They also have a color palette.


public class Puzzle : MonoBehaviour
{
    [SerializeField]
    private string tutorialText;
    public string GetTutorialText() { return tutorialText; }


    protected bool solved = false;
    public bool IsSolved() { return solved; }


    public virtual void Initialize(Color color) { }


    public virtual void StartInteraction(Vector3 position) { }

    public virtual void ContinueInteraction(Vector3 position) { }

    public virtual void EndInteraction() { }
}
