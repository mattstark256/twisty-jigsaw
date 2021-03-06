﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Piece))]
public class PieceGFXGenerator : MonoBehaviour
{
    [SerializeField]
    private Tileset tileset;
    [SerializeField]
    public GameObject pinPrefab;

    public virtual void GeneratePieceGFX(Color color)
    {
        Piece piece = GetComponent<Piece>();

        // Generate the pin at the center of a piece
        if (pinPrefab != null)
        {
            GameObject pin = Instantiate(pinPrefab);
            pin.transform.parent = piece.transform;
            pin.transform.localPosition = Vector3.zero;
            pin.GetComponent<SpriteRenderer>().color = color;
        }


        // Generate the sections that make up the piece
        Vector2Int pieceSize = piece.GetShapeUpperBounds() - piece.GetShapeLowerBounds();
        Vector2Int arrayCorner = piece.GetShapeLowerBounds() - Vector2Int.one;
        bool[,] tileStates = new bool[pieceSize.x + 3, pieceSize.y + 3];
        foreach (Vector2Int occupiedTile in piece.GetOccupiedTiles())
        {
            tileStates[occupiedTile.x - arrayCorner.x, occupiedTile.y - arrayCorner.y] = true;
        }

        for (int x = 0; x < tileStates.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < tileStates.GetLength(1) - 1; y++)
            {
                int tileCode = 0;
                if (tileStates[x, y]) tileCode += 1;
                if (tileStates[x, y + 1]) tileCode += 2;
                if (tileStates[x + 1, y]) tileCode += 4;
                if (tileStates[x + 1, y + 1]) tileCode += 8;

                GameObject section = null;
                float sectionAngle = 0;

                switch (tileCode)
                {
                    case 0:
                        break;
                    case 1:
                        section = Instantiate(tileset.sectionConvexPrefab);
                        sectionAngle = 0;
                        break;
                    case 2:
                        section = Instantiate(tileset.sectionConvexPrefab);
                        sectionAngle = 270;
                        break;
                    case 3:
                        section = Instantiate(tileset.sectionStraightPrefab);
                        sectionAngle = 270;
                        break;
                    case 4:
                        section = Instantiate(tileset.sectionConvexPrefab);
                        sectionAngle = 90;
                        break;
                    case 5:
                        section = Instantiate(tileset.sectionStraightPrefab);
                        sectionAngle = 0;
                        break;
                    case 6:
                        section = Instantiate(tileset.sectionDiagonalPrefab);
                        sectionAngle = 90;
                        break;
                    case 7:
                        section = Instantiate(tileset.sectionConcavePrefab);
                        sectionAngle = 0;
                        break;
                    case 8:
                        section = Instantiate(tileset.sectionConvexPrefab);
                        sectionAngle = 180;
                        break;
                    case 9:
                        section = Instantiate(tileset.sectionDiagonalPrefab);
                        sectionAngle = 0;
                        break;
                    case 10:
                        section = Instantiate(tileset.sectionStraightPrefab);
                        sectionAngle = 180;
                        break;
                    case 11:
                        section = Instantiate(tileset.sectionConcavePrefab);
                        sectionAngle = 270;
                        break;
                    case 12:
                        section = Instantiate(tileset.sectionStraightPrefab);
                        sectionAngle = 90;
                        break;
                    case 13:
                        section = Instantiate(tileset.sectionConcavePrefab);
                        sectionAngle = 90;
                        break;
                    case 14:
                        section = Instantiate(tileset.sectionConcavePrefab);
                        sectionAngle = 180;
                        break;
                    default:
                        section = Instantiate(tileset.sectionSolidPrefab);
                        sectionAngle = 0;
                        break;
                }

                if (section != null)
                {
                    section.transform.parent = piece.transform;
                    section.transform.localPosition = piece.GetShapeLowerBounds() + new Vector2(x, y) - Vector2.one * 0.5f;
                    section.transform.localRotation = Quaternion.Euler(0, 0, sectionAngle);
                    section.GetComponent<SpriteRenderer>().color = color;
                }
            }
        }
    }
}
