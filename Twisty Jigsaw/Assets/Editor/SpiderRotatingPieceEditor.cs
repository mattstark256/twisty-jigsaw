using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SpiderRotatingPiece))]
public class SpiderRotatingPieceEditor : RotatingPieceEditor
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
