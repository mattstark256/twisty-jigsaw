using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Sprite Set", menuName = "ScriptableObjects/Sprite Set", order = 1)]
public class SpriteSet : ScriptableObject
{
    public GameObject pinPrefab;
    public GameObject sectionConvexPrefab;
    public GameObject sectionConcavePrefab;
    public GameObject sectionStraightPrefab;
    public GameObject sectionDiagonalPrefab;
    public GameObject sectionSolidPrefab;
    public Cross crossPrefab;
}
