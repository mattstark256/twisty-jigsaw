using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PieceGFXGenerator))]
public class Puzzle : MonoBehaviour
{
    private Piece[] pieces;

    private int[,] overlaps;
    private Vector2Int overlapsArrayCorner;

    [SerializeField]
    private Cross crossPrefab;
    private Cross[,] crosses;

    [SerializeField]
    private float clickRadius = 1.5f;
    [SerializeField]
    private Color color = Color.magenta;

    private Vector3 center;
    public Vector3 GetCenter() { return center; }
    private float width;
    public float GetWidth() { return width; }

    private bool solved = false;
    public bool IsSolved() { return solved; }



    public void Initialize()
    {
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
            pieceGFXGenerator.GeneratePieceGFX(piece, color);
        }

        // Generate the cross array
        crosses = new Cross[arraySize.x, arraySize.y];
        for (int x = 0; x < crosses.GetLength(0); x++)
        {
            for (int y = 0; y < crosses.GetLength(1); y++)
            {
                Cross newCross = Instantiate(crossPrefab, transform);
                newCross.transform.localPosition = (Vector3)(Vector2)overlapsArrayCorner + new Vector3(x, y, 0);
                newCross.SetSize((overlaps[x, y] > 1) ? 1 : 0, false);
                newCross.GetComponent<SpriteRenderer>().color = color;
                crosses[x, y] = newCross;
            }
        }

        // Find the center and width
        center = transform.position + (Vector3)(arrayLowerBounds + (arraySize - Vector2.one) / 2);
        width = arraySize.x;
    }



    public void PuzzleClicked(Vector3 clickPosition, bool clockwise)
    {
        if (solved) return;

        Vector2 localClickPosition = transform.InverseTransformPoint(clickPosition);

        Piece closestPiece = null;
        float shortestDistance = float.MaxValue;
        foreach (Piece piece in pieces)
        {
            float distance = Vector2.Distance(localClickPosition, piece.GetCoOrds());
            if (distance < shortestDistance)
            {
                closestPiece = piece;
                shortestDistance = distance;
            }
        }
        if (closestPiece != null && shortestDistance < clickRadius)
        {
            if (clockwise) { closestPiece.RotateCW(); } else { closestPiece.RotateCCW(); }
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
