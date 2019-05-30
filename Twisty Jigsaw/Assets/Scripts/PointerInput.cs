using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerInput : MonoBehaviour
{
    public void HandleInput(Camera cam, Puzzle puzzle)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            puzzle.InteractionStart(cam.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
