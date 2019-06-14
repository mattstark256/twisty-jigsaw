using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SyncedRotatingPiece))]
public class SyncedRotatingPieceEditor : RotatingPieceEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }


    protected override void OnSceneGUI()
    {
        base.OnSceneGUI();
    }
}
