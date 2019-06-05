using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SlidingPiece))]
public class SlidingPieceEditor : PieceEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }


    protected override void OnSceneGUI()
    {
        base.OnSceneGUI();

        SlidingPiece piece = target as SlidingPiece;
        

        EditorGUI.BeginChangeCheck();
        Vector3 newRailStart = Handles.DoPositionHandle(piece.GetRailStart(), Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(piece, "Move Point");
            EditorUtility.SetDirty(piece);
            piece.SetRailStart(newRailStart);
        }


        EditorGUI.BeginChangeCheck();
        Vector3 newRailEnd = Handles.DoPositionHandle(piece.GetRailEnd(), Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(piece, "Move Point");
            EditorUtility.SetDirty(piece);
            piece.SetRailEnd(newRailEnd);
        }
    }
}
