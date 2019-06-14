using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(RotatingPiece))]
public class RotatingPieceEditor : PieceEditor
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
