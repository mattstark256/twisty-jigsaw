using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeOut : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 1;


    private void Start()
    {
        StartCoroutine(FadeOutCoroutine());
    }
    

    private IEnumerator FadeOutCoroutine()
    {
        Image image = GetComponent<Image>();
        Color initialColor = image.color;

        float f = 0;
        while (f<1)
        {
            f += Time.deltaTime / fadeDuration;
            if (f > 1) f = 1;

            image.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.SmoothStep(initialColor.a, 0, f));

            yield return null;
        }

        Destroy(gameObject);
    }
}
