using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(OverlapPuzzle))]
public class OverlapPuzzleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        OverlapPuzzle puzzle = (OverlapPuzzle)target;
        EditorGUI.BeginChangeCheck();
        GUILayout.Button("Rotate CW");
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObjects(puzzle.GetComponentsInChildren<Piece>(), "Rotate puzzle");
            foreach(Piece piece in puzzle.GetComponentsInChildren<Piece>())
            {
                EditorUtility.SetDirty(piece);

                piece.transform.localPosition = new Vector3(piece.transform.localPosition.y, -piece.transform.localPosition.x, piece.transform.localPosition.z);

                RotatingPiece rotatingPiece = piece as RotatingPiece;
                if (rotatingPiece != null)
                {
                    rotatingPiece.RotateTilesCW();
                }
            }
        }
    }
}
