using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpiderRotatingPiece : RotatingPiece
{
    // The spider pieces rotate a bit slower than other pieces.
    private const float durationMultiplier = 1.3f;

    [SerializeField]
    private List<Piece> connectedPieces = new List<Piece>();
    public List<Piece> GetConnectedPieces() { return connectedPieces; }


    // When getting movement area, include all connected pieces.
    protected override void CalculateMovementBounds()
    {
        base.CalculateMovementBounds();

        foreach (Piece piece in connectedPieces)
        {
            if (piece is SpiderRotatingPiece) Debug.Log("Warning! Nested spider pieces are not yet supported and could cause the game to break!");

            movementLowerBounds = Vector2Int.Min(movementLowerBounds, piece.GetMovementLowerBounds() + piece.GetCoOrds() - GetCoOrds());
            movementUpperBounds = Vector2Int.Max(movementUpperBounds, piece.GetMovementUpperBounds() + piece.GetCoOrds() - GetCoOrds());
        }

        int maxRadius = Mathf.Max(
            Mathf.Abs(movementLowerBounds.x),
            Mathf.Abs(movementLowerBounds.y),
            Mathf.Abs(movementUpperBounds.x),
            Mathf.Abs(movementUpperBounds.y));

        movementUpperBounds = Vector2Int.one * maxRadius;
        movementLowerBounds = movementUpperBounds * -1;
    }
    

    // When rotating, rotate all connected pieces. Check if any connected pieces are busy.
    protected override void RotateIfNecessary()
    {
        if (rotationsToDo == 0) return;
        if (isBusy) return;
        if (puzzle.IsSolved()) return;
        foreach (Piece piece in connectedPieces) { if (piece.IsBusy()) return; }

        int sign = (int)Mathf.Sign(rotationsToDo);
        StartRotateCoroutine(sign, true);
        rotationsToDo -= sign;
    }


    protected override IEnumerator RotateCoroutine(int sign, bool playSound)
    {
        if (playSound) gameData.GetSoundEffectManager().PlayEffect("Click 2");

        ModifyOverlaps(-1);
        if (sign == 1) RotateTilesCCW(); else RotateTilesCW();
        ModifyOverlaps(1);
        // Do the same for the connected pieces (also move their coOrds)
        foreach (Piece piece in connectedPieces)
        {
            piece.ModifyOverlaps(-1);
            if (sign == 1) piece.RotateTilesCCW(); else piece.RotateTilesCW();
            Vector2Int relativeCoOrds = piece.GetCoOrds() - GetCoOrds();
            piece.SetCoOrds(GetCoOrds() + new Vector2Int(relativeCoOrds.y * sign * -1, relativeCoOrds.x * sign));
            piece.ModifyOverlaps(1);
        }

        puzzle.UpdateCrosses();

        // Re-parent the connected pieces
        foreach (Piece piece in connectedPieces) { piece.transform.SetParent(transform); }

        isBusy = true;
        foreach (Piece piece in connectedPieces) { piece.SetBusy(true); }
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, sign * 90);
        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / (rotationDuration * durationMultiplier);
            //f += Time.deltaTime / rotationDuration;
            if (f > 1) f = 1;

            float smoothedF = Mathf.SmoothStep(0, 1, f);

            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, smoothedF);

            yield return null;
        }
        isBusy = false;
        foreach (Piece piece in connectedPieces) { piece.SetBusy(false); }

        // Re-parent the connected pieces
        foreach (Piece piece in connectedPieces) { piece.transform.SetParent(puzzle.transform); }
        
        puzzle.CheckIfSolved();
        RotateIfNecessary();
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        foreach (Piece connectedPiece in connectedPieces)
        {
            Gizmos.DrawLine(transform.position, connectedPiece.transform.position);
        }
    }
}
