using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(GameController))]
public class PointerInput : MonoBehaviour
{
    GameController gameController;


    private void Awake()
    {
        gameController = GetComponent<GameController>();
    }


    public void HandleInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //gameController.StartInteraction(cam.ScreenToWorldPoint(Input.mousePosition));
            gameController.StartInteraction(Input.mousePosition);
        }
    }
}
