using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMenuButton : MonoBehaviour
{
    [SerializeField]
    private Image lines;

    private GameData gameData;

    public void Initialize(GameData _gameData)
    {
        gameData = _gameData;
    }

    public void SetColor(Color color)
    {
        lines.color = color;
    }

    public void OpenMenu()
    {
        gameData.GetMenuManager().OpenMainMenu();
    }
}
