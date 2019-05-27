using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(Piece))]
public class PieceEditor : Editor
{
    bool drawState = false;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Piece piece = (Piece)target;

        EditorGUI.BeginChangeCheck();
        GUILayout.Button("Rotate clockwise");
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(piece, "Rotate piece");
            piece.RotateTilesCW();
            EditorUtility.SetDirty(piece);
        }
    }


    private void OnSceneGUI()
    {
        Piece piece = (Piece)target;

        // Update the scene view
        SceneView.RepaintAll();

        // Override normal mouse interaction (This doesn't seem to be working...)
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        Event e = Event.current;

        // Get the local tile the mouse is over
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Vector2Int selectedTile = Vector2Int.RoundToInt(piece.transform.InverseTransformPoint(ray.origin));

        Handles.DrawWireCube(piece.transform.TransformPoint((Vector2)selectedTile), Vector3.one);


        if (e.type == EventType.MouseDown && e.button == 0)
        {
            drawState = !piece.GetTileOccupied(selectedTile);
            Undo.RecordObject(piece, "Change tile");
            piece.SetTileOccupied(selectedTile, drawState);
            EditorUtility.SetDirty(piece);
        }

        if (e.type == EventType.MouseDrag && e.button == 0)
        {
            if (piece.GetTileOccupied(selectedTile) != drawState)
            {
                Undo.RecordObject(piece, "Change tile");
                piece.SetTileOccupied(selectedTile, drawState);
                EditorUtility.SetDirty(piece);
            }
        }
    }
}
