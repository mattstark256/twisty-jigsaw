﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackdrop : MonoBehaviour
{
    [SerializeField]
    private MenuShape menuShapePrefab;

    [SerializeField]
    private float shapeSpacing = 100;

    private Vector2Int gridSize;
    private MenuShape[,] menuShapes;

    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null) { Debug.LogWarning("No canvas found in parent of MenuBackdrop!"); }
        Rect canvasRect = canvas.GetComponent<RectTransform>().rect;
        Vector2 canvasSize = new Vector2(canvasRect.width, canvasRect.height);
        Debug.Log(canvasSize);
        gridSize = new Vector2Int(Mathf.CeilToInt(canvasSize.x / shapeSpacing) + 1, Mathf.CeilToInt(canvasSize.y / shapeSpacing) + 1);
        menuShapes = new MenuShape[gridSize.x, gridSize.y];

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                MenuShape menuShape = Instantiate(menuShapePrefab, transform);
                menuShape.transform.localPosition = new Vector3(x * shapeSpacing - canvasSize.x / 2, y * shapeSpacing - canvasSize.y / 2, 0);
                menuShapes[x, y] = menuShape;
            }
        }

        StartCoroutine(TwistCoroutine());
    }


    private IEnumerator TwistCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                menuShapes[x, y].Twist(x * 0.15f + y * 0.27f);
            }
        }

        yield return new WaitForSeconds(4);
        
        StartCoroutine(TwistCoroutine());
    }

}
