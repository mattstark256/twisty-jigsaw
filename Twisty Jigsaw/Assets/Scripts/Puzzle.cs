using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Puzzles are interactive and can be solved. They also have a color palette.


public class Puzzle : MonoBehaviour
{
    [SerializeField]
    protected ColorPalette colorPalette;
    public ColorPalette GetColorPalette() { return colorPalette; }

    protected bool solved = false;
    public bool IsSolved() { return solved; }


    public virtual void Initialize() { }


    public virtual void InteractionStart(Vector3 position) { }
}
