using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("If left blank this is set to Camera.main")]
    private Camera cam;
    public Camera GetCamera() { return cam; }
    

    private void Awake()
    {
        if (cam == null) { cam = Camera.main; }

        // Make the view area width 1
        cam.orthographicSize = 0.5f / cam.aspect;

    }

    
    public void Initialize(Puzzle puzzle)
    {
        cam.backgroundColor = puzzle.GetColorPalette().backgroundColor;
    }
}
