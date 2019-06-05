using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class FramerateCounter : MonoBehaviour
{
    [SerializeField]
    private Text framerateTextPrefab;
    [SerializeField]
    private Transform framerateParent;
    [SerializeField]
    private int averageSample = 10;

    bool visible = false;
    private Text framerateText;
    
    private List<float> framerates = new List<float>();


    private void Update()
    {
        if (visible)
        {
            float framerate = 1f / Time.deltaTime;
            framerates.Add(framerate);
            if (framerates.Count> averageSample) { framerates.RemoveAt(0); }
            float rollingAverage = framerates.Average();
            
            framerateText.text = "Current framerate: " + (framerate).ToString("F2") + "\nRolling average: " + (rollingAverage).ToString("F2");
        }
    }


    public void SetVisible(bool _visible)
    {
        if (visible)
        {
            if (!_visible) { Destroy(framerateText.gameObject); }
        }
        else
        {
            if (_visible) { framerateText = Instantiate(framerateTextPrefab, framerateParent); }
        }
        visible = _visible;
    }


    public bool GetVisible()
    {
        return visible;
    }
}
