using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPiece : Piece
{
    private int rotationsToDo = 0;


    public override void StartInteraction(Vector3 position)
    {
        rotationsToDo--;
        if (!isBusy) { StartCoroutine(RotateCoroutine(-1)); }
    }


    private IEnumerator RotateCoroutine(int sign)
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
            f += Time.deltaTime / 0.25f;
            if (f > 1) f = 1;

            float smoothedF = Mathf.SmoothStep(0, 1, f);

            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, smoothedF);

            yield return null;
        }
        isBusy = false;

        puzzle.CheckIfSolved();
        if (puzzle.IsSolved())
        {
            rotationsToDo = 0;
        }
        else
        {
            rotationsToDo -= sign;
            if (rotationsToDo != 0) { StartCoroutine(RotateCoroutine((int)Mathf.Sign(rotationsToDo))); }
        }
    }
}
