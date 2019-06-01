using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseCircle : MonoBehaviour
{
    [SerializeField]
    private float pulseDuration = 1;
    
    void Start()
    {
        StartCoroutine(PulseAnimation());
    }

    private IEnumerator PulseAnimation()
    {
        SpriteRenderer image = GetComponent<SpriteRenderer>();

        float f = 0;
        while(f<1)
        {
            f += Time.deltaTime / pulseDuration;
            if (f > 1) f = 1;

            image.material.SetFloat("_PulseAmount", f);

            yield return null;
        }

        Destroy(gameObject);
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().material.SetColor("_Color", color);
    }
}
