using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : MonoBehaviour
{
    private float size = 1;


    public void SetSize(float newSize, bool doAnimation)
    {
        StopAllCoroutines();
        if (doAnimation)
        {
            StartCoroutine(SetSizeCoroutine(newSize));
        }
        else
        {
            size = newSize;
            transform.localScale = Vector3.one * size;
        }
    }


    private IEnumerator SetSizeCoroutine(float newSize)
    {
        while (size != newSize)
        {
            float sizeDelta = newSize - size;
            float maxDelta = Time.deltaTime / 0.25f;
            if (Mathf.Abs(sizeDelta) > maxDelta) { sizeDelta = Mathf.Sign(sizeDelta) * maxDelta; }
            size += sizeDelta;

            transform.localScale = Vector3.one * size;

            yield return null;
        }
    }
}
