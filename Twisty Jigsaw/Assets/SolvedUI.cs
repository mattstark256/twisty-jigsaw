using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SolvedUI : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve animationCurve;
    [SerializeField]
    private float duration;


    private void Awake()
    {
        StartCoroutine(ScaleCoroutine());
    }


    public void SetColor(Color color)
    {
        foreach(Text text in GetComponentsInChildren<Text>())
        {
            text.color = color;
        }
    }


    private IEnumerator ScaleCoroutine()
    {
        Vector3 defaultScale = transform.localScale;
        
        float f = 0;
        while (f<1)
        {
            f += Time.deltaTime / duration;
            if (f > 1) f = 1;

            transform.localScale = defaultScale * animationCurve.Evaluate(f);

            yield return null;
        }
    }
}
