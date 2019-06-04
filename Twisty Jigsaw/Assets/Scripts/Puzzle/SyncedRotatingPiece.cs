using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncedRotatingPiece : RotatingPiece
{
    private bool syncedPiecesCached = false;
    private List<SyncedRotatingPiece> syncedPieces = new List<SyncedRotatingPiece>();

    
    protected override void RotateIfNecessary()
    {
        if (rotationsToDo == 0) return;
        if (isBusy) return;
        if (puzzle.IsSolved()) return;
        if (!syncedPiecesCached) CacheSyncedPieces();
        foreach (SyncedRotatingPiece piece in syncedPieces) { if (piece.IsBusy()) return; }

        int sign = (int)Mathf.Sign(rotationsToDo);
        StartRotateCoroutine(sign);
        foreach (SyncedRotatingPiece piece in syncedPieces) { piece.StartRotateCoroutine(sign); }
        rotationsToDo -= sign;
    }


    private void CacheSyncedPieces()
    {
        foreach (Piece piece in puzzle.GetPieces())
        {
            if (piece != this)
            {
                SyncedRotatingPiece syncedRotatingPiece = piece as SyncedRotatingPiece;
                if (syncedRotatingPiece != null)
                {
                    syncedPieces.Add(syncedRotatingPiece);
                }
            }
        }
        syncedPiecesCached = true;
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.white;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.6f * transform.lossyScale.x);
    }
}
