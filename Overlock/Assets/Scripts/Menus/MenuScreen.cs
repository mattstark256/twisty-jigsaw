using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    protected GameData gameData;

    public virtual void Initialize(GameData _gameData)
    {
        gameData = _gameData;
    }
}
