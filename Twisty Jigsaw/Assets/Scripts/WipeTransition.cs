using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CameraController))]
public class WipeTransition : MonoBehaviour
{
    [SerializeField]
    private Image wipeImage;
    [SerializeField]
    private float wipeDuration = 1f;

    private RenderTexture renderTexture;
    private CameraController cameraController;


    private void Awake()
    {
        cameraController = GetComponent<CameraController>();
    }


    private void Start()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.Default);
        wipeImage.material.SetTexture("_MainTex", renderTexture);
        wipeImage.enabled = false;
    }


    public void DoWipeTransition(Vector3 interactionPosition)
    {
        Debug.Log("doing wipe");
        Camera cam = cameraController.GetCamera();
        cam.targetTexture = renderTexture;
        cam.Render();
        cam.targetTexture = null;

        wipeImage.material.SetVector("_WipeCenter", interactionPosition);

        StartCoroutine(WipeCoroutine());
    }


    private IEnumerator WipeCoroutine()
    {
        wipeImage.enabled = true;

        float f = 0;
        while (f < 1)
        {
            wipeImage.material.SetFloat("_WipeAmount", f);

            // Some other functions I tried
            //wipeImage.material.SetFloat("_WipeAmount", 1 - Mathf.Pow(1 - f, 0.5f));
            //wipeImage.material.SetFloat("_WipeAmount", Mathf.Pow(f, 2f));
            //wipeImage.material.SetFloat("_WipeAmount", 0.3f * Mathf.Tan(f * Mathf.PI * 0.5f));
            //wipeImage.material.SetFloat("_WipeAmount", (Mathf.Pow(f * 10 + 1, 2f) - 1) / 10);

            yield return null;

            f += Time.deltaTime / wipeDuration;
            if (f > 1) f = 1;
        }

        wipeImage.enabled = false;
    }
}
