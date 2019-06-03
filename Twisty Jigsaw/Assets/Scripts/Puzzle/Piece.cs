using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Piece : MonoBehaviour
{
    // These need to be serialized in order to be saved
    [SerializeField, HideInInspector]
    private List<Vector2Int> occupiedTiles = new List<Vector2Int>();
    [SerializeField, HideInInspector]
    private Color editorColor = Color.magenta;
    public Color GetEditorColor() { return editorColor; }
    public void SetEditorColor(Color newEditorColor) { editorColor = newEditorColor; }


    private Vector2Int coOrds;
    public Vector2Int GetCoOrds() { return coOrds; }

    // Shape bounds represent the region occupied by the shape in local space
    private Vector2Int shapeUpperBounds;
    public Vector2Int GetShapeUpperBounds() { return shapeUpperBounds; }
    private Vector2Int shapeLowerBounds;
    public Vector2Int GetShapeLowerBounds() { return shapeLowerBounds; }

    // Movement bounds represent the region occupied by the shape in puzzle space when the piece can be moved or rotated
    private Vector2Int movementUpperBounds;
    public Vector2Int GetMovementUpperBounds() { return movementUpperBounds; }
    private Vector2Int movementLowerBounds;
    public Vector2Int GetMovementLowerBounds() { return movementLowerBounds; }

    protected OverlapPuzzle puzzle;

    protected bool isBusy;
    public bool IsBusy() { return isBusy; }



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
        movementUpperBounds = Vector2Int.one * maxRadius;
        movementLowerBounds = movementUpperBounds * -1;

        puzzle = GetComponentInParent<OverlapPuzzle>();
    }


    public List<Vector2Int> GetOccupiedTiles()
    {
        return occupiedTiles;
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


    public void ModifyOverlaps(int amount)
    {
        foreach (Vector2Int occupiedTile in occupiedTiles)
        {
            puzzle.ModifyOverlaps(coOrds + occupiedTile, amount);
        }
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


    public virtual void StartInteraction(Vector3 position)
    {
    }


    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.3f * transform.lossyScale.x);
        Gizmos.color = editorColor;
        foreach (Vector2Int tile in occupiedTiles)
        {
            Gizmos.DrawCube(transform.position + (Vector3)(Vector2)tile * transform.lossyScale.x, Vector3.one * transform.lossyScale.x);
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
