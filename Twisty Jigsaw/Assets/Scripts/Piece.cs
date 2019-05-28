using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField]
    private List<Vector2Int> occupiedTiles = new List<Vector2Int>();
    [SerializeField]
    private Color color = Color.white;
    public Color GetColor() { return color; }
    public void SetColor(Color newColor) { color = newColor; }


    private Vector2Int coOrds;
    public Vector2Int GetCoOrds() { return coOrds; }

    // Rotation bounds represent the smallest region that can contain the shape at the default rotation
    private Vector2Int shapeUpperBounds;
    public Vector2Int GetShapeUpperBounds() { return shapeUpperBounds; }
    private Vector2Int shapeLowerBounds;
    public Vector2Int GetShapeLowerBounds() { return shapeLowerBounds; }

    // Rotation bounds represent the smallest region that can contain the shape regardless of rotation
    private Vector2Int rotationUpperBounds;
    public Vector2Int GetRotationUpperBounds() { return rotationUpperBounds; }
    private Vector2Int rotationLowerBounds;
    public Vector2Int GetRotationLowerBounds() { return rotationLowerBounds; }

    private Puzzle puzzle;

    private bool isRotating;
    public bool IsRotating() { return isRotating; }
    private int rotationsToDo = 0;



    private void Awake()
    {
        coOrds = Vector2Int.RoundToInt(transform.localPosition);

        shapeUpperBounds = new Vector2Int(int.MinValue, int.MinValue);
        shapeLowerBounds = new Vector2Int(int.MaxValue, int.MaxValue);
        int maxRadius = 0;
        foreach (Vector2Int tile in occupiedTiles)
        {
            shapeUpperBounds = Vector2Int.Max(shapeUpperBounds, tile);
            shapeLowerBounds = Vector2Int.Min(shapeLowerBounds, tile);
            maxRadius = Mathf.Max(Mathf.Abs(tile.x), Mathf.Abs(tile.y), maxRadius);
        }
        rotationUpperBounds = Vector2Int.one * maxRadius;
        rotationLowerBounds = rotationUpperBounds * -1;

        puzzle = GetComponentInParent<Puzzle>();
    }


    public bool GetTileOccupied(Vector2Int tile)
    {
        return (occupiedTiles.Contains(tile));
    }


    public void SetTileOccupied(Vector2Int tile, bool occupied)
    {
        if (occupiedTiles.Contains(tile))
        { if (!occupied) occupiedTiles.Remove(tile); }
        else
        { if (occupied) occupiedTiles.Add(tile); }
    }


    public List<Vector2Int> GetOccupiedTiles()
    {
        return occupiedTiles;
    }


    public void RotateTilesCW()
    {
        for (int i = 0; i < occupiedTiles.Count; i++)
        {
            occupiedTiles[i] = new Vector2Int(occupiedTiles[i].y, -occupiedTiles[i].x);
        }
    }


    public void RotateTilesCCW()
    {
        for (int i = 0; i < occupiedTiles.Count; i++)
        {
            occupiedTiles[i] = new Vector2Int(-occupiedTiles[i].y, occupiedTiles[i].x);
        }
    }


    public void RotateCW()
    {
        rotationsToDo--;
        if (!isRotating) { StartCoroutine(RotateCoroutine(-1)); }
    }


    public void RotateCCW()
    {
        rotationsToDo++;
        if (!isRotating) { StartCoroutine(RotateCoroutine(1)); }
    }


    private IEnumerator RotateCoroutine(int sign)
    {
        puzzle.RemovePieceFromOverlaps(this);
        if (sign == 1) RotateTilesCCW(); else RotateTilesCW();
        puzzle.AddPieceToOverlaps(this);
        puzzle.UpdateCrosses();

        isRotating = true;
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
        isRotating = false;

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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.3f);
        Gizmos.color = color;
        foreach (Vector2Int tile in occupiedTiles)
        {
            Gizmos.DrawCube(transform.position + (Vector3)(Vector2)tile, Vector3.one);
        }
    }


    // This could be used for testing purposes later on
    //// In certain cases (such as when using Undo) duplicates can exist in occupiedTiles. This checks for duplicates and removes any it finds.
    //public void RemoveDuplicateTiles()
    //{
    //    int i = 0;
    //    while (i < occupiedTiles.Count)
    //    {
    //        int j = 0;
    //        while (j < occupiedTiles.Count)
    //        {
    //            if (j != i && occupiedTiles[i] == occupiedTiles[j])
    //            {
    //                Debug.Log(name + " contains a duplicate tile at " + occupiedTiles[j] + ". Please remove it!");
    //                occupiedTiles.RemoveAt(j);
    //            }
    //            else
    //            { j++; }
    //        }
    //        i++;
    //    }
    //}
}
