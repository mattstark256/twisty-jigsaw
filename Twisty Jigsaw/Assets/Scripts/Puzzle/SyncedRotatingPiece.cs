using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncedRotatingPiece : RotatingPiece
{
    // This is used to prevent recursion when rotating all the synced pieces.
    private bool spreadSuppressed = false;


    public void SuppressSpread()
    {
        spreadSuppressed = true;
    }


    public override void StartInteraction(Vector3 position)
    {
        base.StartInteraction(position);

        if (spreadSuppressed)
        {
            spreadSuppressed = false;
            return;
        }

        foreach (Piece piece in puzzle.GetPieces())
        {
            if (piece != this)
            {
                SyncedRotatingPiece syncedRotatingPiece = piece as SyncedRotatingPiece;
                if (syncedRotatingPiece != null)
                {
                    syncedRotatingPiece.SuppressSpread();
                    syncedRotatingPiece.StartInteraction(position);
                }
            }
        }
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.6f * transform.lossyScale.x);
    }
}
