using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerInput : MonoBehaviour
{
    [SerializeField]
    private Puzzle puzzle;
    [SerializeField, Tooltip("If left blank this is set to Camera.main")]
    private Camera cam;
    
    void Start()
    {
        if (cam == null) { cam = Camera.main; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            puzzle.PuzzleClicked(cam.ScreenToWorldPoint(Input.mousePosition), 0);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            puzzle.PuzzleClicked(cam.ScreenToWorldPoint(Input.mousePosition), 1);
        }
    }
}
