using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Tooltip("If left blank this is set to Camera.main")]
    private Camera cam;
    public Camera GetCamera() { return cam; }

    [SerializeField]
    private float borderWidth = 1;


    private void Awake()
    {
        if (cam == null) { cam = Camera.main; }
    }


    public void Initialize(Puzzle puzzle)
    {
        cam.transform.position = puzzle.GetCenter() + Vector3.back;

        cam.orthographicSize = (puzzle.GetWidth() / 2 + borderWidth) / cam.aspect;
    }
}
