using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Tileset", menuName = "ScriptableObjects/Tileset", order = 1)]
public class Tileset : ScriptableObject
{
    public GameObject sectionConvexPrefab;
    public GameObject sectionConcavePrefab;
    public GameObject sectionStraightPrefab;
    public GameObject sectionDiagonalPrefab;
    public GameObject sectionSolidPrefab;
}
