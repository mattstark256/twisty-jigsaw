using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// This class handles puzzle input. It serves two purposes:
// 1. Ignoring inputs that are over UI elements and ending inputs that move over UI elements.
// 2. Handling touch and mouse inputs differently. If there are multiple touches mousePosition would give you the average, which isn't what I want. Instead I keep track of one touch at a time.


public enum PressState { pressed, held, released, none };


[RequireComponent(typeof(GameData))]
public class PuzzleInput : MonoBehaviour
{
    private GameData gameData;

    private PressState pointerPressState;
    public PressState GetPointerPressState() { return pointerPressState; }
    private Vector3 pointerScreenPosition;
    public Vector3 GetPointerScreenPosition() { return pointerScreenPosition; }
    private Vector3 pointerWorldPosition;
    public Vector3 GetPointerWorldPosition() { return pointerWorldPosition; }

    private bool fingerDown = false;
    private int fingerID;
    private bool mouseDown = false;


    private void Awake()
    {
        gameData = GetComponent<GameData>();
    }


    // This should be called every frame by PuzzleManager
    public void HandleInput()
    {
        if (fingerDown || mouseDown)
        {
            pointerPressState = PressState.held;

            // Check for end of interaction
            if (fingerDown)
            {
                Touch touch = GetTouchByFingerID(fingerID);
                if (touch.phase == TouchPhase.Ended || PointIsOverUI(touch.position))
                {
                    fingerDown = false;
                    pointerPressState = PressState.released;
                }
            }
            if (mouseDown)
            {
                if (Input.GetMouseButtonUp(0) || PointIsOverUI(Input.mousePosition))
                {
                    mouseDown = false;
                    pointerPressState = PressState.released;
                }
            }
        }
        else
        {
            pointerPressState = PressState.none;

            // Check for start of interaction
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && !PointIsOverUI(touch.position))
                {
                    fingerDown = true;
                    fingerID = touch.fingerId;
                    pointerPressState = PressState.pressed;
                    break;
                }
            }
            if (!fingerDown && Input.GetMouseButtonDown(0) && !PointIsOverUI(Input.mousePosition))
            {
                mouseDown = true;
                pointerPressState = PressState.pressed;
            }
        }


        // Update pointer position
        if (mouseDown) { pointerScreenPosition = Input.mousePosition; }
        if (fingerDown) { pointerScreenPosition = GetTouchByFingerID(fingerID).position; }
        pointerWorldPosition = gameData.GetCameraController().GetCamera().ScreenToWorldPoint(pointerScreenPosition);
        pointerWorldPosition.z = 0;
    }


    // https://www.reddit.com/r/Unity3D/comments/2zc2xa/how_to_exlude_ui_from_touch_input/cphkhyo/
    private static List<RaycastResult> tempRaycastResults = new List<RaycastResult>();
    public bool PointIsOverUI(Vector2 point)
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = point;
        tempRaycastResults.Clear();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, tempRaycastResults);
        return tempRaycastResults.Count > 0;
    }


    // This function is required because Input.GetTouch() uses the array index of current touches, not the fingerID
    private Touch GetTouchByFingerID(int id)
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.fingerId == id)
            {
                return touch;
            }
        }
        Debug.Log("can't find touch by fingerId! returning touch 0 instead");
        return Input.GetTouch(0);
    }
}
