using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerInput : MonoBehaviour
{
    public void HandleInput(Camera cam, Puzzle puzzle)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            puzzle.PuzzleClicked(cam.ScreenToWorldPoint(Input.mousePosition), true);
        }
        //if (Input.GetButtonDown("Fire2"))
        //{
        //    puzzle.PuzzleClicked(cam.ScreenToWorldPoint(Input.mousePosition), true);
        //}
    }
}
