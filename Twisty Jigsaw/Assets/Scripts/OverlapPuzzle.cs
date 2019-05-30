using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PieceGFXGenerator))]
public class OverlapPuzzle : Puzzle
{
    [SerializeField]
    private SpriteSet spriteSet;

    private const float clickRadius = 1.5f;

    private Piece[] pieces;

    private int[,] overlaps;
    private Vector2Int overlapsArrayCorner;
    
    private Cross[,] crosses;


    public override void Initialize()
    {
        base.Initialize();

        // Find all the pieces
        pieces = GetComponentsInChildren<Piece>();

        // Create the overlaps array
        Vector2Int arrayUpperBounds = new Vector2Int(int.MinValue, int.MinValue);
        Vector2Int arrayLowerBounds = new Vector2Int(int.MaxValue, int.MaxValue);
        foreach (Piece piece in pieces)
        {
            arrayUpperBounds = Vector2Int.Max(arrayUpperBounds, piece.GetCoOrds() + piece.GetRotationUpperBounds());
            arrayLowerBounds = Vector2Int.Min(arrayLowerBounds, piece.GetCoOrds() + piece.GetRotationLowerBounds());
        }
        overlapsArrayCorner = arrayLowerBounds;
        Vector2Int arraySize = arrayUpperBounds - arrayLowerBounds + Vector2Int.one;
        overlaps = new int[arraySize.x, arraySize.y];

        // Populate the overlaps array
        foreach (Piece piece in pieces)
        {
            AddPieceToOverlaps(piece);
        }

        // Generate the piece GFX
        PieceGFXGenerator pieceGFXGenerator = GetComponent<PieceGFXGenerator>();
        foreach (Piece piece in pieces)
        {
            pieceGFXGenerator.GeneratePieceGFX(piece, spriteSet, colorPalette.foregroundColor);
        }

        // Generate the cross array
        crosses = new Cross[arraySize.x, arraySize.y];
        for (int x = 0; x < crosses.GetLength(0); x++)
        {
            for (int y = 0; y < crosses.GetLength(1); y++)
            {
                Cross newCross = Instantiate(spriteSet.crossPrefab, transform);
                newCross.transform.localPosition = (Vector3)(Vector2)overlapsArrayCorner + new Vector3(x, y, 0);
                newCross.SetSize((overlaps[x, y] > 1) ? 1 : 0, false);
                newCross.GetComponent<SpriteRenderer>().color = colorPalette.foregroundColor;
                crosses[x, y] = newCross;
            }
        }

        // Scale and position the puzzle to fit in the view area. The view area always has width 1.
        Vector3 center = arrayLowerBounds + (arraySize - Vector2.one) / 2;
        float width = arraySize.x + 2;
        transform.localScale = Vector3.one / width;
        transform.localPosition = -center / width;
    }



    public override void InteractionStart(Vector3 position)
    {
        base.InteractionStart(position);

        if (solved) return;

        Vector2 localPosition = transform.InverseTransformPoint(position);

        Piece closestPiece = null;
        float shortestDistance = float.MaxValue;
        foreach (Piece piece in pieces)
        {
            float distance = Vector2.Distance(localPosition, piece.GetCoOrds());
            if (distance < shortestDistance)
            {
                closestPiece = piece;
                shortestDistance = distance;
            }
        }
        if (closestPiece != null && shortestDistance < clickRadius)
        {
            closestPiece.RotateCW();
        }
    }


    public void AddPieceToOverlaps(Piece piece)
    {
        foreach (Vector2Int occupiedTile in piece.GetOccupiedTiles())
        {
            Vector2Int overlapTile = occupiedTile + piece.GetCoOrds() - overlapsArrayCorner;
            overlaps[overlapTile.x, overlapTile.y]++;
        }
    }


    public void RemovePieceFromOverlaps(Piece piece)
    {
        foreach (Vector2Int occupiedTile in piece.GetOccupiedTiles())
        {
            Vector2Int overlapTile = occupiedTile + piece.GetCoOrds() - overlapsArrayCorner;
            overlaps[overlapTile.x, overlapTile.y]--;
        }
    }


    public void UpdateCrosses()
    {
        for (int x = 0; x < crosses.GetLength(0); x++)
        {
            for (int y = 0; y < crosses.GetLength(1); y++)
            {
                crosses[x, y].SetSize((overlaps[x, y] > 1) ? 1 : 0, true);
            }
        }
    }


    public void CheckIfSolved()
    {
        foreach (Piece piece in pieces)
        {
            if (piece.IsRotating())
            {
                solved = false;
                return;
            }
        }

        for (int x = 0; x < overlaps.GetLength(0); x++)
        {
            for (int y = 0; y < overlaps.GetLength(1); y++)
            {
                if (overlaps[x, y] > 1)
                {
                    solved = false;
                    return;
                }
            }
        }

        solved = true;
        Debug.Log("puzzle solved");
    }
}
