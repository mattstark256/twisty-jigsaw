using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShape : MonoBehaviour
{
    private const float turnDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 26.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Twist(float delay) { StartCoroutine(TwistCoroutine(delay)); }

    private IEnumerator TwistCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        Quaternion initialRotation = transform.localRotation;

        float f = 0;
        while (f<1)
        {
            f += Time.deltaTime / turnDuration;
            if (f > 1) f = 1;

            transform.localRotation = initialRotation * Quaternion.Euler(0, 0, Mathf.SmoothStep(0, 90, f));

            yield return null;
        }
    }
}
