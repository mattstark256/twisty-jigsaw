using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpiderRotatingPiece))]
public class SpiderPieceGFXGenerator : PieceGFXGenerator
{
    [SerializeField]
    private GameObject spiderArmPrefab;

    private const float armStartRadius = 0.2f;
    private const float armWidth = 0.1f;


    public override void GeneratePieceGFX(Color color)
    {
        base.GeneratePieceGFX(color);

        SpiderRotatingPiece spiderPiece = GetComponent<SpiderRotatingPiece>();

        foreach(Piece piece in spiderPiece.GetConnectedPieces())
        {
            GameObject spiderArm = Instantiate(spiderArmPrefab, spiderPiece.transform);

            Vector3 armEndPoint = piece.transform.localPosition - spiderPiece.transform.localPosition;
            Vector3 armStartPoint = (armEndPoint).normalized * armStartRadius;
            Vector3 armVector = armEndPoint - armStartPoint;
            spiderArm.transform.localPosition = armStartPoint + armVector / 2;
            spiderArm.transform.localRotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.up, armVector, Vector3.forward));
            spiderArm.transform.localScale = new Vector3(armWidth, armVector.magnitude, 1);

            spiderArm.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
