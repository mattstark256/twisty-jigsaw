using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(Piece))]
public class PieceEditor : Editor
{
    private bool drawState = false;
    

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        Piece piece = (Piece)target;

        GUILayout.Label("Piece Editor", EditorStyles.boldLabel);
        GUILayout.Label("Use right mouse in the scene view to edit the shape.", EditorStyles.label);

        EditorGUILayout.BeginHorizontal();

        EditorGUI.BeginChangeCheck();
        GUILayout.Button("Rotate CCW");
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(piece, "Rotate piece");
            EditorUtility.SetDirty(piece);
            piece.RotateTilesCCW();
        }

        EditorGUI.BeginChangeCheck();
        GUILayout.Button("Rotate CW");
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(piece, "Rotate piece");
            EditorUtility.SetDirty(piece);
            piece.RotateTilesCW();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        Color color = EditorGUILayout.ColorField("Color", piece.GetColor());
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(piece, "Change piece color");
            EditorUtility.SetDirty(piece);
            piece.SetColor(color);
        }

        EditorGUI.BeginChangeCheck();
        GUILayout.Button("Randomize color");
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(piece, "Change piece color");
            EditorUtility.SetDirty(piece);
            Color randomColor = Color.HSVToRGB(Random.value, 1, 1);
            randomColor.a = 0.5f;
            piece.SetColor(randomColor);
        }
    }


    protected virtual void OnSceneGUI()
    {
        Piece piece = (Piece)target;

        // Update the scene view
        SceneView.RepaintAll();

        Event e = Event.current;

        // Get the local tile the mouse is over
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Vector2Int selectedTile = Vector2Int.RoundToInt(piece.transform.InverseTransformPoint(ray.origin));

        Handles.DrawWireCube(piece.transform.TransformPoint((Vector2)selectedTile), Vector3.one);


        // Right mouse button usually is used for moving the camera but I override that. 
        if (e.type == EventType.MouseDown && e.button == 1)
        {
            drawState = !piece.GetTileOccupied(selectedTile);
            Undo.RecordObject(piece, "Change tile");
            EditorUtility.SetDirty(piece);
            piece.SetTileOccupied(selectedTile, drawState);
            e.Use();
        }

        if (e.type == EventType.MouseDrag && e.button == 1)
        {
            if (piece.GetTileOccupied(selectedTile) != drawState)
            {
                Undo.RecordObject(piece, "Change tile");
                EditorUtility.SetDirty(piece);
                piece.SetTileOccupied(selectedTile, drawState);
            }
            e.Use();
        }
    }
}
