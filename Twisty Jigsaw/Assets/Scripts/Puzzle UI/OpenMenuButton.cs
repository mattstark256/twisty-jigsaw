using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenuButton : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image image;

    private GameData gameData;

    public void Initialize(GameData _gameData)
    {
        gameData = _gameData;
    }

    public void SetColors(Color foregroundColor, Color backgroundColor)
    {
        text.color = backgroundColor;
        image.color = foregroundColor;
    }

    public void OpenMenu()
    {
        gameData.GetMenuManager().OpenMainMenu();
    }
}
