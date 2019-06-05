using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPiece : Piece
{
    protected int rotationsToDo = 0;
    protected const float rotationDuration = 0.25f;


    private void Update()
    {
        RotateIfNecessary();
    }


    protected override void CalculateMovementBounds()
    {
        if (!shapeBoundsCached) CalculateShapeBounds();
        int maxRadius = Mathf.Max(
            Mathf.Abs(shapeLowerBounds.x),
            Mathf.Abs(shapeLowerBounds.y),
            Mathf.Abs(shapeUpperBounds.x),
            Mathf.Abs(shapeUpperBounds.y));
        movementUpperBounds = Vector2Int.one * maxRadius;
        movementLowerBounds = movementUpperBounds * -1;
        movementBoundsCached = true;
    }


    public override void StartInteraction(Vector3 position)
    {
        rotationsToDo--;
        RotateIfNecessary();
    }


    protected virtual void RotateIfNecessary()
    {
        if (rotationsToDo == 0) return;
        if (isBusy) return;
        if (puzzle.IsSolved()) return;

        int sign = (int)Mathf.Sign(rotationsToDo);
        StartRotateCoroutine(sign);
        rotationsToDo -= sign;
    }


    public void StartRotateCoroutine(int sign) { StartCoroutine(RotateCoroutine(sign)); }
    protected virtual IEnumerator RotateCoroutine(int sign)
    {
        ModifyOverlaps(-1);
        if (sign == 1) RotateTilesCCW(); else RotateTilesCW();
        ModifyOverlaps(1);
        puzzle.UpdateCrosses();

        isBusy = true;
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, sign * 90);
        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / rotationDuration;
            if (f > 1) f = 1;

            float smoothedF = Mathf.SmoothStep(0, 1, f);

            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, smoothedF);

            yield return null;
        }
        isBusy = false;

        puzzle.CheckIfSolved();
        RotateIfNecessary();
    }
}
