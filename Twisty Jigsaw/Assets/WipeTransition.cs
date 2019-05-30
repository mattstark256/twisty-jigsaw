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
        wipeImage.material.SetFloat("_AspectRatio", (float)Screen.width / Screen.height);
        wipeImage.enabled = false;
    }


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    DoWipeTransition();
        //}
    }


    public void DoWipeTransition()
    {
        Debug.Log("doing wipe");
        Camera cam = cameraController.GetCamera();
        cam.targetTexture = renderTexture;
        cam.Render();
        cam.targetTexture = null;

        StartCoroutine(WipeCoroutine());
    }


    private IEnumerator WipeCoroutine()
    {
        wipeImage.enabled = true;

        float f = 0;
        while (f < 1)
        {
            //wipeImage.material.SetFloat("_WipeAmount", f);
            wipeImage.material.SetFloat("_WipeAmount", Mathf.Pow(f, 2f));

            yield return null;

            f += Time.deltaTime / wipeDuration;
            if (f > 1) f = 1;
        }

        wipeImage.enabled = false;
    }
}
