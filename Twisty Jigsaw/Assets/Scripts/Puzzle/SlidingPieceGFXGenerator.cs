using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SlidingPiece))]
public class SlidingPieceGFXGenerator : PieceGFXGenerator
{
    [SerializeField]
    private GameObject railPrefab;
    [SerializeField]
    private GameObject railEndPrefab;
    [SerializeField]
    private GameObject handlePrefab;


    public override void GeneratePieceGFX(Color color)
    {
        base.GeneratePieceGFX(color);

        SlidingPiece slidingPiece = GetComponent<SlidingPiece>();

        Vector3 railVector = slidingPiece.GetRailEnd() - slidingPiece.GetRailStart();
        Quaternion railRotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, railVector, Vector3.forward));

        GameObject rail = Instantiate(railPrefab, slidingPiece.transform.parent);
        rail.transform.localPosition = slidingPiece.GetRailStart() + railVector / 2;
        rail.transform.localRotation = railRotation;
        rail.transform.localScale = new Vector3(1, railVector.magnitude, 1);
        rail.GetComponent<SpriteRenderer>().color = color;

        GameObject railEnd1 = Instantiate(railEndPrefab, slidingPiece.transform.parent);
        GameObject railEnd2 = Instantiate(railEndPrefab, slidingPiece.transform.parent);
        railEnd1.transform.localPosition = slidingPiece.GetRailStart();
        railEnd2.transform.localPosition = slidingPiece.GetRailEnd();
        railEnd1.transform.localRotation = railRotation * Quaternion.Euler(0, 0, 180);
        railEnd2.transform.localRotation = railRotation;
        railEnd1.GetComponent<SpriteRenderer>().color = color;
        railEnd2.GetComponent<SpriteRenderer>().color = color;

        GameObject handle = Instantiate(handlePrefab, slidingPiece.transform);
        handle.transform.localRotation = railRotation;
        handle.GetComponent<SpriteRenderer>().color = color;
    }
}
