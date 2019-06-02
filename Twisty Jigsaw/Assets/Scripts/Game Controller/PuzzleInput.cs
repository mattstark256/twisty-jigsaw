using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public enum PressState { pressed, held, released, none };


[RequireComponent(typeof(GameData))]
public class PuzzleInput : MonoBehaviour
{
    private GameData gameController;

    private PressState pointerPressState;
    public PressState GetPointerPressState() { return pointerPressState; }

    private Vector3 pointerScreenPosition;
    public Vector3 GetPointerScreenPosition() { return pointerScreenPosition; }

    private Vector3 pointerWorldPosition;
    public Vector3 GetPointerWorldPosition() { return pointerWorldPosition; }

    private bool pressing = false;


    private void Awake()
    {
        gameController = GetComponent<GameData>();
    }


    public void HandleInput()
    {
        pointerScreenPosition = Input.mousePosition;
        pointerWorldPosition = gameController.GetCameraController().GetCamera().ScreenToWorldPoint(pointerScreenPosition);
        pointerWorldPosition.z = 0;

        bool oldPressing = pressing;

        if (pressing)
        {
            if (!Input.GetButton("Fire1") || PointIsOverUI(pointerScreenPosition))
            {
                pressing = false;
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && !PointIsOverUI(pointerScreenPosition))
            {
                pressing = true;
            }
        }

        pointerPressState =
            pressing ?
                oldPressing ?
                    PressState.held :
                    PressState.pressed :
                oldPressing ?
                    PressState.released :
                    PressState.none;
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
}
