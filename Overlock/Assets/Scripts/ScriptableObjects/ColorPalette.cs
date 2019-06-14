using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Color Palette", menuName = "ScriptableObjects/Color Palette", order = 1)]
public class ColorPalette : ScriptableObject
{
    public Color foregroundColor = Color.black;
    public Color backgroundColor = Color.white;
}
