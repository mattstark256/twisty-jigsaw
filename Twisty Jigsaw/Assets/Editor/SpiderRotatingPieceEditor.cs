using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SpiderRotatingPiece))]
public class SpiderRotatingPieceEditor : RotatingPieceEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpiderRotatingPiece spiderPiece = (SpiderRotatingPiece)target;

        EditorGUI.BeginChangeCheck();
        GUILayout.Button("Rotate spider");
        if (EditorGUI.EndChangeCheck())
        {
            foreach(Piece piece in spiderPiece.GetConnectedPieces())
            {
                if (piece == null) continue;

                Undo.RecordObject(piece, "Rotate piece");
                EditorUtility.SetDirty(piece);
                piece.RotateTilesCW();
                Vector3 relativePosition = piece.transform.localPosition - spiderPiece.transform.localPosition;
                piece.transform.localPosition = spiderPiece.transform.localPosition + new Vector3(relativePosition.y, -relativePosition.x, relativePosition.z);
            }
        }
    }


    protected override void OnSceneGUI()
    {
        base.OnSceneGUI();
    }
}
